using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public bool RootMotion = false;
    public float Speed = 10f;
    void Start()
    {

    }

    void Update()
    {
        if (RootMotion == true)
        {
            transform.GetComponent<Animator>().SetFloat("ForwardBackward", Input.GetAxis("Vertical"));
            transform.GetComponent<Animator>().SetFloat("LeftRight", Input.GetAxis("Horizontal"));
        }
        else
        {
            transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime * Speed, 0f, Input.GetAxis("Vertical") * Time.deltaTime * Speed, Space.Self);
        }

    }
}