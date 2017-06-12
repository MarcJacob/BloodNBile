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
    public Effect SpellEffect { get; protected set; }
    private static List<Spell> SpellsList = new List<Spell>();
    public String Name { get; protected set; }

    public Spell(int humor, int cost, float cooldown, string name)
    {
        Humor = humor;
        Cost = cost;
        Cooldown = cooldown;
        ID = LastID;
        LastID++;
        Name = name;
        SpellsList.Add(this);
    }

    public bool IsCastable(Mage mage)
    {
        bool isCastable = true;
        if (mage.ReloadingSpells.ContainsKey(this) && mage.ReloadingSpells[this] >= 0)	isCastable = false;
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

    public virtual void Cast(BnBMatch match, Mage caster)
    {
        Debugger.LogMessage(Name + " se lance !");
    }

    public static Spell GetSpellFromID(int id)
    {
        foreach (Spell s in SpellsList)
            if (s.ID == id)
            {
                return s;
            }
        return null;
    }

    public static void LoadSpells()
    {
        SummonSpell SummonBlood = new SummonSpell(0, 50, 15, "Summon blood minions");
        ConvertSpell BloodToPhlegm = new ConvertSpell(0, 20, 5, "Blood to Phlegm", 1 );
        ConvertSpell BloodToBlack = new ConvertSpell(0, 20, 5, "Blood to Blackile", 2);
        ConvertSpell BloodToYellow = new ConvertSpell(0, 20, 5, "Blood to Yellile", 3);
        ConvertSpell PhlegmToBlood = new ConvertSpell(1, 20, 5, "Phlegm to Blood", 0);
        ConvertSpell PhlegmToBlack = new ConvertSpell(1, 20, 5, "Phlegm to Blackile", 2);
        ConvertSpell PhlegmToYellow = new ConvertSpell(1, 20, 5, "Phlegm to Yellile", 3);
        ConvertSpell BlackToBlood = new ConvertSpell(2, 20, 5, "Blackile to Blood", 0);
        ConvertSpell BlackToPhlegm = new ConvertSpell(2, 20, 5, "Blackile to Phlegm", 1);
        ConvertSpell BlackToYellow = new ConvertSpell(2, 20, 5, "Blackile to Yellile", 3);
        ConvertSpell YellowToBlood = new ConvertSpell(3, 20, 5, "Yellile to Blood", 0);
        ConvertSpell YellowToPhlegm = new ConvertSpell(3, 20, 5, "Yellile to Phlegm", 1);
        ConvertSpell YellowToBlack = new ConvertSpell(3, 20, 5, "Yellile to Blackile", 2);
    }
}
