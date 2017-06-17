using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class ProjectilesCreationMessage
{
    public int[] IDs;
    public int[] MeshIDs;
    public SerializableVector3[] StartPositions;
    public SerializableVector3[] Directions;
    public float[] Speeds;
    public float[] Sizes;

    public ProjectilesCreationMessage(int[] IDs, int[] MeshIDs, SerializableVector3[] StartPositions, SerializableVector3[] Directions, float[] Speeds, float[] Sizes)
    {
        this.IDs = IDs;
        this.StartPositions = StartPositions;
        this.Directions = Directions;
        this.Speeds = Speeds;
        this.Sizes = Sizes;
    }
}
