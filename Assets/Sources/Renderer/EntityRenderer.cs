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
    public Dictionary<int, GameObject> MageGOs = new Dictionary<int, GameObject>(); // Permet de relier l'affichage des mages sous forme de GOs à leur entité.

    Camera Cam; // Caméra relié au client actuel.
	void Start () {
        Cam = Camera.main;
        Model.LoadModels(); // Chargement des modèles d'entité pour les entités dessinables (DrawableEntity).
    }

    public void AddUnit(NetworkMessageReceiver message)
    {
        Unit unit = (Unit)message.ReceivedMessage.Content;
        if (!Units.Contains(unit))
        {
            Units.Add(unit);
        }
    }

    public void RemoveUnit(NetworkMessageReceiver message)
    {

        Unit unit = (Unit)message.ReceivedMessage.Content;
        if (Units.Contains(unit))
        {
            Debugger.LogMessage("Suppression d'une unité..");
            Units.Remove(unit);
            if (OnUnitRemovedCallback != null)
            OnUnitRemovedCallback(unit);
        }
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

    void Update () {
        // Afficher les entités.
        if (Units != null)
        {
            foreach (Unit entity in Units)
            {
                if (entity.MeshID >= 0)
                {
                    CurrentlyRendered = entity;
                    RenderMesh();
                }

            }
        }
	}

    DrawableEntity CurrentlyRendered; // Entité dont l'affichage est actuellement calculée.

    private void RenderMesh()
    {
        if (CurrentlyRendered == null) return; // S'il n'y a pas d'entité dans CurrentlyRenderer, alors on n'exécute pas le corps de cette méthode.
        int LODLevel = DetermineLOD();
        if (LODLevel >= Model.GetModels()[CurrentlyRendered.MeshID].ModelMeshs.Length) return; // Si il n'y a pas assez de niveaux de détails pour une telle distance alors on n'affiche pas l'entité.
        Graphics.DrawMesh(Model.GetModels()[CurrentlyRendered.MeshID].ModelMeshs[DetermineLOD()], CurrentlyRendered.Pos, CurrentlyRendered.Rot, Model.GetModels()[CurrentlyRendered.MeshID].ModelMaterial, 0);
    }
    public int DistPerLOD = 50*50; // Distance séparant chaque changement de LOD. La distance maximale d'affichage est donc nombre de LOD * DistPerLOD.
    private int DetermineLOD()
    {
        float dist = ((Vector3)CurrentlyRendered.Pos - Cam.transform.position).sqrMagnitude;
        int LODLevel = (int)dist / DistPerLOD;
        return LODLevel;
    }
}
