using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// L'EntityRenderer est un singleton (classe statique) CLIENT-SIDE qui s'occupe d'afficher les entités en fonction des informations qu'on lui donne.
/// </summary>
public class EntityRenderer : MonoBehaviour {

    DrawableEntity[] Entities; // Ensemble des entités que le Renderer "connait". Est mit à jour par le serveur.
	void Start () {
        Entities = new DrawableEntity[]
        {
            new DrawableEntity(new Vector3(0f, 0f, 0f), Quaternion.identity, "Test Entity", 0, 2f),
            new DrawableEntity(new Vector3(10f, 0f, 0f), Quaternion.identity, "Test Entity 2", 0, 2f)
        };
	}

    public void UpdateEntities(DrawableEntity[] entities) // Mise à jour des entités.
    {
        Entities = entities;
    }

	void Update () {
        // Afficher les entités.
        foreach (DrawableEntity entity in Entities)
        {
            CurrentlyRendered = entity;
            RenderMesh();
        }
	}

    DrawableEntity CurrentlyRendered; // Entité dont l'affichage est actuellement calculée.

    private void RenderMesh()
    {
        if (CurrentlyRendered == null) return; // S'il n'y a pas d'entité dans CurrentlyRenderer, alors on n'exécute pas le corps de cette méthode.

        Graphics.DrawMesh(Model.GetModels()[CurrentlyRendered.MeshID].ModelMesh, CurrentlyRendered.Pos, CurrentlyRendered.Rot, Model.GetModels()[CurrentlyRendered.MeshID].ModelMaterial, 0);

    }

    private void OnGUI()
    {
        Camera cam = Camera.main;

        foreach (DrawableEntity entity in Entities)
        {
            Vector2 screenPos = cam.WorldToScreenPoint(entity.Pos + new Vector3(0f, entity.Size, 0f));
            // Correction de la position sur l'axe vertical
            screenPos = new Vector2(screenPos.x, Mathf.Abs(screenPos.y - Screen.height));
            if (screenPos.y < Screen.height / 2 + 50 && screenPos.x < Screen.width / 2 + 50 && screenPos.y > Screen.height / 2 - 50 && screenPos.x > Screen.width / 2 - 50)
                GUI.TextArea(new Rect(screenPos + new Vector2(-entity.Name.Length*5, entity.Size), new Vector2(100f, 50f)), entity.Name);
        }
    }
}
