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
        renderer.RegisterOnUnitRotationUpdatedCallback(OnEntityRotationUpdated);
        renderer.RegisterOnUnitRemovedCallback(OnEntityDied);
        Initialized = true;
    }

    public void OnEntityDied(Unit e)
    {
        if (e.Equals(LinkedEntity))
        {
            Debug.Log("Mort de l'entité " + e.Name);
            Destroy(gameObject);
        }
    }

    public void OnEntityPositionUpdated(Unit unit, bool forced)
    {
        if (forced == true)
            Debug.Log("MAJ forcée");
        if (LinkedEntity == unit && (forced || TrackLocation))
        {
            Debug.Log("Mise à jour de la position de l'entité ID " + unit.ID);
            transform.position = unit.Pos;
        }
    }
    
    public void OnEntityRotationUpdated(Unit unit)
    {
        if (LinkedEntity == unit && TrackRotation)
        {
            Debug.Log("Mise à jour de la rotation de l'enttié ID " + unit.ID);
            transform.rotation = unit.Rot;
        }
    } 

    bool Initialized = false;

    void Update()
    {
        if (Initialized)
        {

        }
    }
}
