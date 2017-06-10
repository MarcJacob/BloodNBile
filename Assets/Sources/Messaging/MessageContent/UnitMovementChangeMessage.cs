using System;

[Serializable]
public struct UnitMovementChangeMessage
{
    public int UnitID; // Identifiant de l'unité concernée.
    public SerializableVector3 NewMovementVector; // Nouveau vecteur mouvement.

    public UnitMovementChangeMessage(int id, SerializableVector3 vector)
    {
        UnitID = id;
        NewMovementVector = vector;
    }
}
