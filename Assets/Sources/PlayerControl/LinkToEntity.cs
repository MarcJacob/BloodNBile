using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Cette classe permet de "lier" un GameObject à une entité d'un certain identifiant contenu dans l'objet EntityRenderer.
/// Plus précisement, cette classe force le GameObject à conserver la position que lui dicte le serveur, tout en étant libre de ses mouvements
/// (côté client) entre deux de ces mises à jours. Si le client et le serveur sont synchronisés alors le mouvement du GameObject devrait
/// correspondre à celui de l'entitée liée côté serveur et donc avoir un déplacement fluide.
/// </summary>
public class LinkTo : MonoBehaviour {

    public Unit LinkedEntity;
    public bool TrackRotation = true;
    public bool TrackLocation = true;
    public void LinkEntity(Unit e)
    {
        LinkedEntity = e;
        OnEntityPositionUpdated(e, true);
    }

    public void Initialize(Unit e, EntityRenderer renderer)
    {
        LinkEntity(e);
        renderer.RegisterOnUnitPositionUpdatedCallback(OnEntityPositionUpdated);
        renderer.RegisterOnUnitRemovedCallback(OnEntityDied);
        Initialized = true;
    }

    public void OnEntityDied(Unit e)
    {
        if (e.Equals(LinkedEntity))
        {
            Debugger.LogMessage("Mort de l'entité " + e.Name);
            Destroy(gameObject);
        }
    }

    Vector3 LastKnownServerPosition; // Dernière position sur le serveur connue.
    Quaternion LastKnownServerRotation; // Dernière rotation sur le serveur connue.

    Vector3 CurrentMovementVector; // Vecteur mouvement en direction de la dernière position sur le serveur connue.
    bool FastPositionUpdate = false; // Si vrai, alors l'entité va LERP vers sa dernière position connue au lieu d'y aller
    // en fonction de sa vitesse. Fait pour les mises à jour de la position après un coup de lag par exemple.
    public void OnEntityPositionUpdated(Unit unit, bool forced)
    {
        if (unit == LinkedEntity)
        {
            LastKnownServerPosition = unit.Pos;
            LastKnownServerRotation = unit.Rot;
        }
            
    }

    bool Initialized = false;
    float MaxDistanceToServerPosition = 5f; // Distance maximale à la position dictée par le serveur si cette entité ne traque
    // pas la position de son entité associée constamment.
    float MaxDistanceToServerPositionFalling = 10f; // IDEM mais durant une chute. Permet d'éviter les "lag de chute".
    void Update()
    {
        if (Initialized)
        {
            // Si on ne traque pas la position de l'entité "directement", alors on attend d'être trop éloigné de la position
            // dictée par le serveur avant de rectifier.
            if (!TrackLocation && ((LastKnownServerPosition - transform.position).sqrMagnitude > MaxDistanceToServerPosition * MaxDistanceToServerPosition || (!TrackLocation && (LastKnownServerPosition - transform.position).sqrMagnitude > MaxDistanceToServerPositionFalling* MaxDistanceToServerPositionFalling && (int)LastKnownServerPosition.y != (int)transform.position.y)))
            {
                transform.position = LastKnownServerPosition;
            }
            // Sinon on Lerp vers la dernière position connue sur le serveur
            else
            {
                if (TrackLocation && (transform.position - LastKnownServerPosition).sqrMagnitude > 0.04f)
                    transform.position = Vector3.Lerp(transform.position, LastKnownServerPosition, Time.deltaTime * 4f);
                else if (TrackLocation)
                    transform.position = Vector3.Lerp(transform.position, LastKnownServerPosition, Time.deltaTime * 8f);
                if (TrackRotation)
                transform.rotation = Quaternion.Lerp(transform.rotation, LastKnownServerRotation, Time.deltaTime * 5f);
            }
        }
    }
}
