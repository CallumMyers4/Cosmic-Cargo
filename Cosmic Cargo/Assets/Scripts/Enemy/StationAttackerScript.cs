using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationAttackerScript : MonoBehaviour
{
    [SerializeField]
    private GameObject ship;    // Reference to target (station)
    
    [SerializeField]
    private GameObject bulletPrefab; // Bullet object
    [SerializeField]
    private Transform firePoint; // Bullet spawn point

    private float moveSpeed = 6f; 
    private float attackRange = 15f; // Distance to ship before firing
    private float fireRate = 1.5f; // Wait time between shots
    private float nextFireTime = 0f; // When to fire next
    private GameObject gameManager;
    private GameManagerScript gameManagerScript;

    void Start()
    {
        gameManager = GameObject.FindWithTag("GameController"); // Find Player by tag
        gameManagerScript = gameManager.GetComponent<GameManagerScript>();
    }

    void Update()
    {
        if (gameManagerScript.playing)
        {
            if (ship == null) return;

            RotateTowardsShip(); // Always face the ship

            // If within attack range, fire bullets
            if (Vector2.Distance(transform.position, ship.transform.position) <= attackRange)
            {
                if (Time.time >= nextFireTime)
                {
                    Fire();
                    nextFireTime = Time.time + fireRate; // Set next fire time
                }
            }
            else
            {
                // Move towards ship
                MoveTowardsShip();
            }
        }
    }

    void MoveTowardsShip()
    {
        Vector2 direction = (ship.transform.position - transform.position).normalized;
        transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
    }

    void RotateTowardsShip()
    {
        Vector2 direction = ship.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle - 90f); // Adjust for upward-facing sprite
    }

    void Fire()
    {
        if (bulletPrefab == null || firePoint == null) return;

        // Make firePoint look at the ship
        Vector2 direction = (ship.transform.position - firePoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        firePoint.rotation = Quaternion.Euler(0, 0, angle - 90f); // Same correction as above

        // Spawn bullet
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}