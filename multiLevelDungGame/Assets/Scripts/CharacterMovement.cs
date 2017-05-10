using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent] //prevents it from being attached to more than one gameobjcet
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class CharacterMovement : MonoBehaviour {

    Animator charAnim;
    bool jumping = false;
    bool swinging = false;
    bool combo1Available = false;
    bool combo1InUse = false;
    bool combo2Available = false;
    bool combo2InUse = false;

    void Awake()
    {
        charAnim = GetComponent<Animator>();
    }

    void Update()
    {
        Turn();
        Move();
        Jump();
        Swing();
        ComboOne();
        ComboTwo();
    }

    void Turn()
    {
        //charAnim.SetFloat("Turning", Input.GetAxis("Horizontal"));
        //charAnim.SetFloat("Turning", Input.GetAxis("Horizontal2"));
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

        //charAnim.SetFloat("Forward", Input.GetAxis("Vertical"));
        charAnim.SetFloat("MoveZ", Input.GetAxis("Vertical2"));
        charAnim.SetFloat("MoveX", Input.GetAxis("Horizontal2"));
        //Debug.Log(Input.GetAxisRaw("Vertical") + ";;" + Input.GetAxis("Vertical"));
    }

    void Jump()
    {
        Debug.Log(Input.GetAxis("Jump"));
        if ((Input.GetAxisRaw("Jump") > 0.2) && jumping == false)
        {
            jumping = true;
            charAnim.SetTrigger("Jump");
            StartCoroutine(JumpTime());
        }
    } 

    IEnumerator JumpTime()
    {
        yield return new WaitForSeconds(1);
        jumping = false;
    }

    void DamageRecieved()
    {
        charAnim.SetTrigger("Damaged");
    }

    void PowerUpCollected()
    {
        charAnim.SetTrigger("Powerup");
    }

    void Swing()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button0)) 
        {
            if(swinging == false)
            {
                if (combo1Available == true)
                {
                    charAnim.SetTrigger("Swing2");
                    swinging = true;
                    combo1InUse = true;
                    StartCoroutine(ComboTime());
                }
                else if (combo2Available == true)
                {
                    Debug.Log("SwingCalled");
                    charAnim.SetTrigger("Swing3");
                    swinging = true;
                    combo2InUse = true;
                    StartCoroutine(ComboTime());
                }
                else
                {
                    charAnim.SetTrigger("Swing");
                    swinging = true;
                    StartCoroutine(ComboTime());
                }
            }
            Debug.Log("button Pressed");
            
        }
    }

    void ComboOne()
    {
        if (combo1Available == true)
        {
           
            StartCoroutine(ComboBoolReset());
        }
    }

    void ComboTwo()
    {
        if (combo2Available == true)
        {
            Debug.Log("ResetCalled");
            StartCoroutine(ComboBoolReset());
        }
    }


    IEnumerator ComboTime()
    {
        yield return new WaitForSeconds(0.5f);
        swinging = false;
        if (combo1InUse ==  false && combo2InUse == false)
        {
            combo1Available = true;
        }
        else if (combo1InUse == true)
        {
            combo2Available = true;
        }
        combo1InUse = false;
        combo2InUse = false;

    }

    IEnumerator ComboBoolReset()
    {
        yield return new WaitForSeconds(0.5f);
        if(combo1Available == true)
        {
            combo1Available = false;
        }
          
       if(combo2Available == true && combo2InUse == true)
        {
            Debug.Log("EarlyReset");
            combo2Available = false;
        }  

            
     
    }
}
