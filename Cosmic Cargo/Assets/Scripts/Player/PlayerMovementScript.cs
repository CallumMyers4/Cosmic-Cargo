using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    private float moveSpeed = 8f;   //players move speed
    private Rigidbody2D rb;  //rigidbody ref
    public Camera playerCam, stationCam;  //reference to player's cam and station's cam

    private Vector2 movement;   //store inputs
    private Vector2 mousePos;   //mouse pos in world space

    void Start()
    {
        //fill rb ref
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        //get input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        
        //get mouse pos (in world)
        mousePos = playerCam.ScreenToWorldPoint(Input.mousePosition);

        //switch between cams when holding tab
        if (Input.GetKey(KeyCode.Tab))
        {
            playerCam.enabled = false;
            stationCam.enabled = true;
        }
        else
        {
            stationCam.enabled = false;
            playerCam.enabled = true;
        }
    }

    void FixedUpdate()
    {
        //move player
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        
        //get direction towards mouse
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }
}
