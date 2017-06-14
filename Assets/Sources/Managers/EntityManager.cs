using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class EntityManager {

    public List<Entity> Entities = new List<Entity>(); // Ensemble des entités
    public List<Unit> Units = new List<Unit>(); // Ensemble des unités. NOTE : Entities contient Units.
    public List<Unit> SpawnedUnits = new List<Unit>();
    WeakReference MatchWeakRef; // Le match auquel cet EntityManager est lié.
    public BnBMatch Match { get { return ((BnBMatch)(MatchWeakRef.Target)); } }
    public Entity[] GetAllEntities()
    {
        return Entities.ToArray();
    }

    public EntityManager(BnBMatch match)
    {
        MatchWeakRef = new WeakReference(match);
        Unit.RegisterOnUnitDiedCallback(OnUnitDeath);
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


    float EntityPositionsUpdateCooldown = 0.1f;
    float currentEntityPositionUpdateCooldown = 0f;
    public void UpdateEntities()
    {
        if (SpawnedUnits.Count > 0)
        {
            int count = SpawnedUnits.Count;
            int[] IDs = new int[count];
            int[] MeshIDs = new int[count];
            SerializableVector3[] positions = new SerializableVector3[count];
            SerializableQuaternion[] rotations = new SerializableQuaternion[count];
            HumorLevels[] humors = new HumorLevels[count];
            string[] names = new string[count];
            float[] sizes = new float[count];
            Faction[] factions = new Faction[count];
            float[] speeds = new float[count];
            Unit[] spawns = SpawnedUnits.ToArray();
            for (int i = 0; i < count; i++)
            {
                Unit u = spawns[i];
                IDs[i] = u.ID;
                MeshIDs[i] = u.MeshID;
                positions[i] = u.Pos;
                rotations[i] = u.Rot;
                humors[i] = u.Humors;
                names[i] = u.Name;
                sizes[i] = u.Size;
                factions[i] = u.Fac;
                speeds[i] = u.GetSpeed();
            }
            Match.SendMessageToPlayers(10, new UnitsCreationMessage(Match.ID, IDs, positions, rotations, MeshIDs, humors, names, sizes, factions, speeds), false, true);
            SpawnedUnits = new List<Unit>();
        }
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
            List<EntityPositionRotationUpdate> updateList = new List<EntityPositionRotationUpdate>();
            foreach(Entity e in Entities)
            {
                updateList.Add(new EntityPositionRotationUpdate(e.ID, e.Pos, e.Rot));
            }
            Match.SendMessageToPlayers(12, updateList.ToArray(), true, true);
            currentEntityPositionUpdateCooldown = 0f;
        }
        else
        {
            currentEntityPositionUpdateCooldown += Time.deltaTime;
        }
    }

    public Entity CreateEntity(Vector3 pos, Quaternion rot, string name)
    {
        Debugger.LogMessage("Création d'une entité : " + name);
        Entity ent = new Entity(Match.ID, Entities.Count, pos, rot, name);
        Entities.Add(ent);
        return ent;
    }

    public Unit CreateUnit(Vector3 pos, Quaternion rot, string name, int mesh, float size, Faction fac, float speed, HumorLevels humors) // Surcharge pour les entités de type Unit.
    {
        Debugger.LogMessage("Création d'une unité : " + name);
        Unit newUnit = new Unit(Match.ID, Entities.Count, pos, rot, name, mesh, size, fac, speed, humors);

        OnUnitCreated(newUnit);
        return newUnit;
    }

    public void OnUnitCreated(Unit unit, bool sendNetworkMessage = true)
    {
        Units.Add(unit);
        Entities.Add(unit);
        SpawnedUnits.Add(unit);

        if (OnUnitCreatedCallback != null)
        {
            OnUnitCreatedCallback(unit);
        }
    }

    Action<Unit> OnUnitCreatedCallback;
    public void RegisterOnUnitCreatedCallback(Action<Unit> cb)
    {
        OnUnitCreatedCallback += cb;
    }

    void OnUnitDeath(Unit unit)
    {
        if (unit.MatchID == Match.ID)
            if (Units.Contains(unit))
            {
                Match.HumorBank.ChangeHumor(0, (int)((float)unit.Humors.Blood * BnBMatch.MapHumorsGainProportion));
                Match.HumorBank.ChangeHumor(1, (int)((float)unit.Humors.Phlegm * BnBMatch.MapHumorsGainProportion));
                Match.HumorBank.ChangeHumor(2, (int)((float)unit.Humors.YellowBile * BnBMatch.MapHumorsGainProportion));
                Match.HumorBank.ChangeHumor(3, (int)((float)unit.Humors.BlackBile * BnBMatch.MapHumorsGainProportion));
                if (OnUnitDeathCallback != null)
                {
                    OnUnitDeathCallback(unit);
                }
                Debugger.LogMessage(unit.Name + " est morte.");
                Match.SendMessageToPlayers(11, unit.ID, false, false);
                Units.Remove(unit);
            }
    }

    Action<Unit> OnUnitDeathCallback;
    public void RegisterOnUnitDeathCallback(Action<Unit> cb)
    {
        OnUnitDeathCallback += cb;
    }
}
