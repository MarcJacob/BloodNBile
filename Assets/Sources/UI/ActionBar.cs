using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBar {

    public Dictionary<KeyCode, Slot> Slots { get; private set; }
    public Slot CurrentSlot { get; private set; }
    public Mage ActionBarMage { get; private set; }
    public const int NB_SLOTS = 10;

    public ActionBar(Mage mage)
    {
        Slots = new Dictionary<KeyCode, Slot>();

        for(int i=0; i<NB_SLOTS-1; i++)
        {
            Slot slot = new Slot(Spell.GetSpellFromID(i), (KeyCode)(49 + i));
            Slots.Add(slot.Key, slot);
        }
        Slot slot2 = new Slot(Spell.GetSpellFromID(NB_SLOTS-1), (KeyCode)48);
        Slots.Add(slot2.Key, slot2);

        CurrentSlot = Slots[(KeyCode)49];
        ActionBarMage = mage;
    }

    public void ChangeSlot(Slot slot)
    {
        CurrentSlot = slot;
        Debug.Log("Slot changé !");
    }

    public void ChangeSlotSpell(Slot slot, Spell spell)
    {
        slot.ChangeSpell(spell);
    }

}
