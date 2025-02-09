using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackerScript : MonoBehaviour
{
    private float moveSpeed = 10;
    [SerializeField]
    private Transform firePoint; // Bullet spawn point
    private GameObject player;
    [SerializeField]
    private GameObject bulletPrefab; //bullet obj
    [SerializeField]
    private float attackRange = 15f; // Distance to ship before firing
    private float fireRate = 2.5f; // Wait time between shots
    private float nextFireTime = 0f; // When to fire next

    public AudioSource audioSource;
    public AudioClip shootAudio;
    
    private GameObject gameManager;
    private GameManagerScript gameManagerScript;

    void Start()
    {  
        player = GameObject.FindWithTag("Player"); // Find Player by tag

        gameManager = GameObject.FindWithTag("GameController"); // Find Player by tag
        gameManagerScript = gameManager.GetComponent<GameManagerScript>();
    }
    void Update()
    {
        if (gameManagerScript.playing)
        {
            if (player == null) return;

            RotateTowardsPlayer(); // Always face the ship

            // If within attack range, fire bullets
            if (Vector2.Distance(transform.position, player.transform.position) <= attackRange)
            {
                if (Time.time >= nextFireTime)
                {
                    Fire();
                    nextFireTime = Time.time + fireRate; // Set next fire time
                }
            }

            // Move towards ship
            MoveTowardsPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
    }

    void RotateTowardsPlayer()
    {
        Vector2 direction = player.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle - 90f); // Adjust for upward-facing sprite
    }

    void Fire()
    {
        if (bulletPrefab == null || firePoint == null) return;

        // Make firePoint look at the ship
        Vector2 direction = (player.transform.position - firePoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        firePoint.rotation = Quaternion.Euler(0, 0, angle - 90f); // Same correction as above

        // Spawn bullet
        audioSource.PlayOneShot(shootAudio, 0.5f);
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}