using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpPickup : MonoBehaviour {

    private AudioSource soundToPlay;
    private bool playerWithinTrigger = false;
    private int activePower;
    public GameObject powerupHealth;
    public GameObject powerupShield;
    public GameObject powerupDamage;
    public GameObject player;

    // Use this for initialization
    void Start () {
        soundToPlay = gameObject.GetComponent<AudioSource>();
        RandPower();
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Joystick1Button3) && playerWithinTrigger == true)
        {
            player.SendMessage("Powerup", activePower);
            soundToPlay.Play();
            powerupHealth.SetActive(false);
            powerupShield.SetActive(false);
            powerupDamage.SetActive(false);
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerWithinTrigger = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerWithinTrigger = false;
        }
    }

    void RandPower()
    {
        activePower = Random.Range(1, 4);

        if (activePower == 1)
        {
            powerupHealth.SetActive(true);
        }
        else if (activePower == 2)
        {
            powerupShield.SetActive(true);
        }
        else
        {
            powerupDamage.SetActive(true);
        }

    }

}
