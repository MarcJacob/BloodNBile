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

    bool Menu = false;

	void Start () {

        transform.parent = pivot.transform;
        pivot.transform.parent = player.transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Menu = false;

    }
	
	void Update () {

        if (Menu == false)
        {
            if (cameraFollowPlayerAnimation)        // Condition qui détermine si la caméra doit suivre le joueur. Utilise à désactiver pour quelques animations où la caméra ne peut pas bouger.
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
        }
            
        cameraRotation = new Vector3(-Input.GetAxis("Mouse Y") * 0.5f * mouseSpeed, 0, 0);
        
        pivot.transform.Rotate(cameraRotation);


       if (pivot.transform.localRotation.x < -0.2f) 
        {
            pivot.transform.localRotation = new Quaternion(-0.2f, pivot.transform.localRotation.y, pivot.transform.localRotation.z, pivot.transform.localRotation.w);
        }

       if (pivot.transform.localRotation.x > 0.5f) 
        {
            pivot.transform.localRotation = new Quaternion(0.5f, pivot.transform.localRotation.y, pivot.transform.localRotation.z, pivot.transform.localRotation.w);
        }

       if (Input.GetKeyDown(KeyCode.Tab) && Menu == true)
       {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Menu = false;
       }
       else if (Input.GetKeyDown(KeyCode.Tab))
       {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Menu = true;
       }

	}
}
