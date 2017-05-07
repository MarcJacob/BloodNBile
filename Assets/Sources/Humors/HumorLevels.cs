using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    /// Add a certain quantity of a humor.
    /// </summary>
    /// <param name="humor">ID number of the humor : 0-Blood, 1-Phlegm, 2-Black Bile, 3-Yellow Bile</param>
    /// <param name="quantity"></param>
    public void GainHumor(int humor, int quantity)
    {
        switch(humor)
        {
            case 0: Blood += quantity; break;
            case 1: Phlegm += quantity; break;
            case 2: BlackBile += quantity; break;
            case 3: YellowBile += quantity; break;

        }
    }

    /// <summary>
    /// Remove a certain quantity of a humor.
    /// </summary>
    /// <param name="humor">ID number of the humor : 0-Blood, 1-Phlegm, 2-Black Bile, 3-Yellow Bile</param>
    /// <param name="quantity"></param>
    public void LoseHumor(int humor, int quantity)
    {
        switch (humor)
        {
            case 0: Blood -= quantity; break;
            case 1: Phlegm -= quantity; break;
            case 2: BlackBile -= quantity; break;
            case 3: YellowBile -= quantity; break;

        }
    }
}
