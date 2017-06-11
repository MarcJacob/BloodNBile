using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Unit : DrawableEntity {

    public Faction Fac;
    protected HumorLevels Humor;


    // Movement
    float BaseSpeed; // Vitesse "de base" de l'unité en mètres par seconde.
    public SerializableVector3 MovementVector { get; private set; }
    public bool CanMove { get; private set; } // L'unité peut-elle influencer son propre mouvement ? Est faux par exemple quand l'unité est entrain de tomber
                         // ou est affectée par un phénomène physique.

    public Unit(BnBMatch Match, int ID, Vector3 pos, Quaternion rot, string name, int mesh, float size, Faction fac, float speed = 5f) : base(Match, ID, pos, rot, name, mesh, size)
    {
        BaseSpeed = speed;
        Fac = fac;
        CanMove = true;
    }

    public float GetSpeed()
    {
        return BaseSpeed;
    }

    public virtual bool IsDead() // A refaire mes choupinous
    {
        return false;
    }

    /**
     * <summary> Permet de voir si cette unité se trouve sur une autre case que celle passé en paramètre. </summary>
     */
    public bool IsInCell(CellsManager Cells, Cell cell)
    {
        int x = (int)Pos.x / Cells.SizeCellX;
        int y = (int)Pos.z / Cells.SizeCellY;

        if (x != cell.PositionX || y != cell.PositionY)
        {
            return true;
        }
        else
            return false;
    }

    public void RemoveHumors(int type, int quantity)
    {
        Humor.LoseHumor(type, quantity);
        CheckDeath();
    }

    virtual protected void CheckDeath()
    {
        if (Humor.Blood <= 0 && Humor.Phlegm <= 0 && Humor.BlackBile <= 0 && Humor.YellowBile <= 0)
        {
            Die();
        }
    }

    public override void Die()
    {
        base.Die();
        OnUnitDiedCallback(this);

    }


    static Action<Unit> OnUnitDiedCallback;
    public static void RegisterOnUnitDiedCallback(Action<Unit> cb)
    {
        OnUnitDiedCallback += cb;
    }



	public override void UpdateEntity ()
    {
        
	}


    public override void SetRot(Quaternion quat)
    {
        base.SetRot(quat);
        if (OnUnitRotationChanged != null)
        OnUnitRotationChanged(this);
    }

    static Action<Unit> OnUnitMovementVectorChanged;
    static public void RegisterOnUnitMovementVectorChanged(Action<Unit> cb)
    {
        OnUnitMovementVectorChanged += cb;
    }

    static Action<Unit> OnUnitRotationChanged;
    static public void RegisterOnUnitRotationChanged(Action<Unit> cb)
    {
        OnUnitRotationChanged += cb;
    }
}
