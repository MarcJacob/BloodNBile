using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
class UnitsCreationMessage
{
    public int MatchID;
    public int[] IDs;
    public SerializableVector3[] Positions;
    public SerializableQuaternion[] Rotations;
    public int[] MeshIDs;
    public HumorLevels[] Humors;
    public string[] Names;
    public float[] Sizes;
    public Faction[] Factions;
    public float[] Speeds;

    public UnitsCreationMessage(int matchID, int[] ids, SerializableVector3[] pos, SerializableQuaternion[] rots, int[] meshids, HumorLevels[] humors, string[] names, float[] sizes, Faction[] facs, float[] speeds)
    {
        MatchID = matchID;
        IDs = ids;
        Positions = pos;
        Rotations = rots;
        MeshIDs = meshids;
        Humors = humors;
        Names = names;
        Sizes = sizes;
        Factions = facs;
        Speeds = speeds;
    }
}
