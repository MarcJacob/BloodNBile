using System;
using System.Collections.Generic;
using UnityEngine;

public class MagesManager
{
    public List<Mage> Mages;
    EntityManager EntityModule; // EntityManager associé à ce MagesManager.
    BnBMatch Match; // Match auquel ce MagesManager appartient.

    public MagesManager(BnBMatch match, EntityManager module)
    {
        Match = match;
        EntityModule = module;
        Mages = new List<Mage>();
    }


    public int CreateMage(Vector3 pos, string name, Faction fac)
    {
        Mage newMage = new Mage(Match, EntityModule.GetAllEntities().Length, pos, Quaternion.identity, name, fac, new HumorLevels(100, 100, 100, 100));
        EntityModule.Entities.Add(newMage);
        EntityModule.Units.Add(newMage);
        Mages.Add(newMage);
        OnMageCreated(newMage);
        return newMage.ID;
    }

    public void OnMageCreated(Mage mage)
    {
        Match.SendMessageToPlayers(13, mage, false, true);
    }

    public void OnClientMovement(NetworkMessageReceiver message)
    {
        if (Match.IsInMatch(message.ConnectionID))
        {
            UnitMovementChangeMessage messageContent = (UnitMovementChangeMessage)message.ReceivedMessage.Content;

            Unit unit = EntityModule.GetUnitFromID(messageContent.UnitID);
            if (unit != null && unit.CanMove)
            {
                Vector3 mov = (Vector3)messageContent.NewMovementVector;
                unit.Move((Vector3)messageContent.NewMovementVector * unit.GetSpeed());
            }
        }
    }

    public void OnClientRotated(NetworkMessageReceiver message)
    {
        UnitRotationChangedMessage messageContent = (UnitRotationChangedMessage)message.ReceivedMessage.Content;
        Debug.Log("Reçu une nouvelle rotation du joueur");
        Unit unit = EntityModule.GetUnitFromID(messageContent.UnitID);
        if (unit != null)
        {
            Quaternion newRot = (Quaternion)messageContent.NewQuaternion;
            unit.SetRot(newRot);
        }
    }
}
