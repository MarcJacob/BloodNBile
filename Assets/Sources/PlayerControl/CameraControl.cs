using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public float mouseSpeed = 1f;

    private Vector3 playerRotation;
    private Vector3 cameraRotation;
    public GameObject player;
    public GameObject pivot;
    public bool cameraFollowPlayerAnimation = true;

	void Start () {

        transform.parent = pivot.transform;
        pivot.transform.parent = player.transform;

    }
	
	void Update () {

        if (cameraFollowPlayerAnimation)        //Condition which determine if camera have to follow the rotation of player during an animation
        {
            pivot.transform.parent = player.transform;
            playerRotation = new Vector3(0, Input.GetAxis("Mouse X") * mouseSpeed, 0);
            player.transform.Rotate(playerRotation);
        }
        else
        {
            pivot.transform.parent = null;
            cameraRotation = new Vector3(0, Input.GetAxis("Mouse X") * mouseSpeed, 0);
            pivot.transform.Rotate(cameraRotation);
        }
            
        cameraRotation = new Vector3(Input.GetAxis("Mouse Y") * 0.5f * mouseSpeed, 0, 0);
        
        pivot.transform.Rotate(cameraRotation);


       if (pivot.transform.localRotation.x < -0.2f) 
        {
            pivot.transform.localRotation = new Quaternion(-0.2f, pivot.transform.localRotation.y, pivot.transform.localRotation.z, pivot.transform.localRotation.w);
        }

       if (pivot.transform.localRotation.x > 0.5f) 
        {
            pivot.transform.localRotation = new Quaternion(0.5f, pivot.transform.localRotation.y, pivot.transform.localRotation.z, pivot.transform.localRotation.w);
        }

       if (Input.GetKeyDown(KeyCode.A))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

       if (Input.GetKeyDown(KeyCode.E))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

	}
}
