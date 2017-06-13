using System;
using System.Collections.Generic;
using UnityEngine;

public class HumorlingsManager
{
    public List<Humorling> Humorlings;
    public EntityManager EntityModule { get; private set; } // EntityManager associé à ce MagesManager.

    public HumorlingsManager(EntityManager module)
    {
        EntityModule = module;
        Humorlings = new List<Humorling>();
    }


    public int CreateHumorling(MobType type, Vector3 pos, Faction fac)
    {
        Humorling newHumorling = new Humorling(EntityModule.Match, EntityModule.GetAllEntities().Length, pos, Quaternion.identity, "Humorling",  0, 1, type, fac);
        EntityModule.OnUnitCreated(newHumorling);
        Humorlings.Add(newHumorling);
        return newHumorling.ID;
    }

    public void RunAIs()
    {
        foreach(Humorling H in Humorlings)
        {
            H.AI(EntityModule.Match.CellsModule);
        }
    }
}