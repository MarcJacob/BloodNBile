using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public Animator animations;


	void Start () {
        animations = gameObject.GetComponent<Animator>();
	}
	
	void Update () {

        if (Input.GetKey(KeyCode.Z))
        {
            animations.CrossFade("MoveForward", 0.1f);
        }

        if (Input.GetKey(KeyCode.S))
        {
            animations.CrossFade("MoveBack", 0.1f);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            animations.CrossFade("MoveLeft", 0.1f);
        }

        if (Input.GetKey(KeyCode.D))
        {
            animations.CrossFade("MoveRight", 0.1f);
        }


	}
}
