using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

[DisallowMultipleComponent] //prevents it from being attached to more than one gameobjcet
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class CharacterBehaviour : MonoBehaviour
{

    Animator charAnim;
    bool jumping = false;
    bool swinging = false;
    bool damageSend = true;
    bool isBlocking = false;
    bool combo1Available = false;
    bool combo1InUse = false;
    bool combo1AnimInProg = false;
    bool combo2Available = false;
    bool combo2InUse = false;
    bool combo2AnimInProg = false;

    List<Collider> enemColliders;

    //AudioSources
    AudioSource blockedAttack;
    AudioSource swingSound;
    AudioSource swingSound2;
    AudioSource swingSound3;
    AudioSource damageFlinchSound;

    public float playerHealth = 100.0f;
    public bool playerShield = false;
    public int shieldCount = 0;
    public float playerDamage = 20.0f;

    void Awake()
    {
        charAnim = GetComponent<Animator>();
        enemColliders = new List<Collider>();

        //Sounds
        AudioSource[] audios = GetComponents<AudioSource>();
        blockedAttack = audios[0];
        swingSound = audios[1];
        swingSound2 = audios[2];
        swingSound3 = audios[3];
        damageFlinchSound = audios[4];
    }

    void Update()
    {
        //Turn();
        //Move();
        //Jump();
        Swing();
        ComboOne();
        ComboTwo();
        Blocking();
        IsDead();
        if (swinging == true && damageSend == true)
        {
            SendDamage();
        }
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
        charAnim.SetTrigger("DamageTaken");
        damageFlinchSound.Play();
    }

    void PowerUpCollected()
    {
        charAnim.SetTrigger("Powerup");
    }

    void Swing()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            if (swinging == false)
            {
                
                if (combo1Available == true)
                {
                    swingSound2.PlayDelayed(0.9f);
                    charAnim.SetTrigger("Swing2");
                    swinging = true;
                    combo1InUse = true;
                    combo1AnimInProg = true;
                    StartCoroutine(SecondSwingProgress());
                    StartCoroutine(ComboTime());
                }
                else if (combo2Available == true)
                {
                    swingSound3.PlayDelayed(1.05f);
                    charAnim.SetTrigger("Swing3");
                    swinging = true;
                    combo2InUse = true;
                    combo2AnimInProg = true;
                    StartCoroutine(ThirdSwingProgress());
                    StartCoroutine(ComboTime());
                }
                else
                {
                    swingSound.PlayDelayed(0.5f);
                    charAnim.SetTrigger("Swing");
                    swinging = true;
                    StartCoroutine(ComboTime());
                }
            }

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
            StartCoroutine(ComboBoolReset());
        }
    }


    IEnumerator ComboTime()
    {
        yield return new WaitForSeconds(0.8f);
        swinging = false;
        //Debug.Log("not swinging");
        //Debug.Break();
        damageSend = true;
        if (combo1InUse == false && combo2InUse == false)
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
       // Debug.Log("Combo Missed");
        //Debug.Break();
        if (combo1Available == true)
        {
            combo1Available = false;
        }

        if (combo2Available == true && combo2InUse == true)
        {
            combo2Available = false;
        }
    }

    IEnumerator SecondSwingProgress()
    {
        yield return new WaitForSeconds(1.1f);
        combo1AnimInProg = false;
    }

    IEnumerator ThirdSwingProgress()
    {
        yield return new WaitForSeconds(1.5f);
        //Debug.Break();
        combo2AnimInProg = false;
    }

    void Blocking()
    {
        if (Input.GetButtonDown("Block"))
        {
            charAnim.SetBool("Blocking", true);
            isBlocking = true;
        }
        if (Input.GetButtonUp("Block"))
        {
            charAnim.SetBool("Blocking", false);
            isBlocking = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("Enemy123");
            //enemColliders.Add(other);
            if (swinging == true || combo1AnimInProg == true || combo2AnimInProg == true)
            {
                if(combo2AnimInProg == true)
                {
                    other.gameObject.SendMessage("TakeDamage", playerDamage*2);
                }
                else
                {
                    other.gameObject.SendMessage("TakeDamage", playerDamage);
                }
                
            }
        }
    }

    //void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.tag == "Enemy")
    //    {
    //        enemColliders.Remove(other);
    //    }
    //}

    void SendDamage()
    {
        damageSend = false;
        foreach (Collider o in enemColliders)
        {
            o.gameObject.SendMessage("TakeDamage", playerDamage);
        }
    }

    void TakeDamage(float damage)
    {
        if(isBlocking != true)
        {
            if (playerShield == true && shieldCount > 0)
            {
                shieldCount -= 1;
                DamageRecieved();
            }
            else
            {
                playerHealth -= damage;
                DamageRecieved();
            }
        }
        else
        {
            blockedAttack.Play();
        }
    }

    void IsDead()
    {
        if (playerHealth <= 0)
        {
            charAnim.SetBool("IsDead", true);
            SceneManager.LoadScene("MainMenu");

        }
    }
}


