using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    public GameObject creditsPanel;

    public void PlayGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void OpenCredits()
    {
        creditsPanel.SetActive(true);
    }

    public void BackToMenu()
    {
        creditsPanel.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
