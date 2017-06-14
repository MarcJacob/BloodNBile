using System;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : DrawableEntity
{
    public Projectile(BnBMatch Match, int ID, Unit source, Vector3 Dest, float size, float speed) : base(Match.ID, ID, source.Pos, Quaternion.identity, "Projectile", 1001, size)
    {

    }

    int SourceEntityID;
    SerializableVector3 Dest;
}
