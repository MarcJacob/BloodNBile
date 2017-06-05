using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DrawableEntity : Entity {

    public int MeshID { get; private set; }
    public float Size { get; private set; }

    /// <summary>
    /// Création d'une DrawableEntity.
    /// </summary>
    /// <param name="manager"></param>
    /// <param name="ID"></param>
    /// <param name="pos"></param>
    /// <param name="rot"></param>
    /// <param name="name"></param>
    /// <param name="mesh"> Identifiant du Mesh utilisé par les clients. Mettre à -1 pour ne pas afficher. </param>
    /// <param name="size"></param>
    public DrawableEntity(BnBMatch Match, int ID, Vector3 pos, Quaternion rot, string name, int mesh, float size) : base(Match, ID, pos, rot, name)
    {
        MeshID = mesh;
        Size = size;
    }

    public override void UpdateEntity()
    {

    }
}
