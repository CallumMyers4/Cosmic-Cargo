using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;

public class MainMenuManagerScript : MonoBehaviour
{
    [SerializeField]
    private string mainScene;
    [SerializeField]
    private GameObject mainPanel, controlPanel;

    //opens main level
    public void Play()
    {
        SceneManager.LoadScene(mainScene);
    }

    //opens control panel, closes menu
    public void OpenControls()
    {
        mainPanel.SetActive(false);
        controlPanel.SetActive(true);
    }

    //opens menu panel, closes controls
    public void OpenMenu()
    {
        mainPanel.SetActive(true);
        controlPanel.SetActive(false);
    }

    //exits
    public void Exit()
    {
        Application.Quit();
    }
}
