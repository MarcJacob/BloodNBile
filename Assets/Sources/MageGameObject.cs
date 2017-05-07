using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageGameObject : MonoBehaviour {

    public GameObject Cube;
    private Mage LeMage;
    Entity[] Entities = Entity.GetAllEntities();

	// Use this for initialization
	void Start () {
        foreach (Mage m in Entities)
        {
            LeMage = m;
            Cube.transform.position = m.Pos;
            Cube.transform.rotation = m.Rot;
        }
	}
	
	// Update is called once per frame
	void Update () {
        UpdatePosRot();
    }

    public void UpdatePosRot()
    {
        foreach (Mage m in Entities)
        {
            LeMage = m;
            Cube.transform.position = m.Pos;
            Cube.transform.rotation = m.Rot;
        }
    }
}
