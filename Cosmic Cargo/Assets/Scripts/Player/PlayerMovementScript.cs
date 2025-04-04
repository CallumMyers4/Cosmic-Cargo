using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    private float moveSpeed = 12.0f;   //players move speed
    private Rigidbody2D rb;  //rigidbody ref
    public Camera playerCam, stationCam;  //reference to player's cam and station's cam
    public GameObject exhaust, bullet;  //sprite for dashing, bullet prefab
    public int partsCollected = 0;
    public float health = 100.0f;
    public GameManagerScript gameManager;

    private Vector2 movement;   //store inputs
    private Vector2 mousePos;   //mouse pos in world space

    // Fire rate control
    private float fireRate = 0.2f; // Time between shots
    private float nextFireTime = 0f; // Time when player can fire next

    public AudioClip shootAudio, pickupAudio;   //audio for when shooting or collecting a part
    public AudioSource audioSource; //ref to player's audio player
    
    //check if currently dashing, how long dash should last, how long is left before timer runs out, when dash was last used and
    //how quick to move whilst dashing and how long between allowing dash
    private bool isDashing; private float dashTime = 5.0f, dashTimeLeft = 0.0f, lastDashTime = 0.0f, dashSpeed = 20.0f, dashCooldown = 10.0f;

    void Start()
    {
        //fill rb ref
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (gameManager.playing)
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

            //fire bullets if past cooldown
            if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
            {
                Fire();
                nextFireTime = Time.time + fireRate; // Set next allowed fire time
            }
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
        exhaust.SetActive(true);
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
            exhaust.SetActive(false);
            rb.velocity = Vector2.zero; //end dash
        }
    }

    //create a bullet
    void Fire()
    {
        //get the player's forward direction (facing the mouse)
        Vector2 fireDirection = transform.up; 

        //calculate bullet spawn position slightly in front of the player
        Vector3 bulletSpawn = transform.position + (Vector3)fireDirection * 2.3f; // Adjust the multiplier if needed

        //create
        Instantiate(bullet, bulletSpawn, transform.rotation);

        //play sound
        audioSource.PlayOneShot(shootAudio, 0.5f);
    }

    //return time since dash as a %
    public float GetDashCooldown()
    {
        float timeSinceLastDash = Time.time - lastDashTime;
        return Mathf.Clamp01(timeSinceLastDash / dashCooldown); // Normalized value (0 to 1)
    }

    //return parts collected (current stage)
    public float GetPartsCollected()
    {
        int maxParts = 10; //parts per stage
        return (float)partsCollected / maxParts; //return as %
    }

    //return health
    public float GetCurrentHealth()
    {
        float maxHealth = 100; //parts per stage
        return health / maxHealth; //return as %
    }

    //add when finding parts
    void OnTriggerEnter2D(Collider2D collider) 
    {
        if (collider.gameObject.CompareTag("Part"))
        {
            audioSource.PlayOneShot(pickupAudio, 0.5f);
            Destroy(collider.gameObject);
            partsCollected++;
        }
    }
}