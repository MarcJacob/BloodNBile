using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// L'EntityRenderer est un singleton CLIENT-SIDE qui s'occupe d'afficher les entités en fonction des informations qu'on lui donne.
/// </summary>
public class EntityRenderer : MonoBehaviour {

    List<Unit> Units = new List<Unit>(); // Ensemble des unités que le Renderer "connait". Est mit à jour par le serveur.
    List<Mage> Mages = new List<Mage>(); // Ensemble des mages que le Renderer "connait".
    List<Projectile> Projectiles = new List<Projectile>();
    public Dictionary<int, GameObject> MageGOs = new Dictionary<int, GameObject>(); // Permet de relier l'affichage des mages sous forme de GOs à leur entité.
    public List<UnitRender> RenderedUnits = new List<UnitRender>();

    Camera Cam; // Caméra relié au client actuel.
	void Start () {
        Cam = Camera.main;
        Model.LoadModels(); // Chargement des modèles d'entité pour les entités dessinables (DrawableEntity).
    }

    public void AddUnit(NetworkMessageReceiver message)
    {
        UnitsCreationMessage units = (UnitsCreationMessage)message.ReceivedMessage.Content;
        Debugger.LogMessage("Units added !");
        for(int i = 0; i < units.IDs.Length; i++)
        {
            Unit newUnit = new Unit(units.MatchID, units.IDs[i], units.Positions[i], units.Rotations[i], units.Names[i], units.MeshIDs[i], units.Sizes[i], units.Factions[i], units.Speeds[i], units.Humors[i]);
            if (!Units.Contains(newUnit))
            {
                Units.Add(newUnit);
                RenderedUnits.Add(new UnitRender(newUnit.Pos, newUnit.Rot, newUnit));
            }
        }
    }

