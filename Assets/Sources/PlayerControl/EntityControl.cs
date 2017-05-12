using System;
using System.Collections.Generic;
using UnityEngine;

public class EntityControl : MonoBehaviour
{

    NetworkSocketInfo NetworkInfo;
    LinkTo EntityLink;

    public void Initialize(NetworkSocketInfo netInfo)
    {
        NetworkInfo = netInfo;
        EntityLink = GetComponent<LinkTo>();
        EntityLink.TrackLocation = false;
        EntityLink.TrackRotation = false;
    }

    private void Start()
    {
        Camera.main.transform.position = transform.position + new Vector3(0f, 2.5f, -4f);
        GameObject pivot = new GameObject("CameraPivot");
        pivot.transform.position = transform.position + new Vector3(0, 1.5f, 0);
        pivot.transform.rotation = transform.rotation;
        pivot.transform.parent = transform;
        Camera.main.transform.parent = pivot.transform;
    }

    Vector3 DirectionVector;
    bool Changed = false;

    private void Update()
    {
        if (NetworkInfo != null)
        {
            Mouselook();
            HandleInput();
            if (Changed)
            {
                new NetworkMessage(14, new UnitMovementChangeMessage(EntityLink.LinkedEntity.ID, DirectionVector)).Send(NetworkInfo, NetworkInfo.ConnectionIDs[0]);
            }
        }

        transform.Translate(EntityLink.LinkedEntity.GetSpeed() * DirectionVector * Time.deltaTime);
    }

    float RotationUpdateFrequency = 0.5f;
    float RotationUpdateCooldown = 0f;
    void Mouselook()
    {
        float rawX = Input.GetAxis("Mouse X");
        float rawY = Input.GetAxis("Mouse Y");

        transform.eulerAngles += new Vector3(0, rawX, 0);
        transform.Find("CameraPivot").eulerAngles += new Vector3(-rawY, 0, 0);

        RotationUpdateCooldown += Time.deltaTime;
        if (RotationUpdateCooldown >= RotationUpdateFrequency)
        {
            RotationUpdateCooldown = 0f;
            Debug.Log("Envoie d'une nouvelle rotation au serveur");
            new NetworkMessage(16, new UnitRotationChangedMessage(EntityLink.LinkedEntity.ID, (SerializableQuaternion)transform.rotation)).Send(NetworkInfo, NetworkInfo.ConnectionIDs[0], NetworkInfo.UnreliableChannelID);
        }
    }

    void HandleInput()
    {
        Changed = false;
        if (Input.GetKeyDown(KeyCode.Z))
        {
            DirectionVector.z += 1f;
            Changed = true;
        }
        else if (Input.GetKeyUp(KeyCode.Z))
        {
            DirectionVector.z += -1f;
            Changed = true;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            DirectionVector.z += -1f;
            Changed = true;
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            DirectionVector.z += 1f;
            Changed = true;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            DirectionVector.x += 1f;
            Changed = true;
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            DirectionVector.x += -1f;
            Changed = true;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Changed = true;
            DirectionVector.x += -1f;
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            DirectionVector.x += 1f;
            Changed = true;
        }

        //Debug.Log(DirectionVector);
    }
}