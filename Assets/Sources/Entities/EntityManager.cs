using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Model.LoadModels();
    }
	
	// Update is called once per frame
	void Update () {
        UpdateEntities();
	}

    private void UpdateEntities()
    {
        foreach(Entity e in Entity.GetAllEntities())
        {
            e.UpdateEntity();
        }
    }
}
