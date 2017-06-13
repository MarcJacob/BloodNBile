using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Mage : Unit {

    public float LOP { get; private set; }
    public bool IsCasting;
    public Dictionary<Spell, float> ReloadingSpells;

    public Mage(BnBMatch Match, int ID, Vector3 pos, Quaternion rot, string name, Faction fac, HumorLevels humors) : base(Match, ID, pos, rot, name, -1, 8, fac, 5, humors)
    {
        IsCasting = false;
        ReloadingSpells = new Dictionary<Spell, float>();
        LOP = (humors.Blood + humors.Phlegm + humors.BlackBile + humors.YellowBile) / 4;
    }

    /// <summary>
    /// Remove a certain quantity of a humor.
    /// </summary>
    /// <param name="humor">ID number of the humor : 0-Blood, 1-Phlegm, 2-Black Bile, 3-Yellow Bile</param>
    /// <param name="quantity"></param>

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
                ReloadingSpells.Remove(s[i]);
                Debugger.LogMessage(Humors.Blood + " et " + Humors.Phlegm + " et " + Humors.BlackBile + " et " + Humors.YellowBile);
            }
        }
    }

    public void UpdateLOP()
    {
        LOP = Humors.Blood + Humors.Phlegm + Humors.BlackBile + Humors.YellowBile;
    }


    const float HardUnbalanceCap = 0.6f;
    const float SoftUnbalanceCap = 0.4f;
    protected override void OnDamageTaken()
    {
        UpdateLOP();
        // Check les déséquilibres : Si l'une des humeurs arrivent à 60% du LOP ou 2 des humeurs chacunes à 40% du LOP alors le Mage meurt.

        if ((float)Humors.Blood / (float)LOP > HardUnbalanceCap)
        {
            Die();
        }
        else if ((float)Humors.Phlegm / (float)LOP > HardUnbalanceCap)
        {
            Die();
        }
        else if ((float)Humors.YellowBile / (float)LOP > HardUnbalanceCap)
        {
            Die();
        }
        else if ((float)Humors.BlackBile/ (float)LOP > HardUnbalanceCap)
        {
            Die();
        }
        else // check si il y a Soft Unbalance
        {
            int n = 0;
            if ((float)Humors.Blood / (float)LOP > SoftUnbalanceCap)
            {
                n++;
            }
            if ((float)Humors.Phlegm / (float)LOP > SoftUnbalanceCap)
            {
                n++;
            }
            if ((float)Humors.YellowBile / (float)LOP > SoftUnbalanceCap)
            {
                n++;
            }
            if ((float)Humors.BlackBile / (float)LOP > SoftUnbalanceCap)
            {
                n++;
            }

            if (n >= 2)
            {
                Die();
            }
        }
    }

}
