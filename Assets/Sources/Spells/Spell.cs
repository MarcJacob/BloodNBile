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
    public EffectBlueprint[] SpellEffectBlueprints { get; protected set; }
    private static List<Spell> SpellsList = new List<Spell>();
    public String Name { get; protected set; }

    public Spell(int humor, int cost, float cooldown, string name, EffectBlueprint[] effects)
    {
        Humor = humor;
        Cost = cost;
        Cooldown = cooldown;
        ID = LastID;
        LastID++;
        Name = name;
        SpellsList.Add(this);
        SpellEffectBlueprints = effects;
    }

    public Spell(int humor, int cost, float cooldown, string name, EffectBlueprint effect)
    {
        Humor = humor;
        Cost = cost;
        Cooldown = cooldown;
        ID = LastID;
        LastID++;
        Name = name;
        SpellsList.Add(this);
        SpellEffectBlueprints = new EffectBlueprint[] { effect };
    }

    public bool IsCastable(Mage mage)
    {
        bool isCastable = true;
        if (mage.ReloadingSpells.ContainsKey(this) && mage.ReloadingSpells[this] >= 0)isCastable = false;
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
        foreach(EffectBlueprint ebp in SpellEffectBlueprints)
        {
            ebp.Instantiate(caster, match);
        }
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
        Spell SummonBlood = new Spell(0, 50, 15, "Summon blood minions (unfriendly)", new EffectBPSummon(MobType.BLOOD_HUMORLING, false));
        Spell SummonBloodUF = new Spell(0, 50, 15, "Summon blood minions", new EffectBPSummon(MobType.BLOOD_HUMORLING, true));
        Spell BloodToPhlegm = new Spell(0, 20, 5, "Blood to Phlegm", new EffectBPChangeHumor(0, 15, 0, 0));
        Spell BloodToBlack = new Spell(0, 20, 5, "Blood to Blackile", new EffectBPChangeHumor(0, 0, 0, 15));
        Spell BloodToYellow = new Spell(0, 20, 5, "Blood to Yellile", new EffectBPChangeHumor(0, 0, 15, 0));
        Spell PhlegmToBlood = new Spell(1, 20, 5, "Phlegm to Blood", new EffectBPChangeHumor(15, 0, 0, 0));
        Spell PhlegmToBlack = new Spell(1, 20, 5, "Phlegm to Blackile", new EffectBPChangeHumor(0, 0, 0, 15));
        Spell PhlegmToYellow = new Spell(1, 20, 5, "Phlegm to Yellile", new EffectBPChangeHumor(0, 0, 15, 0));
        Spell BlackToBlood = new Spell(2, 20, 5, "Blackile to Blood", new EffectBPChangeHumor(15, 0, 0, 0));
        Spell BlackToPhlegm = new Spell(2, 20, 5, "Blackile to Phlegm", new EffectBPChangeHumor(0, 15, 0, 0));
        Spell BlackToYellow = new Spell(2, 20, 5, "Blackile to Yellile", new EffectBPChangeHumor(0, 0, 15, 0));
        Spell YellowToBlood = new Spell(3, 20, 5, "Yellile to Blood", new EffectBPChangeHumor(15, 0, 0, 0));
        Spell YellowToPhlegm = new Spell(3, 20, 5, "Yellile to Phlegm", new EffectBPChangeHumor(0, 15, 0, 0));
        Spell YellowToBlack = new Spell(3, 20, 5, "Yellile to Blackile", new EffectBPChangeHumor(0, 0, 0, 15));
    }
}
