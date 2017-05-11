using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Unit : DrawableEntity {

    public Faction Fac;
    protected int HealthPoints = 100;


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

    protected void AddToCell(CellsManager Cells)
    {
            int x = (int) Pos.z / Cells.SizeCellX;
            int y = (int) Pos.x / Cells.SizeCellY;
            Cells.cells[x, y].Add(this);
    }

    public void RemoveFromCell(CellsManager Cells, int x, int y)
    {
        Cells.cells[x, y].Remove(this);
    }

    protected int GetUnitPositionX(CellsManager Cells)
    {
        int x = (int)Pos.z / Cells.SizeCellX;
        return x;
    }

    protected int GetUnitPositionY(CellsManager Cells)
    {
        int y = (int)Pos.x / Cells.SizeCellY;
        return y;
    }

    public void AddHP(int healPoints)
    {
        this.HealthPoints += healPoints;
    }

    public void RemoveHP(int healPoints)
    {
        this.HealthPoints -= healPoints;
        if (HealthPoints <= 0f && OnUnitDiedCallback != null)
        {
            Die();
        }
    }

    public override void Die()
    {
        base.Die();
        OnUnitDiedCallback(this);
    }

    public int GetHealthPoints()
    {
        return HealthPoints;
    }

    static Action<Unit> OnUnitDiedCallback;
    public static void RegisterOnUnitDiedCallback(Action<Unit> cb)
    {
        OnUnitDiedCallback += cb;
    }


    void Start () {
		
	}
	
	
	public override void UpdateEntity () {

        if (MovementVector != Vector3.zero || WilledMovementVector != Vector3.zero)
        {
            ProcessMovement();
        }
	}

    void ProcessMovement()
    {
        Pos += (SerializableVector3)((Vector3)MovementVector + (Quaternion)Rot * (Vector3)WilledMovementVector * Time.deltaTime);
    }

    public SerializableVector3 WilledMovementVector { get; private set; } // Mouvement que l'unité applique sur elle même.
    /// <summary>
    /// Si l'unité est en capacité d'influencer son propre mouvement, fait avancer l'unité dans la direction indiquée.
    /// Le mouvement ajouté au vecteur mouvement dépend de la vitesse de l'unité.
    /// </summary>
    /// <param name="dir"></param>
    public void Move(Vector3 dir)
    {
        if (CanMove)
        {
            WilledMovementVector = dir;
            if (OnUnitMovementVectorChanged != null)
            {
                OnUnitMovementVectorChanged(this);
            }
        }
    }

    public void PreventWilledMovement()
    {
        CanMove = false;
        WilledMovementVector = Vector3.zero;
        if (OnUnitMovementVectorChanged != null)
        {
            OnUnitMovementVectorChanged(this);
        }
    }

    public void AllowMovement()
    {
        CanMove = true;
    }

    public void ApplyMovement(Vector3 mov)
    {
        MovementVector += (SerializableVector3)mov;
        if (OnUnitMovementVectorChanged != null)
        {
            OnUnitMovementVectorChanged(this);
        }
    }

    public void SetMovement(Vector3 mov)
    {
        MovementVector = mov;
        Debug.Log("Setting movement to " + mov);
        if (OnUnitMovementVectorChanged != null)
        {
            OnUnitMovementVectorChanged(this);
        }
    }

    static Action<Unit> OnUnitMovementVectorChanged;
    static public void RegisterOnUnitMovementVectorChanged(Action<Unit> cb)
    {
        OnUnitMovementVectorChanged += cb;
    }
}
