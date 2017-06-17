using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class EffectBPCreateProjectile : EffectBlueprint
{

    public EffectBPCreateProjectile(float size, float speed, EffectBPProjectileHit onHitEffect)
    {
        Size = size;
        Speed = speed;
        OnHitEffect = onHitEffect;
    }

    float Size;
    float Speed;
    EffectBPProjectileHit OnHitEffect;

    public override void Instantiate(Unit caster, BnBMatch match)
    {
        match.EntityModule.CreateProjectile(caster, (Quaternion)caster.Rot * Vector3.forward, OnHitEffect, Size, Speed);
    }
}
