using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent] //prevents it from being attached to more than one gameobjcet
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class CharacterMovement : MonoBehaviour {
    Animator charAnim;

    void Awake()
    {
        charAnim = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            charAnim.SetBool("Walk", true);
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            charAnim.SetBool("Walk", false);
        }

        charAnim.SetFloat("Forward", Input.GetAxisRaw("Vertical"));
        Debug.Log(Input.GetAxisRaw("Vertical"));
    }

}
