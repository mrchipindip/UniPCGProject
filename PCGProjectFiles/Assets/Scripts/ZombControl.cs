using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombControl : MonoBehaviour {

    public Transform player;
    Animator zombAnim;
    public float health = 10.0f;
    public float damage = 10.0f;
    public float speed = 0.007f;
    public float rotSpeed = 0.2f;
    public float waypointAccuracy = 0.5f;
    NPCFollowPlayer chaseNav;
    public GameObject[] waypoints;

    private int currentWaypoint = 0;
    private string waypointState = "patrol";

    private bool playerInRange = false;
    private bool atkInProg = false;
    private int atkCounter = 0;
    private bool inChase = false;

    bool dead = false;

    AudioSource attackSound;
    AudioSource idleSound;
    AudioSource deathSound;


    // Use this for initialization
    void Start()
    {
        zombAnim = GetComponent<Animator>();
        chaseNav = gameObject.GetComponent<NPCFollowPlayer>();

        AudioSource[] audios = GetComponents<AudioSource>();
        idleSound = audios[0];
        attackSound = audios[1];
        deathSound = audios[2];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (dead == false)
        {
            CalcState();
        }
        else
        {
            zombAnim.SetTrigger("Dead");
            StartCoroutine(DeathDelay());
           // idleSound.Stop();
            //deathSound.Play();
        }

        if (atkInProg == true && playerInRange == true)
        {
            player.parent.gameObject.SendMessage("TakeDamage", damage);
            playerInRange = false;
            StartCoroutine(DelayAttack());
            attackSound.Play();
        }

    }

    void CalcState()
    {

        Vector3 direction = player.position - this.transform.position;
        float viewAngle = Vector3.Angle(direction, this.transform.forward);

        if (waypointState == "patrol" && waypoints.Length > 0)
        {
            if (idleSound.isPlaying != true)
            {
                idleSound.Play();
            }

            if (Vector3.Distance(waypoints[currentWaypoint].transform.position, transform.position) < waypointAccuracy)
            {
                currentWaypoint++;
                if (currentWaypoint >= waypoints.Length)
                {
                    currentWaypoint = 0;
                }
            }

            direction = waypoints[currentWaypoint].transform.position - transform.position;

            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);
            this.transform.Translate(0, 0, Time.deltaTime * speed);

        }

        if ((Vector3.Distance(player.position, this.transform.position) < 5 && viewAngle < 30) || (Vector3.Distance(player.position, this.transform.position) < 5 && inChase == true))
        {
            waypointState = "chasing";
            direction.y = 0;

            //this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);

            //skelAnim.SetBool("Idle", false);
            if (!dead)
            {
                if (direction.magnitude > 1.5f)
                {
                    //this.transform.Translate(0, 0, 0.007f);
                    chaseNav.myState = NPCFollowPlayer.NPC.Chasing;
                    inChase = true;
                    atkInProg = false;
                    zombAnim.SetBool("Walking", true);
                    zombAnim.SetBool("Attacking", false);

                    if (idleSound.isPlaying != true)
                    {
                        idleSound.Play();
                    }
                }
                else
                {
                    chaseNav.myState = NPCFollowPlayer.NPC.Idle;
                    inChase = false;
                    atkInProg = true;
                    zombAnim.SetBool("Walking", false);
                    zombAnim.SetBool("Attacking", true);
                    Debug.Log("Attacking");
                }
            }
            else
            {
                chaseNav.myState = NPCFollowPlayer.NPC.Idle;
                playerInRange = false;
            }
        }
        else if (Vector3.Distance(player.position, this.transform.position) < 2.2f)
        {
            direction.y = 0;
            waypointState = "chasing";

            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);

            //skelAnim.SetBool("Idle", false);
            if (!dead)
            {
                if (direction.magnitude > 1.5f)
                {
                    //this.transform.Translate(0, 0, 0.007f);
                    chaseNav.myState = NPCFollowPlayer.NPC.Chasing;
                    inChase = true;
                    zombAnim.SetBool("Walking", true);
                    zombAnim.SetBool("Attacking", false);
                    atkInProg = false;

                    if (idleSound.isPlaying != true)
                    {
                        idleSound.Play();
                    }
                }
                else
                {
                    chaseNav.myState = NPCFollowPlayer.NPC.Idle;
                    inChase = false;
                    zombAnim.SetBool("Walking", false);
                    zombAnim.SetBool("Attacking", true);
                    Debug.Log("Attacking");
                    atkInProg = true;
                    //StartCoroutine(DelayFirstSwing());
                }
            }
            else
            {
                chaseNav.myState = NPCFollowPlayer.NPC.Idle;
                playerInRange = false;
            }
        }
        else
        {
            zombAnim.SetBool("Walking", true);
            zombAnim.SetBool("Attacking", false);
            waypointState = "patrol";
            //skelAnim.SetBool("Idle", true);
            chaseNav.myState = NPCFollowPlayer.NPC.Idle;
            inChase = false;
            atkInProg = false;
        }

        //this.transform.Translate(0, 0, 0.009f);
        // skelAnim.SetBool("Walking", true);
    }

    void TakeDamage(float damageTaken)
    {
        Debug.Log("DamageRecieved" + " " + damageTaken);
        health -= damageTaken;

        if (health <= 0)
        {
            dead = true;
        }

        // skelAnim.SetTrigger("Damaged");

    }
    void OnTriggerEnter(Collider other)
    {
        if (!dead)
        {
            if (other.gameObject.tag == "Player")
            {
                playerInRange = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }

    IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(1.5f);
        playerInRange = true;
    }

    IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(this.gameObject);
    }

}
