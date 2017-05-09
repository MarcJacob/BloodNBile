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

        return newMage.ID;
    }
}
