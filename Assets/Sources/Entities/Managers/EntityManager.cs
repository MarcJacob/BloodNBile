using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class EntityManager {

    public List<Entity> Entities = new List<Entity>(); // Ensemble des entités
    public List<Unit> Units = new List<Unit>(); // Ensemble des unités. NOTE : Entities contient Units.
    BnBMatch Match; // Le match auquel cet EntityManager est lié.
    public Entity[] GetAllEntities()
    {
        return Entities.ToArray();
    }

    public EntityManager(BnBMatch match)
    {
        Match = match;
        
        Unit.RegisterOnDestinationSetCallback(OnUnitStartedMoving);
        Unit.RegisterOnArrivedToDestinationCallback(OnUnitReachedDestination);
        Unit.RegisterOnUnitDiedCallback(OnUnitDeath);
    }

    public void UpdateEntities()
    {
        List<Entity> DeadEntities = new List<Entity>();
        foreach(Entity e in Entities)
        {
            if (e.Alive)
                e.UpdateEntity();
            else
                DeadEntities.Add(e);
        }

        foreach(Entity e in DeadEntities)
        {
            Entities.Remove(e);
        }
    }

    public Entity CreateEntity(Vector3 pos, Quaternion rot, string name)
    {
        Debug.Log("Création d'une entité : " + name);
        Entity ent = new Entity(Match, Entities.Count, pos, rot, name);
        Entities.Add(ent);
        return ent;
    }

    public Unit CreateUnit(Vector3 pos, Quaternion rot, string name, int mesh, float size, Faction fac, float speed) // Surcharge pour les entités de type Unit.
    {
        Debug.Log("Création d'une unité : " + name);
        Unit newUnit = new Unit(Match, Entities.Count, pos, rot, name, mesh, size, fac, speed);
        Units.Add(newUnit);
        Entities.Add(newUnit);


        OnUnitCreated(newUnit);
        return newUnit;
    }

    void OnUnitCreated(Unit unit)
    {
        if (Units.Contains(unit))
        Match.SendMessageToPlayers(10, unit, false, true);
    }

    void OnUnitDeath(Unit unit)
    {
        if (Units.Contains(unit))
        {
            Debug.Log(unit.Name + " est morte.");
            Match.SendMessageToPlayers(11, unit, false, true);
            Units.Remove(unit);
        }
    }

    void OnUnitStartedMoving(Unit unit)
    {
        if (Units.Contains(unit))
            Match.SendMessageToPlayers(12, unit, false, true);
    }

    void OnUnitReachedDestination(Unit unit)
    {
        if (Units.Contains(unit))
            Match.SendMessageToPlayers(13, unit, false, true);
    }
}
