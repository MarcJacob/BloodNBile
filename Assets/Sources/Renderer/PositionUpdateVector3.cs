using System;
using System.Collections.Generic;
using UnityEngine;

class PositionUpdateVector3
{
    public float x;
    public float y;
    public float z;

    public bool Forced;

    public PositionUpdateVector3(float x, float y, float z, bool forced)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        Forced = forced;
    }

    public static implicit operator Vector3(PositionUpdateVector3 pv)
    {
        return new Vector3(pv.x, pv.y, pv.z);
    }

    public static implicit operator PositionUpdateVector3(Vector3 v)
    {
        return new PositionUpdateVector3(v.x, v.y, v.z, false);
    }
}
