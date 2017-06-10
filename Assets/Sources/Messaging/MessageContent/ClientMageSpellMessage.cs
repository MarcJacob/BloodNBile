using System;

[Serializable]
public class ClientMageSpellMessage
{

    public int MageID;
    public int SpellID;
    public HumorLevels Humors;

    public ClientMageSpellMessage(int mageID, int spellID)
    {
        MageID = mageID;
        SpellID = spellID;
    }
    public ClientMageSpellMessage(int mageID, int spellID, HumorLevels humors)
    {
        MageID = mageID;
        SpellID = spellID;
        Humors = humors;
    }
}
