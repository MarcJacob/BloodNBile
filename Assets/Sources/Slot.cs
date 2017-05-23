using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot {

    public Spell SlotSpell { get; private set; }
    public KeyCode Key { get; private set; }

    public Slot(KeyCode key)
    {
        SlotSpell = null;
        Key = key;
    }

    public Slot(Spell spell, KeyCode key)
    {
        SlotSpell = spell;
        Key = key;
    }

    public void ChangeSpell(Spell spell)
    {
        SlotSpell = spell;
    }

    public void ChangeKey(KeyCode key)
    {
        Key = key;
    }
}
