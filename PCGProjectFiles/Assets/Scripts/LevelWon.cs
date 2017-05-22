using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelWon : MonoBehaviour {

    private bool playerWithinTrigger = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        CheckForWin();
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

    void CheckForWin()
    {
        if ((Input.GetKeyDown(KeyCode.Joystick1Button3)) && playerWithinTrigger == true)
        {
            Debug.Log("Won Won WOn");
            SceneManager.LoadScene("MainMenu");
        }
    }

}
