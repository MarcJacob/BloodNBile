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

    Dictionary<Entity, PositionUpdateVector3> PositionUpdates = new Dictionary<Entity, PositionUpdateVector3>(); // Mises à jour de positions d'entités en cours.
    Dictionary<Entity, Quaternion> RotationUpdates = new Dictionary<Entity, Quaternion>(); // Mises à jour de rotations d'entités en cours

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
            Debug.Log("Suppression d'une unité..");
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
        Debug.Log("Mage crée ! ID : " + mage.ID);
    }

    Action<Unit> OnUnitRemovedCallback;
    public void RegisterOnUnitRemovedCallback(Action<Unit> cb)
    {
        OnUnitRemovedCallback += cb;
    }

    public void ProcessMovement(Unit unit)
    {
        if (unit.MovementVector != Vector3.zero || unit.WilledMovementVector != Vector3.zero)
        {
            SetUnitPos(unit, unit.Pos + (SerializableVector3)((Vector3)unit.MovementVector * Time.deltaTime), false, false);
        }
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

    void SetUnitPos(Unit unit, Vector3 pos, bool force = false, bool smooth = false)
    {
        if (smooth)
        {
            if (!PositionUpdates.ContainsKey(unit))
                PositionUpdates.Add(unit, pos);
            else
                PositionUpdates[unit] = pos;
        }
        else
        {
            unit.SetPos(pos);
            if (OnUnitPositionUpdatedCallback != null)
                OnUnitPositionUpdatedCallback(unit, force);
        }
    }

    void SetUnitRot(Unit unit, Quaternion rot, bool force = false)
    {
        if (!RotationUpdates.ContainsKey(unit))
            RotationUpdates.Add(unit, rot);
        else
            RotationUpdates[unit] = rot;
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

    public void OnUnitMovementVectorUpdate(NetworkMessageReceiver message)
    {
        UnitMovementChangeMessage messageContent = (UnitMovementChangeMessage)message.ReceivedMessage.Content;
        GetUnitFromID(messageContent.UnitID).SetMovement(messageContent.NewMovementVector);
    }

    public void OnEntitiesPositionUpdate(NetworkMessageReceiver message)
    {
        Debug.Log("Mise à jour de la position de toutes les entités");
        Entity[] ent = (Entity[])message.ReceivedMessage.Content;
        foreach(Entity e in ent)
        {
            SetUnitPos(GetUnitFromID(e.ID), e.Pos, true, true);
            SetUnitRot(GetUnitFromID(e.ID), e.Rot, true);
        }
    }

    public void OnEntityRotationUpdate(NetworkMessageReceiver messageReceiver)
    {
        UnitRotationChangedMessage messageContent = (UnitRotationChangedMessage)messageReceiver.ReceivedMessage.Content;

        Unit unit = GetUnitFromID(messageContent.UnitID);
        if (unit != null)
        {
            SetUnitRot(unit, messageContent.NewQuaternion, false);
        }
    }

    public void ProcessPositionUpdate(Entity e)
    {
        if (PositionUpdates.ContainsKey(e))
        {
            e.SetPos(Vector3.Lerp(e.Pos, PositionUpdates[e], Time.deltaTime*5));
            if (OnUnitPositionUpdatedCallback != null)
                OnUnitPositionUpdatedCallback(GetUnitFromID(e.ID), PositionUpdates[e].Forced);
            if (((Vector3)e.Pos - PositionUpdates[e]).sqrMagnitude <= 0.2f)
            {
                PositionUpdates.Remove(e);
            }
        }
    }

    public void ProcessRotationUpdate(Entity e)
    {
        if (RotationUpdates.ContainsKey(e))
        {
            e.SetRot(Quaternion.Lerp(e.Rot, RotationUpdates[e], Time.deltaTime*5));
            if (OnUnitRotationUpdatedCallback != null)
                OnUnitRotationUpdatedCallback(GetUnitFromID(e.ID));
            if (Quaternion.Angle(e.Rot, RotationUpdates[e]) <= 0.1f)
            {
                RotationUpdates.Remove(e);
            }
        }
    }

    void Update () {
        // Afficher les entités.
        if (Units != null)
        {
            foreach (Unit entity in Units)
            {
                ProcessMovement(entity);
                ProcessPositionUpdate(entity);
                ProcessRotationUpdate(entity);
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
