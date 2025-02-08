using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    private int partsPerRound = 10; //number of parts to spawn
    public float maxX, maxY, minX, minY;    //boundaries of game world
    public GameObject partsPrefab, enemyPrefab; //prefabs for the collectables and enemy
    [SerializeField]
    private PlayerMovementScript player;  //player ref
    [SerializeField]
    public UnityEngine.UI.Image healthBar, enemyBar, dashBar; //ref to healthbar, bar displaying enemies in danger zone, and dash cooldown bar
    [SerializeField]
    private GameObject HUDPanel, pausePanel; //ref to main HUD and pause menu
    // Start is called before the first frame update
    void Start()
    {
        //spawn in objects on game start
        for (int i = 0; i < partsPerRound; i++)
        {
            //instantiate between boundaries, avoid space station and player
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && dashBar != null)
        {
            dashBar.fillAmount = player.GetDashCooldown();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (HUDPanel.activeSelf)
            {
                OpenPause();
            }
            else
            {
                ClosePause();
            }
        }
    }

    //opens pause, closes HUD
    public void OpenPause()
    {
        pausePanel.SetActive(true);
        HUDPanel.SetActive(false);
    }

    //opens HUD, closes pause
    public void ClosePause()
    {
        HUDPanel.SetActive(true);
        pausePanel.SetActive(false);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
