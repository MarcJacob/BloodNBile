using System;
using System.Collections;
using UnityEngine;

[Serializable]
public struct SerializableQuaternion
{
    public float x;
    public float y;
    public float z;
    public float w;

    public SerializableQuaternion(float x, float y, float z, float w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    public static implicit operator Quaternion(SerializableQuaternion sq)
    {
        return new Quaternion(sq.x, sq.y, sq.z, sq.w);
    }

    public static implicit operator SerializableQuaternion(Quaternion q)
    {
        return new SerializableQuaternion(q.x, q.y, q.z, q.w);
    }
}