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
    public GameObject leftBorder, rightBorder, bottomBorder, topBorder;    //borders of game world
    private float minX, maxX, minY, maxY;   //boundaries of spawning
    public GameObject partsPrefab, shipAttackerPrefab, playerAttackerPrefab; //prefabs for the collectables and enemies
    private float spawnInterval = 2f; // time in seconds between enemy spawns
    [SerializeField]
    private PlayerMovementScript player;  //player ref
    [SerializeField]
    public UnityEngine.UI.Image healthBar, enemyBar, dashBar; //ref to healthbar, bar displaying enemies in danger zone, and dash cooldown bar
    [SerializeField]
    private GameObject HUDPanel, pausePanel; //ref to main HUD and pause menu
    // Start is called before the first frame update
    void Start()
    {       
        //set borders
        minX = leftBorder.transform.position.x + 5;
        maxX = rightBorder.transform.position.x - 5;
        minY = bottomBorder.transform.position.y + 10;
        maxY = topBorder.transform.position.y - 10;

        //spawn collectables
        for (int i = 0; i < partsPerRound; i++)
        {
            float randomX = Random.Range(minX, maxX);
            float randomY = Random.Range(minY, maxY);
            Vector2 collectableSpawn = new Vector2(randomX, randomY);

            Instantiate(partsPrefab, collectableSpawn, Quaternion.identity);
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
