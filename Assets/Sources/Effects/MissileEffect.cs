using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileEffect : Effect {

    public GameObject missile;

    public void Activate(Mage mage, int humor, int quantity)
    {
        Object.Instantiate(missile, mage.Pos, mage.Rot);
    }
}
