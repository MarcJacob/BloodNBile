using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour {

    private Entity[] EntityList;
    private Entity ent;
    private DrawableEntity dEnt;
    private float compteur;

	// Use this for initialization
	void Start () {
        ent = new Entity(Vector3.zero, Quaternion.identity, "Lul");
        dEnt = new DrawableEntity(Vector3.zero, Quaternion.identity, "Lil", 0, 2f);
        EntityList = Entity.GetAllEntities();
    }
	
	// Update is called once per frame
	void Update () {
        compteur += Time.deltaTime;
        if (compteur >= 5)
            ent.Die();

        EntityList = Entity.GetAllEntities();
        UpdateEntities();
	}

    private void UpdateEntities()
    {
        foreach(Entity e in EntityList)
        {
            e.UpdateEntity();
        }
    }
}
