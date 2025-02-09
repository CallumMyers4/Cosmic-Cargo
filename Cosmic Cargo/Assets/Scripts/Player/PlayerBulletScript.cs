using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletScript : MonoBehaviour
{
    public float speed = 20f; //bullets speed
    public float lifetime = 2f; //time until destroy

    private void Start()
    {
        //destroy after lifetime (in secs)
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        //move in direction of fire
        transform.position += transform.up * speed * Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.CompareTag("Enemy"))
        {
            Destroy(collider.gameObject);
            Destroy(gameObject);
        }
        else if (!collider.gameObject.CompareTag("Player") && !collider.gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
    }
}
