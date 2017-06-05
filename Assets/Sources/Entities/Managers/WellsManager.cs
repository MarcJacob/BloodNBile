using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WellsManager
{
    public List<Well> Wells;
    EntityManager EntityModule; // EntityManager associé à ce WellsManager.

    public WellsManager(EntityManager module)
    {
        EntityModule = module;
        Wells = new List<Well>();
    }

    public void Update()
    {
        foreach(Well w in Wells)
        {
            w.UpdateEntity();
        }
    }
}