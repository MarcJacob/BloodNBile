using System;
using System.Collections.Generic;
using UnityEngine;

public class HumorlingsManager
{
    public List<Humorling> Humorlings;
    EntityManager EntityModule; // EntityManager associé à ce MagesManager.

    public HumorlingsManager(EntityManager module)
    {
        EntityModule = module;
        Humorlings = new List<Humorling>();
    }


    public int CreateHumorling(Vector3 pos, Faction fac)
    {
        Humorling newHumorling = new Humorling(EntityModule.Match, EntityModule.GetAllEntities().Length, pos, Quaternion.identity, "Humorling",  0, 1, 0, fac);
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