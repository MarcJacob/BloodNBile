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
    }


    public int CreateMage(Vector3 pos, string name, Faction fac)
    {
        Mage newMage = new Mage(EntityModule.Match, EntityModule.GetAllEntities().Length, pos, Quaternion.identity, name, fac, new HumorLevels(100, 100, 100, 100));
        EntityModule.Entities.Add(newMage);
        EntityModule.Units.Add(newMage);
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
        bool isCastable;
        Spell spell = Spell.GetSpellFromID(((ClientMageSpellMessage) message.ReceivedMessage.Content).SpellID);
        Mage mage = (Mage) EntityModule.GetUnitFromID(((ClientMageSpellMessage)message.ReceivedMessage.Content).MageID);
        isCastable = spell.IsCastable(mage);
        if(isCastable && mage.Humors != null)
        {
            spell.Cast(mage);
            mage.IsCasting = true;
            mage.LoseHumor(spell.Humor, spell.Cost);
            mage.IsCasting = false;
            mage.ReloadingSpells.Add(spell, spell.Cooldown);
            Debug.Log("Les humeurs selon le server : " + mage.Humors);
            EntityModule.Match.SendMessageToPlayers(21, new ClientMageSpellMessage(mage.ID, spell.ID, mage.Humors));
        }
    }

    public void UpdateMagesCooldowns()
    {
        foreach (Mage m in Mages)
        {
            if(m.ReloadingSpells.Count != 0)
                m.UpdateCooldowns();
        }
    }
}
