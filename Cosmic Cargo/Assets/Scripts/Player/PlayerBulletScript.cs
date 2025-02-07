using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletScript : MonoBehaviour
{
    public float speed = 10f; //bullets speed
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
}
