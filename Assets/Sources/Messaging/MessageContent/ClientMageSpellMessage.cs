using System;

[Serializable]
public class ClientMageSpellMessage
{

    public int MageID;
    public int SpellID;

    public ClientMageSpellMessage(int mageID, int spellID)
    {
        MageID = mageID;
        SpellID = spellID;
    }
}
