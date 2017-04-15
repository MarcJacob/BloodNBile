using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour {

    Mage mage;

    // Use this for initialization
    void Start () {
        HumorLevels loul = new HumorLevels(50, 50, 50, 50);
        ConvertSpell BloodToPhlegm = new ConvertSpell(0, 20, 5, 1);
        ConvertSpell BloodToBlack = new ConvertSpell(0, 20, 5, 2);
        ConvertSpell BloodToYelllow = new ConvertSpell(0, 20, 5, 3);
        ConvertSpell PhlegmToBlood = new ConvertSpell(1, 20, 5, 0);
        ConvertSpell PhlegmToBlack = new ConvertSpell(1, 20, 5, 2);
        ConvertSpell PhlegmToYellow = new ConvertSpell(1, 20, 5, 3);
        ConvertSpell BlackToBlood = new ConvertSpell(2, 20, 5, 0);
        ConvertSpell BlackToPhlegm = new ConvertSpell(2, 20, 5, 1);
        ConvertSpell BlackToYellow = new ConvertSpell(2, 20, 5, 3);
        ConvertSpell YellowToBlood = new ConvertSpell(3, 20, 5, 0);
        ConvertSpell YellowToPhlegm = new ConvertSpell(3, 20, 5, 1);
        ConvertSpell YellowToBlack = new ConvertSpell(3, 20, 5, 2);

        mage = new Mage(Vector3.zero, Quaternion.identity, "Loul", loul);

        mage.Cast(BloodToPhlegm);
        mage.Cast(PhlegmToBlack);
        mage.Cast(YellowToBlack);
        mage.Cast(BloodToBlack);
    }
	
	// Update is called once per frame
	void Update () {
        UpdateEntities();
        mage.UpdateCooldowns();
	}

    private void UpdateEntities()
    {
        foreach(Entity e in Entity.GetAllEntities())
        {
            e.UpdateEntity();
        }
    }
}
