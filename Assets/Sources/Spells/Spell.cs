using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Spell {

    public int ID { get; protected set; }
    private static int LastID = 0;
    public int Humor { get; protected set; }
    public int Cost { get; protected set; }
    public float Cooldown { get; protected set; }
    public bool IsReloading { get; protected set; }
    public Effect SpellEffect { get; protected set; }
    public static List<Spell> SpellsList = new List<Spell>();

    public Spell(int humor, int cost, int cooldown)
    {
        Humor = humor;
        Cost = cost;
        Cooldown = cooldown;
        IsReloading = false;
        ID = LastID;
        LastID++;
        SpellsList.Add(this);
    }

    public bool IsCastable(Mage mage)
    {
        bool isCastable = true;
        if (IsReloading) isCastable = false;
        else
        {
            switch (Humor)
            {
                case 0: if (mage.Humors.Blood < Cost) isCastable = false; break;
                case 1: if (mage.Humors.Phlegm < Cost) isCastable = false; break;
                case 2: if (mage.Humors.BlackBile < Cost) isCastable = false; break;
                case 3: if (mage.Humors.YellowBile < Cost) isCastable = false; break;
            }
        }
        return isCastable;
    }

    public virtual void Cast(Mage caster)
    {
        IsReloading = true;
        Debug.Log("Je me lance !");
    }

    public void HasReloaded()
    {
        IsReloading = false;
        Debug.Log("Wow je suis prêt mtn");
    }
}
