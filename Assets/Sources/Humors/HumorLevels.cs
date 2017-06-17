using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class HumorLevels {
    
    public int Blood { get; private set; }
    public int Phlegm { get; private set; }
    public int BlackBile { get; private set; }
    public int YellowBile { get; private set; }

    public HumorLevels(int blood, int phlegm, int black, int yellow)
    {
        Blood = blood;
        Phlegm = phlegm;
        BlackBile = black;
        YellowBile = yellow;
    }


    /// <summary>
    /// Change la quantité d'une humeur choisie.
    /// </summary>
    /// <param name="humor">ID number of the humor : 0-Blood, 1-Phlegm, 2-Black Bile, 3-Yellow Bile</param>
    /// <param name="quantity"></param>
    public void ChangeHumor(int humor, int quantity)
    {
        switch(humor)
        {
            case 0: Blood += quantity; break;
            case 1: Phlegm += quantity; break;
            case 2: BlackBile += quantity; break;
            case 3: YellowBile += quantity; break;

        }

        if (Blood < 0) Blood = 0;
        if (Phlegm < 0) Phlegm = 0;
        if (YellowBile < 0) YellowBile = 0;
        if (BlackBile < 0) BlackBile = 0;
    }

    static public HumorLevels operator +(HumorLevels hl1, HumorLevels hl2)
    {
        return new HumorLevels(hl1.Blood + hl2.Blood, hl1.Phlegm + hl2.Phlegm, hl1.BlackBile + hl2.BlackBile, hl1.YellowBile + hl2.YellowBile);
    }

    public override string ToString()
    {
        return "Blood : " + Blood + " Phlegm : " + Phlegm + " Black : " + BlackBile + " Yellow : " + YellowBile;
    }

    static public HumorLevels operator -(HumorLevels humors)
    {
        return new HumorLevels(-humors.Blood, -humors.Phlegm, -humors.BlackBile, -humors.YellowBile);
    }
}

public enum Humor
{
    BLOOD = 0,
    PHLEGM = 1,
    BLACKBILE = 2,
    YELLOWBILE = 3
}