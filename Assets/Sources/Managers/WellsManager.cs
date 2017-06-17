using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WellsManager
{
    public List<Well> Wells;
    EntityManager EntityModule; // EntityManager associé à ce WellsManager.

    public WellsManager(EntityManager module, Map map)
    {
        EntityModule = module;
        Wells = new List<Well>();

        foreach(WellPlacement well in map.GetWellPlacements())
        {
            Wells.Add(new Well(module.Match, EntityModule.Entities.Count, well.Position, well.Humors));
        }
    }

    public void Update()
    {
        foreach(Well w in Wells)
        {
            w.UpdateEntity(Time.deltaTime);
        }
    }
}