    public void RemoveUnit(NetworkMessageReceiver message)
    {
        Unit unit = GetUnitFromID((int)message.ReceivedMessage.Content);
        if (Units.Contains(unit))
        {
            Debugger.LogMessage("Nombre d'unités :" + Units.Count);
            unit.Alive = false;
            Units.Remove(unit);
            int i = 0;
            Debugger.LogMessage(RenderedUnits.Count);
            while(i < RenderedUnits.Count)
            {
                if (RenderedUnits[i].RenderedUnit.Alive == false)
                {
                    Debugger.LogMessage("Unit render supprimé !");
                    RenderedUnits.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
            Debugger.LogMessage("Nombre d'unités :" + Units.Count);
            if (OnUnitRemovedCallback != null)
                OnUnitRemovedCallback(unit);
        }
        else
        {
            Debug.LogError("Unit does not exist !");
        }

    }

    public void Reset()
    {
        Units = new List<Unit>();
        Mages = new List<Mage>();
        foreach(GameObject go in MageGOs.Values)
        {
            Destroy(go);
        }
        MageGOs = new Dictionary<int, GameObject>();
        RenderedUnits = new List<UnitRender>();
    }

    public void OnMageCreated(NetworkMessageReceiver message)
    {

        Mage mage = (Mage)message.ReceivedMessage.Content;
        Mages.Add(mage);
        Units.Add(mage);
        MageGOs.Add(mage.ID, (GameObject.Instantiate(Resources.Load("Prefabs/PlayerPrefab")) as GameObject) as GameObject);
        MageGOs[mage.ID].AddComponent<LinkTo>().Initialize(mage, this);
        Debugger.LogMessage("Mage crée ! ID : " + mage.ID);
    }

    Action<Unit> OnUnitRemovedCallback;
    public void RegisterOnUnitRemovedCallback(Action<Unit> cb)
    {
        OnUnitRemovedCallback += cb;
    }

    public Unit GetUnitFromID(int ID)
    {
        foreach(Unit unit in Units)
        {
            if (unit.ID == ID)
            {
                return unit;
            }
        }

        return null;
    }

    public Mage GetMageFromID(int ID)
    {
        foreach(Mage mage in Mages)
        {
            if (mage.ID == ID)
            {
                return mage;
            }
        }

        return null;
    }

    public Projectile GetProjectileFromID(int ID)
    {
        foreach(Projectile p in Projectiles)
        {
            if (p.ID == ID)
            {
                return p;
            }
        }
        return null;
    }

    Action<Unit, bool> OnUnitPositionUpdatedCallback;
    public void RegisterOnUnitPositionUpdatedCallback(Action<Unit, bool> cb)
    {
        OnUnitPositionUpdatedCallback += cb;
    }

    Action<Unit> OnUnitRotationUpdatedCallback;
    public void RegisterOnUnitRotationUpdatedCallback(Action<Unit> cb)
    {
        OnUnitRotationUpdatedCallback += cb;
    }

    public void EntitiesPositionRotationUpdate(NetworkMessageReceiver messageReceiver)
    {
        EntityPositionRotationUpdate[] updates = (EntityPositionRotationUpdate[])messageReceiver.ReceivedMessage.Content;
        foreach(EntityPositionRotationUpdate update in updates)
        {
            Unit u = GetUnitFromID(update.EntityID);
            if (u != null)
            {
                u.SetPos(update.NewPosition);
                OnUnitPositionUpdatedCallback(u, false);
                u.SetRot(update.NewRotation);
                if (OnUnitRotationUpdatedCallback != null)
                OnUnitRotationUpdatedCallback(u);
            }
        }
    }

    public void OnProjectilesCreated(NetworkMessageReceiver messageReceiver)
    {
        ProjectilesCreationMessage content = (ProjectilesCreationMessage)messageReceiver.ReceivedMessage.Content;
        for(int i=0; i < content.IDs.Length; i++)
        {
            Projectile p = new Projectile(0, content.IDs[i], Mages[0], content.Directions[i], content.Sizes[i], content.Speeds[i], null);
            p.SetPos(content.StartPositions[i]);
            Projectiles.Add(p);
        }
    }

    public void OnProjectileDestroyed(NetworkMessageReceiver messageReceiver)
    {
        int projectileID = (int)messageReceiver.ReceivedMessage.Content;
        Projectile p = GetProjectileFromID(projectileID);

        if (Projectiles.Contains(p))
        {
            Projectiles.Remove(p);
        }
    }

    void Update () {
        // Afficher les entités.
        if (Units != null)
        {
            foreach(UnitRender render in RenderedUnits)
            {
                if (Units.Contains(render.RenderedUnit))
                {
                    render.Process();
                    if (render.RenderedUnit.MeshID >= 0)
                    {
                        RenderMesh(render.RenderedUnit, render);
                    }
                }
            }

        }
        foreach (Projectile P in Projectiles)
        {
            P.UpdateEntity(Time.deltaTime); // Update client-side. Fait bouger le projectile vers sa destination.
            RenderMesh(P);
        }
    }

    private void RenderMesh(DrawableEntity entity, UnitRender renderer = null)
    {
        if (entity == null) return; // S'il n'y a pas d'entité dans CurrentlyRenderer, alors on n'exécute pas le corps de cette méthode.
        int LODLevel = DetermineLOD(entity);
        if (renderer != null)
        {
            if (LODLevel >= Model.GetModelByID(entity.MeshID).ModelMeshs.Length) return; // Si il n'y a pas assez de niveaux de détails pour une telle distance alors on n'affiche pas l'entité.
            Graphics.DrawMesh(Model.GetModelByID(entity.MeshID).ModelMeshs[LODLevel], renderer.CurrentPos, renderer.CurrentRot, Model.GetModelByID(entity.MeshID).ModelMaterial, 0);
        }
        else
        {
            if (LODLevel >= Model.GetModelByID(entity.MeshID).ModelMeshs.Length) return; // Si il n'y a pas assez de niveaux de détails pour une telle distance alors on n'affiche pas l'entité.
            Graphics.DrawMesh(Model.GetModelByID(entity.MeshID).ModelMeshs[LODLevel], entity.Pos, entity.Rot, Model.GetModelByID(entity.MeshID).ModelMaterial, 0);
        }
    }
    public int DistPerLOD = 40000; // Distance séparant chaque changement de LOD. La distance maximale d'affichage est donc nombre de LOD * DistPerLOD.
    private int DetermineLOD(DrawableEntity entity)
    {
        float dist = ((Vector3)entity.Pos - Cam.transform.position).sqrMagnitude;
        int LODLevel = (int)dist / DistPerLOD;
        return LODLevel;
    }
}
