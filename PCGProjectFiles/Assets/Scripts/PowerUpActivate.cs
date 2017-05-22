using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpActivate : MonoBehaviour {
    Animator charAnim;
    CharacterBehaviour charMovementScript;

    public float healthPickupAmount = 50.0f;
    public float damageIncreaseAmount = 20.0f;

    void Awake()
    {
        charAnim = GetComponent<Animator>();
    }

    void Start () {

        charMovementScript = gameObject.GetComponent<CharacterBehaviour>();
        
	}
	
    void Powerup(int powerNum)
    {
        charAnim.SetTrigger("PowerUp");
        if (powerNum == 1)
        {
            charMovementScript.playerHealth += healthPickupAmount;
            if (charMovementScript.playerHealth > 100.0f)
            {
                charMovementScript.playerHealth = 100.0f;
            }
        }
        else if (powerNum == 2)
        {
            charMovementScript.playerShield = true;
            charMovementScript.shieldCount += 2;
            StartCoroutine(ShieldTimeout());
        }
        else
        {
            charMovementScript.playerDamage += damageIncreaseAmount;
        }

    }

    IEnumerator ShieldTimeout()
    {
        yield return new WaitForSeconds(20);
        charMovementScript.playerShield = false;
        charMovementScript.shieldCount = 0;
    }

    IEnumerator DamageTimeout()
    {
        yield return new WaitForSeconds(15);
        charMovementScript.playerDamage -= damageIncreaseAmount;
    }
}
