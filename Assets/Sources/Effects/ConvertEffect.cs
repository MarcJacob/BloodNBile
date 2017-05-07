using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvertEffect : Effect {

    public void Activate(Mage mage, int humor,int quantity)
    {
        mage.GainHumor(humor, quantity);
    }
}
