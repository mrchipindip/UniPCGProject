using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour {


    public GameObject helpText;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowHelp()
    {
        if(helpText.activeInHierarchy)
        {
            helpText.SetActive(false);
        }
        else
        {
            helpText.SetActive(true);
        }
    }

   public void PlayGame()
    {
        SceneManager.LoadScene("dungeon");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
