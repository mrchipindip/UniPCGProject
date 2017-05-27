using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkelController : MonoBehaviour {

    public Transform player;
    Animator skelAnim;
    public float health = 60.0f;
    public float damage = 10.0f;

    private bool atkInProg = false;
    private int atkCounter = 0;

    bool dead = false;

	// Use this for initialization
	void Start () {
        skelAnim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if(dead == false)
        {
            CalcState();
        }
        else
        {
            skelAnim.SetTrigger("Dead");
        }
        

    }

    void CalcState()
    {

        Vector3 direction = player.position - this.transform.position;
        float viewAngle = Vector3.Angle(direction, this.transform.forward);
        if (Vector3.Distance(player.position, this.transform.position) < 5 && viewAngle < 30)
        {

            direction.y = 0;

            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);

        skelAnim.SetBool("Idle", false);

        if (direction.magnitude > 1.5f)
        {
            this.transform.Translate(0, 0, 0.007f);
            skelAnim.SetBool("Walking", true);
            skelAnim.SetBool("Attacking", false);
        }
        else
        {
            skelAnim.SetBool("Walking", false);
            skelAnim.SetBool("Attacking", true);
        }
    }
        else if (Vector3.Distance(player.position, this.transform.position) < 2.2f)
        {
            direction.y = 0;

            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);

            skelAnim.SetBool("Idle", false);
            if (direction.magnitude > 1.5f)
            {
                this.transform.Translate(0, 0, 0.007f);
                skelAnim.SetBool("Walking", true);
                skelAnim.SetBool("Attacking", false);
            }
            else
            {
                skelAnim.SetBool("Walking", false);
                skelAnim.SetBool("Attacking", true);
                atkInProg = true;
                StartCoroutine(DelayFirstSwing());
            }
        }
        else
        {
            skelAnim.SetBool("Walking", false);
            skelAnim.SetBool("Attacking", false);
            skelAnim.SetBool("Idle", true);
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

    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(atkInProg != true)
            {
                Debug.Log("Hit");
                atkInProg = true;
                other.gameObject.SendMessage("TakeDamage", damage);
                StartCoroutine(WaitTime());
            }
            
        }
    }

    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(2.5f);
        atkInProg = false;
    }

    IEnumerator DelayFirstSwing()
    {
        yield return new WaitForSeconds(1.0f);
        atkInProg = false;
    }

}
