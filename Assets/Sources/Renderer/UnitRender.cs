using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitRender // représentation de l'unité coté client
{
    public Vector3 CurrentPos;
    public Quaternion CurrentRot;
    public Unit RenderedUnit; // réference vers l'unité

    public UnitRender(Vector3 pos, Quaternion rot, Unit unit)
    {
        CurrentPos = pos;
        CurrentRot = rot;
        RenderedUnit = unit;
    }


    static float smoothSpeed = 4f;
    public void Process()
    {
		/*cette fonction actualise la position d'une unité coté client en faisant une moyenne pondérée des deux positions d'une unité
		Plus le ping du client est important, plus la position finale sera proche de celle calculée par le serveur.
		*/ 
        CurrentPos = Vector3.Lerp(CurrentPos, RenderedUnit.Pos, Time.deltaTime * smoothSpeed);
        CurrentRot = Quaternion.Lerp(CurrentRot, RenderedUnit.Rot, Time.deltaTime * smoothSpeed * 2);
    }
}
