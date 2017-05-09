using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Mage : Unit, IHumorEntity {

    public HumorLevels Humors { get; private set; }
    public float LOP { get; private set; }
    private bool IsCasting;
    public Dictionary<Spell, float> ReloadingSpells;

    public Mage(BnBMatch Match, int ID, Vector3 pos, Quaternion rot, string name, Faction fac, HumorLevels humors) : base(Match, ID, pos, rot, name, -1, 2, fac)
    {
        Humors = humors;
        IsCasting = false;
        ReloadingSpells = new Dictionary<Spell, float>();
        LOP = (humors.Blood + humors.Phlegm + humors.BlackBile + humors.YellowBile) / 4;
    }

    /// <summary>
    /// Add a certain quantity of a humor.
    /// </summary>
    /// <param name="humor">ID number of the humor : 0-Blood, 1-Phlegm, 2-Black Bile, 3-Yellow Bile</param>
    /// <param name="quantity"></param>
    public void GainHumor(int humor, int quantity)
    {
        Humors.GainHumor(humor, quantity);
        UpdateLOP();
    }

    /// <summary>
    /// Remove a certain quantity of a humor.
    /// </summary>
    /// <param name="humor">ID number of the humor : 0-Blood, 1-Phlegm, 2-Black Bile, 3-Yellow Bile</param>
    /// <param name="quantity"></param>
    public void LoseHumor(int humor, int quantity)
    {
        Humors.LoseHumor(humor, quantity);
        UpdateLOP();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void Cast(Spell spell)
    {
        if(spell.IsCastable(this) && !IsCasting)
        {
            IsCasting = true;
            spell.Cast(this);
            ReloadingSpells.Add(spell, spell.Cooldown);
            LoseHumor(spell.Humor, spell.Cost);
            IsCasting = false;
        }
    }

    public void UpdateCooldowns()
    {
        Spell[] s = new Spell[ReloadingSpells.Count];
        int i = 0;
        foreach(Spell spell in ReloadingSpells.Keys)
        {
            s[i] = spell;
            i++;
        }
        for (i = 0; i < s.Length; i++)
        {
            ReloadingSpells[s[i]] -= Time.deltaTime;
            if (ReloadingSpells[s[i]] <= 0)
            {
                s[i].HasReloaded();
                ReloadingSpells.Remove(s[i]);
                Debug.Log(Humors.Blood + " et " + Humors.Phlegm + " et " + Humors.BlackBile + " et " + Humors.YellowBile);
            }
        }
    }

    private void UpdateLOP()
    {
        LOP = (Humors.Blood + Humors.Phlegm + Humors.BlackBile + Humors.YellowBile) / 4;
    }
}
