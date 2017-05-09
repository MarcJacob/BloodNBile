using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Unit : DrawableEntity {

    public Faction Fac;
    protected int HealthPoints = 100;


    public SerializableVector3 Destination { get; private set; } // L'entitée sera constamment déplacée vers sa destination par le serveur.
    public bool HasDestination = false; // L'entitée est-elle en cours de route vers une destination ? Si True alors le déplacement se fera.
    float BaseSpeed; // Vitesse "de base" de l'unité en mètres par seconde.


    public Unit(BnBMatch Match, int ID, Vector3 pos, Quaternion rot, string name, int mesh, float size, Faction fac, float speed = 5f) : base(Match, ID, pos, rot, name, mesh, size)
    {
        BaseSpeed = speed;
        Fac = fac;
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

    /// <summary>
    /// Set la destination de l'unité.
    /// </summary>
    /// <param name="dest"></param>
    public void SetDestination(Vector3 dest)
    {
        HasDestination = true;
        Destination = dest;

        // Exécution du callback quand une unité change de destination.
        if (OnDestinationSetCallback != null)
        OnDestinationSetCallback(this);
    }

    static Action<Unit> OnDestinationSetCallback;
    public static void RegisterOnDestinationSetCallback(Action<Unit> cb)
    {
        OnDestinationSetCallback += cb;
    }

    void OnArrivedToDestination()
    {
        HasDestination = false;
        Debug.Log(Name + " est arrivé à destination !");
        if (OnArrivedToDestinationCallback != null)
        OnArrivedToDestinationCallback(this);
    }

    static Action<Unit> OnArrivedToDestinationCallback;
    public static void RegisterOnArrivedToDestinationCallback(Action<Unit> cb)
    {
        OnArrivedToDestinationCallback += cb;
    }

    void Start () {
		
	}
	
	
	public override void UpdateEntity () {

        // Déplacement vers la destination
        if (HasDestination == true)
        {
            Vector3 movementVector = Destination - Pos;
            movementVector.Normalize();
            SetPos((Vector3)Pos + movementVector * BaseSpeed * Time.deltaTime);
            if ((Destination - Pos).sqrMagnitude < Size*Size) // Calcul de la distance à la destination. Si cette distance est inférieur à la taille de l'unité alors elle l'a atteint.
            {
                OnArrivedToDestination();
            }
        }

	}
}
