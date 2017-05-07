using System;
using System.Collections;
using UnityEngine;

[Serializable]
public struct SerializableVector3
{
    public float x;
    public float y;
    public float z;

    public SerializableVector3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public static implicit operator Vector3(SerializableVector3 sv)
    {
        return new Vector3(sv.x, sv.y, sv.z);
    }

    public static implicit operator SerializableVector3(Vector3 v)
    {
        return new SerializableVector3(v.x, v.y, v.z);
    }

    public static SerializableVector3 operator +(SerializableVector3 sv1, SerializableVector3 sv2)
    {
        return new SerializableVector3(sv1.x + sv2.x, sv1.y + sv2.y, sv1.z + sv2.z);
    }
}