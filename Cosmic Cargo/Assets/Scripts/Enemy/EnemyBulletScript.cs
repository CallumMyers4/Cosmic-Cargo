using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    public float speed = 20f; //bullets speed
    public float lifetime = 2f; //time until destroy
    private GameObject player;
    private PlayerMovementScript playerScript;
    private void Start()
    {
        //destroy after lifetime (in secs)
        Destroy(gameObject, lifetime);

        player = GameObject.FindWithTag("Player"); // Find Player by tag
        playerScript = player.GetComponent<PlayerMovementScript>();
    }

    private void Update()
    {
        //move in direction of fire
        transform.position += transform.up * speed * Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            playerScript.health -= 5.0f;
            Destroy(gameObject);
        }
        else if (!collider.gameObject.CompareTag("Enemy") && !collider.gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
    }
}
