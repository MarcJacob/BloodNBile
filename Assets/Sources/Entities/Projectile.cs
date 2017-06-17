using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Projectile : DrawableEntity
{

    public Projectile(int MatchID, int ID, Unit source, Vector3 Dir, float size, float speed, EffectBPProjectileHit onCollisionEffect) : base(MatchID, ID, source.Pos, Quaternion.identity, "Projectile", 1001, size)
    {
        SourceEntityID = source.ID;
        this.Direction = Dir;
        OnCollisionEffect = onCollisionEffect;
        Speed = speed;
    }

    public override void UpdateEntity(float deltaTime)
    {
        base.UpdateEntity(deltaTime);

        SetPos((Vector3)Pos + (Vector3)Direction * Speed * deltaTime);

    }

    public void CheckCollision(CellsManager CellsModule)
    {
        // Check collision
        Cell cell = CellsModule.GetCellAtCoordinates(Pos.x, Pos.z);

        if (cell != null)
        {
            foreach (Unit u in cell.UnitList)
            {
                if (u.ID != SourceEntityID)
                if ((new Vector2(Pos.x, Pos.z) - new Vector2(u.Pos.x, u.Pos.z)).sqrMagnitude - Size < u.Size * u.Size)
                {
                    OnTargetHit(u);
                }
            }
        }
        else
        {
            Die();
        }
    }

    public override void Die()
    {
        base.Die();
        OnProjectileDestroyed(this);
    }

    public int SourceEntityID { get; private set; }
    public SerializableVector3 Direction { get; private set; }
    public float Speed { get; private set; } // En unités par seconde.

    static Action<Projectile> OnProjectileDestroyed;
    static public void RegisterOnProjectileDestroyedCallback(Action<Projectile> cb)
    {
        OnProjectileDestroyed += cb;
    }

    public EffectBPProjectileHit OnCollisionEffect { get; private set; }

    static Action<Projectile, Unit> OnProjectileHitTarget;
    static public void RegisterOnProjectHitTarget(Action<Projectile, Unit> cb)
    {
        OnProjectileHitTarget += cb;
    }
    public virtual void OnTargetHit(Unit unit)
    {
        OnCollisionEffect.SetTarget(unit); // On dit à l'EffectBlueprint associé à ce projectile que la cible sur laquelle appliqué l'effet sera unit.
        // L'effet est instancié de l'extérieur car il y a besoin d'informations dont le projectile ne dispose pas.
        OnProjectileHitTarget(this, unit);
        Die();
    }
}
