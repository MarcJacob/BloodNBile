using System;
using System.Collections.Generic;
using UnityEngine;

public class MagesManager
{
    public List<Mage> Mages;
    EntityManager EntityModule; // EntityManager associé à ce MagesManager.

    public MagesManager(EntityManager module)
    {
        EntityModule = module;
        Mages = new List<Mage>();
        EntityModule.RegisterOnUnitDeathCallback(OnUnitDeath);
    }

    void OnUnitDeath(Unit unit)
    {
        Mage DeadMage = null;
        foreach(Mage m in Mages)
        {
            if (m.ID == unit.ID)
            {
                DeadMage = m;
                OnMageDied(m);
                return;
            }
        }
        if (DeadMage != null)
            Mages.Remove(DeadMage);
    }

    public int CreateMage(Vector3 pos, string name, Faction fac)
    {
        Mage newMage = new Mage(EntityModule.Match, EntityModule.GetAllEntities().Length, pos, Quaternion.identity, name, fac, new HumorLevels(100, 100, 100, 100));
        EntityModule.OnUnitCreated(newMage, false);
        Mages.Add(newMage);
        OnMageCreated(newMage);
        return newMage.ID;
    }

    public void OnMageCreated(Mage mage)
    {
        EntityModule.Match.SendMessageToPlayers(13, mage, false, true);
    }

    public void OnClientEntityUpdate(NetworkMessageReceiver message)
    {
        if (EntityModule.Match.IsInMatch(message.ConnectionID))
        {
            EntityPositionRotationUpdate messageContent = (EntityPositionRotationUpdate)message.ReceivedMessage.Content;

            Unit unit = EntityModule.GetUnitFromID(messageContent.EntityID);
            if (unit != null)
            {
                unit.SetPos(messageContent.NewPosition);
                unit.SetRot(messageContent.NewRotation);
            }
        }
    }

    public void OnClientMageCasting(NetworkMessageReceiver message)
    {
        if (EntityModule.Match.IsInMatch(message.ConnectionID))
        {
            bool isCastable;
            Spell spell = Spell.GetSpellFromID(((ClientMageSpellMessage)message.ReceivedMessage.Content).SpellID);
            Mage mage = (Mage)EntityModule.GetUnitFromID(((ClientMageSpellMessage)message.ReceivedMessage.Content).MageID);
            if (mage != null)
            {
                isCastable = spell.IsCastable(mage);
                if (isCastable && mage.Humors != null)
                {
                    spell.Cast(EntityModule.Match, mage);
                    mage.IsCasting = true;
                    mage.ChangeHumor(spell.Humor, -spell.Cost);
                    mage.IsCasting = false;
                    mage.ReloadingSpells.Add(spell, spell.Cooldown);
                    Debug.Log("Les humeurs selon le server : " + mage.Humors);
                    EntityModule.Match.SendMessageToPlayers(21, new ClientMageSpellMessage(mage.ID, spell.ID));
                }
            }
        }
    }

    float MageUpdatesToClientPerSecond = 10f;
    float cd_MageUpdatesToClient = 0f;
    public void UpdateMages()
    {
        // Mise à jour des cooldowns
        foreach (Mage m in Mages)
        {
            m.UpdateLOP();
            if(m.ReloadingSpells.Count != 0)
                m.UpdateCooldowns();
        }

        if (1 / MageUpdatesToClientPerSecond < cd_MageUpdatesToClient)
        {
            int[] IDs = new int[Mages.Count];
            Dictionary<int, float>[] cds = new Dictionary<int, float>[Mages.Count];
            HumorLevels[] humorLevels = new HumorLevels[Mages.Count];

            for(int i = 0; i < Mages.Count; i++)
            {
                Mage m = Mages[i];
                IDs[i] = m.ID;
                Dictionary<int, float> cdsInt = new Dictionary<int, float>();
                foreach(Spell sp in m.ReloadingSpells.Keys)
                {
                    cdsInt.Add(sp.ID, m.ReloadingSpells[sp]);
                }
                cds[i] = cdsInt;
                humorLevels[i] = m.Humors;
            }
            EntityModule.Match.SendMessageToPlayers(23, new MageUpdateMessage(IDs, cds, humorLevels), true, true);
            cd_MageUpdatesToClient = 0f;
        }
        else
        {
            cd_MageUpdatesToClient += Time.deltaTime;
        }
    }

    Action<Mage> OnMageDied;
    public void RegisterOnMageDiedCallback(Action<Mage> cb)
    {
        OnMageDied += cb;
    }
}
