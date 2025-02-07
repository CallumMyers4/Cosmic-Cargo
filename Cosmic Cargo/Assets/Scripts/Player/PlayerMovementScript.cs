using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    private float moveSpeed = 8.0f;   //players move speed
    private Rigidbody2D rb;  //rigidbody ref
    public Camera playerCam, stationCam;  //reference to player's cam and station's cam

    private Vector2 movement;   //store inputs
    private Vector2 mousePos;   //mouse pos in world space
    
    //check if currently dashing, how long dash should last, how long is left before timer runs out, when dash was last used and
    //how quick to move whilst dashing and how long between allowing dash
    private bool isDashing; private float dashTime = 5.0f, dashTimeLeft = 0.0f, lastDashTime = 0.0f, dashSpeed = 12.0f, dashCooldown = 10.0f;


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

        //check if player can/wants to dash
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= lastDashTime + dashCooldown)
        {
            StartDash();
        }
    }

    void FixedUpdate()
    {
        //move player depending on whether or not dash is currently enabled
        if (isDashing && playerCam.enabled)
        {
            Dash();
        }
        else if (playerCam.enabled)
        {
            Move();
        }
        
        //look towards mouse pos
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }

    //move at normal speed
    void Move()
    {
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    //begin dashing
    void StartDash()
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDashTime = Time.time;
    }

    //move at dash speed
    void Dash()
    {
        if (dashTimeLeft > 0)
        {
            rb.velocity = movement.normalized * dashSpeed;
            dashTimeLeft -= Time.fixedDeltaTime;
        }
        else
        {
            isDashing = false;
            rb.velocity = Vector2.zero; // Stop dash
        }
    }
}
