using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEditor;
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
    private float spawnInterval = 5f; // time in seconds between enemy spawns
    [SerializeField]
    private PlayerMovementScript player;  //player ref
    [SerializeField]
    public UnityEngine.UI.Image healthBar, enemyBar, dashBar, partsBar; //ref to healthbar, bar displaying enemies in danger zone, dash cooldown bar and parts found
    [SerializeField]
    private GameObject HUDPanel, pausePanel; //ref to main HUD and pause menu
    [SerializeField]
    private GameObject loss1Panel, loss2Panel, winPanel;    //ref to lost by enemies, lost by lives and won panels
    public ZoneManagerScript zoneManager;
    public int stage = 1;
    public bool spawningEnemies = true, playing = true; //enemy spawning, game active
    public SpriteRenderer stationShip;  //refernce to ship on station
    public Sprite newShip;  //reference to stage 2 ship sprite

    // Start is called before the first frame update
    void Start()
    {       
        //set borders
        minX = leftBorder.transform.position.x + 5;
        maxX = rightBorder.transform.position.x - 5;
        minY = bottomBorder.transform.position.y + 10;
        maxY = topBorder.transform.position.y - 10;

        //spawn parts
        for (int i = 0; i < partsPerRound; i++)
        {
            Vector2 collectableSpawn;

            while (true)
            {
                float randomX = Random.Range(minX + 1f, maxX - 1f);
                float randomY = Random.Range(minY + 1f, maxY - 1f);
                collectableSpawn = new Vector2(randomX, randomY);

                // Check if the position is inside a collider (e.g., obstacles, walls)
                Collider2D hitCollider = Physics2D.OverlapBox(collectableSpawn, new Vector2(1, 1), 0);

                if (hitCollider == null) // Ensure it's not inside another collider
                {
                    break; // Valid spawn position found
                }
            }

            Instantiate(partsPrefab, collectableSpawn, Quaternion.identity);
        }


        //start spawning enemies
        StartCoroutine(SpawnEnemy());
    }

    // Update is called once per frame
    void Update()
    {
        if (playing)
        {
            if (player != null && dashBar != null)
            {
                dashBar.fillAmount = player.GetDashCooldown();
            }

            if (player != null && partsBar != null)
            {
                partsBar.fillAmount = player.GetPartsCollected();
            }

            if (player != null && healthBar != null)
            {
                healthBar.fillAmount = player.GetCurrentHealth();
            }

            if (player != null && enemyBar != null)
            {
                enemyBar.fillAmount = zoneManager.GetShipsInZonePercent();
            }

            if (player.partsCollected == 10 && stage == 1)
            {
                //spawn next round collectables
                for (int i = 0; i < partsPerRound; i++)
                {
                    float randomX = Random.Range(minX, maxX);
                    float randomY = Random.Range(minY, maxY);
                    Vector2 collectableSpawn = new Vector2(randomX, randomY);
                    Instantiate(partsPrefab, collectableSpawn, Quaternion.identity);

                    player.partsCollected = 0;
                    spawnInterval = 2.5f;
                }
                stationShip.sprite = newShip;
                stage = 2;
            }
            else if (player.partsCollected == 10 && stage == 2)
            {
                Win();
            }

            if (zoneManager.GetShipsInZone() >= 10)
            {
                spawningEnemies = false;
                LoseControl();
            }

            if (player.health <= 0)
            {
                spawningEnemies = false;
                LoseLife();
            }
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

    public void PlayAgain()
    {
        SceneManager.LoadScene("MainLevelScene");
    }

//spawn enemies
private IEnumerator SpawnEnemy()
{
    while (spawningEnemies) //spawning enemies until the game ends
    {
        yield return new WaitForSeconds(spawnInterval);

        Vector2 enemySpawn;

        while (true) //keep trying until a valid spawn position is found
        {
            float randomX = Random.Range(minX + 1f, maxX - 1f); // Stay inside boundary
            float randomY = Random.Range(minY + 1f, maxY - 1f);
            enemySpawn = new Vector2(randomX, randomY);

            //check if the spawn position overlaps a collider (e.g., obstacles, walls)
            Collider2D hitCollider = Physics2D.OverlapBox(enemySpawn, new Vector2(1, 1), 0);

            if (hitCollider == null) //not inside another collider
            {
                break; //spawn position found
            }
        }

        // select which enemy to spawn
        GameObject enemyPrefab;
        float randomValue = Random.value;

        if (randomValue < 0.4f) // 40% chance
        {
            enemyPrefab = shipAttackerPrefab;
        }
        else // 60% chance
        {
            enemyPrefab = playerAttackerPrefab;
        }

        Instantiate(enemyPrefab, enemySpawn, Quaternion.identity);
    }
}

    public void Win()
    {
        winPanel.SetActive(true);
        HUDPanel.SetActive(false);
        Debug.Log("Won!");
    }

    private void LoseLife()
    {
        loss2Panel.SetActive(true);
        HUDPanel.SetActive(false);
        Debug.Log("Lose");
    }

    private void LoseControl()
    {
        loss1Panel.SetActive(true);
        HUDPanel.SetActive(false);
        Debug.Log("Too many enemies");
    }
}
