using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public GameObject pauseButtons;
    public Button resumeButton;
    public Button mainMenuButton;


	// Use this for initialization
	void Start () {
        resumeButton = resumeButton.GetComponent<Button>();
        mainMenuButton = mainMenuButton.GetComponent<Button>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("escape"))
        {
            Time.timeScale = 0;
            pauseButtons.SetActive(true);
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Resume()
    {
        Debug.Log("CalledRsume");
        Time.timeScale = 1;
        pauseButtons.SetActive(false);
    }
}
