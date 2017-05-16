using System;

[Serializable]
public class EntityPositionRotationUpdate
{
    public SerializableVector3 NewPosition;
    public SerializableQuaternion NewRotation;
    public int EntityID;

    public EntityPositionRotationUpdate(int id, SerializableVector3 pos, SerializableQuaternion rot)
    {
        NewPosition = pos;
        NewRotation = rot;
        EntityID = id;
    }
}