using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraScript : MonoBehaviour
{
    public Transform player; // player ref
    public float smoothSpeed = 5f; // smooth movement
    public Vector3 offset; // offset from player pos

    void Start()
    {
        //keep camera back from player
        offset.z = -2;
    }
    void LateUpdate()
    {
        //move towards players position at the end of every
        Vector3 targetPosition = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }

}
