using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Effect {

    /** <summary> Comportement quand l'effet est déclenché (première image). </summary> */
    public abstract void Activate();
    /** <summary> Comportement quand l'effet est en cours (chaque image). </summary> */
    public abstract void Update();
    /** <summary> Comportement quand l'effet se termine (dernière image). </summary> */
    public virtual void Destroy() { Alive = false; }

    public bool Alive { get; private set; }

    public Effect(int matchID)
    {
        MatchID = matchID;
        if (OnEffectCreated != null)
        OnEffectCreated(this);
        Alive = true;
    }

    public int MatchID { get; private set; }

    static Action<Effect> OnEffectCreated;

    static public void RegisterOnEffectCreatedCallback(Action<Effect> cb)
    {
        OnEffectCreated += cb;
    }
}
