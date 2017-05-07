using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Entity  {

    public int ID { get; private set; }
    public SerializableVector3 Pos { get; private set; }
    public SerializableQuaternion Rot { get; private set; }
    public string Name { get; private set; }


    public Entity(int ID, Vector3 pos, Quaternion rot, string name)
    {
        this.ID = ID;
        Pos = pos;
        Rot = rot;
        Name = name;
    }

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
    }

    public bool Alive = true;
    public void Die()
    {
        Alive = false;
    }

    public virtual void UpdateEntity ()
    {

    }

    public void SetPos(Vector3 vect)
    {
        this.Pos = vect;
    }
}
