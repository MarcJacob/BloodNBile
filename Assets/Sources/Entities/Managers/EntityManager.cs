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
        Unit.RegisterOnUnitDiedCallback(OnUnitDeath);
        Unit.RegisterOnUnitMovementVectorChanged(OnUnitMovementVectorChanged);
        Unit.RegisterOnUnitRotationChanged(OnUnitRotationChanged);
    }

    public Unit GetUnitFromID(int ID)
    {
        foreach(Unit u in Units)
        {
            if (u.ID == ID)
            {
                return u;
            }
        }
        return null;
    }


    float EntityPositionsUpdateCooldown = 2f;
    float currentEntityPositionUpdateCooldown = 0f;
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

        if (currentEntityPositionUpdateCooldown >= EntityPositionsUpdateCooldown)
        {
            Match.SendMessageToPlayers(15, Entities.ToArray(), true, true);
            currentEntityPositionUpdateCooldown = 0f;
        }
        else
        {
            currentEntityPositionUpdateCooldown += Time.deltaTime;
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

    void OnUnitMovementVectorChanged(Unit unit)
    {
        Debug.Log((SerializableVector3)((Vector3)unit.MovementVector + (Vector3)unit.WilledMovementVector));
        if (Units.Contains(unit))
            Match.SendMessageToPlayers(12, new UnitMovementChangeMessage(unit.ID, (SerializableVector3)((Vector3)unit.MovementVector + (Quaternion)unit.Rot * (Vector3)unit.WilledMovementVector)), true);
    }

    void OnUnitRotationChanged(Unit unit)
    {
        Debug.Log("Rotation de l'unité modifiée !");
        if (Units.Contains(unit))
        {
            Match.SendMessageToPlayers(16, new UnitRotationChangedMessage(unit.ID, unit.Rot), true);
        }
    }
}
