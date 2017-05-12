using System;
using System.Collections.Generic;

[Serializable]
public struct UnitRotationChangedMessage
{
    public int UnitID;
    public SerializableQuaternion NewQuaternion;

    public UnitRotationChangedMessage(int id, SerializableQuaternion quat)
    {
        UnitID = id;
        NewQuaternion = quat;
    }
}