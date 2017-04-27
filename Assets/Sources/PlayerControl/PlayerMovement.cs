using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {


	void Start () {
        
	}
	
	void Update () {

        transform.GetComponent<Animator>().SetFloat("ForwardBackward", Input.GetAxis("Vertical"));
        transform.GetComponent<Animator>().SetFloat("LeftRight", Input.GetAxis("Horizontal"));


	}
}
