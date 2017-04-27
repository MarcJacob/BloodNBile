using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawableEntity : Entity {

    public int MeshID { get; private set; }
    public float Size { get; private set; }

    public DrawableEntity(int ID, Vector3 pos, Quaternion rot, string name, int mesh, float size) : base(ID, pos, rot, name)
    {
        MeshID = mesh;
        Size = size;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void UpdateEntity()
    {
    }
}
