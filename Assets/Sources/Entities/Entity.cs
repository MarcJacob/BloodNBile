using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Entity  {

    private static List<Entity> EntityList = new List<Entity>();
    private static int lastID = 0;

    public int ID { get; private set; }
    public SerializableVector3 Pos { get; private set; }
    public SerializableQuaternion Rot { get; private set; }
    public string Name { get; private set; }


    public Entity(Vector3 pos, Quaternion rot, string name)
    {
        Pos = pos;
        Rot = rot;
        Name = name;
        ID = lastID;
        lastID++;
        EntityList.Add(this);
    }

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
    }

    public void Die()
    {
        EntityList.Remove(this);
    }

    public static Entity[] GetAllEntities()
    {
        return EntityList.ToArray();
    }

    public virtual void UpdateEntity ()
    {
    }

}
