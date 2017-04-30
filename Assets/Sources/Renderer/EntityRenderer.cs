using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// L'EntityRenderer est un singleton (classe statique) CLIENT-SIDE qui s'occupe d'afficher les entités en fonction des informations qu'on lui donne.
/// </summary>
public class EntityRenderer : MonoBehaviour {

    DrawableEntity[] Entities; // Ensemble des entités que le Renderer "connait". Est mit à jour par le serveur.
    Camera Cam; // Caméra relié au client actuel.
	void Start () {
        Cam = Camera.main;
	}

    public void UpdateEntities(DrawableEntity[] entities) // Mise à jour des entités.
    {
        Entities = entities;
    }

	void Update () {
        // Afficher les entités.
        if (Entities != null)
        {
            foreach (DrawableEntity entity in Entities)
            {
                CurrentlyRendered = entity;
                RenderMesh();
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
        float dist = (CurrentlyRendered.Pos - Cam.transform.position).sqrMagnitude;
        int LODLevel = (int)dist / DistPerLOD;
        return LODLevel;
    }
}
