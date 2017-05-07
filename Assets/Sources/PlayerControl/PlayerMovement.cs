using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {


	void Start () {
        
	}

	void Update () {

        if (Input.GetAxis("Vertical") != 0)
        {
            MoveForwardBackward(Input.GetAxis("Vertical"));
        }

        if (Input.GetAxis("Horizontal") != 0)
        {
            MoveLeftRight(Input.GetAxis("Horizontal"));
        }
    }

    void MoveForwardBackward(float axis)
    {

    }

    void MoveLeftRight(float axis)
    {

    }
}
