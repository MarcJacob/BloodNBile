﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawableEntity : Entity {

    public int MeshID { get; private set; }
    public float Size { get; private set; }

    public DrawableEntity(Vector3 pos, Quaternion rot, string name, int mesh, float size) : base(pos, rot, name)
    {
        MeshID = mesh;
        Size = size;
    }

    void Start () {
		
	}
	
	void Update () {
		
	}

    public override void UpdateEntity()
    {
    }
}
