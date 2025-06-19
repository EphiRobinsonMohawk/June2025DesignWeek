using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Charge Variables")] //This adds a text header above a section
    [Tooltip("Charge Player Has:")]
    public float chargeMeter; //charge player has
    [Tooltip("Rate Of Charge Decay")]
    public float chargeDecay; //rate of charge decay
    [Tooltip("Charge Gained On Battery Pickup:")]
    public float chargeAmmount; //charge per battery
    [Tooltip("Charge Player Stats With:")]
    public float chargeStart; //
    //Chargepad Vars
    [Tooltip("Is Player On Charge Pad")]
    public bool isCharging;
    [Tooltip("How Fast Does Charge Pad Charge The Player")]
    public float chargeSpeed;
    //
    [Tooltip("Isd Player On Drain Pad")]
    public bool isDraining;
    [Tooltip("How Fast Does Drain Pad Drain")]
    public float drainSpeed;
    [Header("Player Things")]

    public bool powerOn = false;
    public float powerTimer;
    [Header("Duration of the powerup")]
    public float powerTimerDuration = 5;
    public int cogsCollected;

    public GameObject punchCircle;
    public float enemyKnockBack = 50;
    public float enemyDamage;
    public Slider changeSlider;


    [Header("Player Things")]
    public AudioSource pAudioSource;
    public AudioSource cAudioSource;
    public AudioSource dAudioSource;
    public AudioClip playerDie;
    public AudioClip playerJump;
    public AudioClip playerMove;
    public AudioClip playerKnockback;
    public AudioClip playerPunch;
    public AudioClip batteryPickUp;
    public AudioClip charging;
    public AudioClip draining;
    public AudioClip powerUp;
    public bool hasPlayed;

    //Declare variables for the Rigidbody2D, Animator and SpriteRenderer
    private Rigidbody2D rb2d;
    private Animator anim;
    private SpriteRenderer sr;
    //float = floating point number, i.e., it has decimals.
    public float moveSpeed;
    public float startMoveSpeed = 8f;
    //Jump stuff
    [Header("Jump Things")] //This adds a text header above a section
    public float jumpHeight;
    public float startJumpHeight = 8f;
    public float fallMultiplier = 5f;   //Scalar to increase fall speed
    public float lowJumpMultiplier = 1.5f;
    //Low jump multiplier for tapping jump vs holding jump
    //Coyote time AKA "ledge forgiveness"
    public float coyoteTimeMax = 0.25f;
    public float coyoteTime = 0;
    //Bool to check if the player is on the ground and a LayerMask
    public bool isGrounded = false;
    public LayerMask groundLayer;   //List of layers. There are 5 by default
    public Transform feet;  //Feet Transform.
    public float groundCheckRay = 0.25f;
    //Transform for starting location.
    private Vector3 startingLocation;
    public bool dead = false;
    // Start is called before the first frame update
    void Start()
    {
        //OUR STUFF
        chargeMeter = chargeStart;
        jumpHeight = startJumpHeight;
        moveSpeed = startMoveSpeed;

        //Initialize the variables
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr =   GetComponent<SpriteRenderer>();

        //Coyote time is reset by default on Start
        coyoteTime = coyoteTimeMax;

        //Starting location is set to where the Player is on Start.
        startingLocation = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Decrease charge when charge is above 0
        if (chargeMeter > 0)
        {
            chargeMeter -= chargeDecay * Time.deltaTime;
        }
        //Execute death code when charge is less than 1
        if (chargeMeter < 1)
        {
            dead = true;
            anim.Play("playerDeath");
            if (!hasPlayed)
            {
                pAudioSource.PlayOneShot(playerDie);
                hasPlayed = true;
            }
            
        }
        //Add charge when player is on charging pad
        if (isCharging)
        {
            chargeMeter += chargeSpeed * Time.deltaTime;
        }
        if (isDraining)
        {
            chargeMeter -= drainSpeed * Time.deltaTime;

        }
        if (powerOn)
        {
            powerTimer += Time.deltaTime;
            jumpHeight = 13;
            moveSpeed = 13;
        }
        if (powerTimer > powerTimerDuration)
        {
            powerOn = false;
            powerTimer = 0;
            moveSpeed = startMoveSpeed;
            jumpHeight = startJumpHeight;
            Debug.Log("Powerup Expired!");
        }
        changeSlider.value = chargeMeter * 0.01f;

        if (chargeMeter > 100)
        {
            chargeMeter = 100;
        }


        //Local float variable for input
        //Unity has a built-in Input system with default "axes"
        //The axis system is at minimum -1, at maximum +1 and everything
        //in between. 0 means "off"

        float h = Input.GetAxis("Horizontal");

        //Call a custom function called "Movement" and
        //send it the value of h


            Movement(h);
            HandleJump();   //Custom for jump management

        //If the player clicks the left mouse button, play the attack animation
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!dead)
            {

                pAudioSource.PlayOneShot(playerPunch);
                anim.SetTrigger("attack");  //Triggers are "play once and then stop"
            }
        }

        //Scene switching demo
        if (Input.GetKeyDown(KeyCode.R))
        {
            //Can reference the scene by name in quotes (string)
            //or by the index number (0, 1, 2, etc.)
            SceneManager.LoadScene("Level2");
        }
    }

    void Movement(float hMovement)
    {
        //Take hMovement, which is the value from h
        //in Update and apply it to the rigidbody
        //We are replacing the x value of velocity
        //with hMovement * moveSpeed, and the y value is
        //whatever it currently is.
        if (!dead)
        {
            rb2d.velocity = new Vector2(hMovement * moveSpeed,
                rb2d.velocity.y);
        }
        else
        {
            rb2d.velocity = Vector2.zero;
        }
        //Modify the animator parameters based on
        //hMovement
        //anim.SetFloat will set the value of a float 
        //parameter. We are changing "walkSpeed" to be
        //whatever hMovement is
                        //Mathf.Abs means "absolute value"
                        //so it ignores the + or - sign!
        anim.SetFloat("walkSpeed", Mathf.Abs(hMovement));

        //SpriteRenderers have a parameter for flipX, which flips 
        //the sprite itself on the x axis.
        //If hMovement is < 0, flip to the left. If it's > 0, flip
        //to the right!
        if(hMovement < 0 && !dead)
        {
            sr.flipX = true;
            punchCircle.transform.rotation = Quaternion.Euler(0, 180, 0);
        } else if (hMovement > 0 && !dead)
        {
            sr.flipX = false;
            punchCircle.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    void HandleJump()
    {
        //Use raycast to check for the ground. Raycast is a collision "line"
        //that returns true or false (yes there was a collision, no there wasn't)
        //Parameters are (starting point, direction, size, layer mask)
        //Only the first two parameters are required; leaving the size blank
        //will make it infinitely long!
        isGrounded = Physics2D.Raycast(feet.position, Vector2.down, 
            groundCheckRay, groundLayer);

        //If the player is on the ground, reset coyote time
        if (isGrounded)
        {
            coyoteTime = coyoteTimeMax;
        } else
        {
            coyoteTime -= Time.deltaTime;   //If not on the ground, start 
            //counting down using deltaTime.
        }

        //If coyoteTime is still active, and the players hit the jump button
        if (coyoteTime > 0 && Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) && !dead)
        {
            pAudioSource.PlayOneShot(playerJump);
            //Add the jump value to the rigidbody2D velocity
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpHeight);

            //Alternate jump method is to use AddForce
            //rb2d.AddForce(Vector2.up * jumpHeight * 50, ForceMode2D.Impulse);
        }
        //Speed up the descent
        if (rb2d.velocity.y < 0)
        {
            //Add to the velocity by using gravity and our multiplier
            rb2d.velocity += Vector2.up * Physics2D.gravity
                * (fallMultiplier) * Time.deltaTime;
            //This is multipled by deltaTime to scale by the framerate
        } 
        //If the rigidbody is going upward and the spacebar is not being pressed
        //i.e., it was pressed and released
        else if (rb2d.velocity.y > 0 && !Input.GetKey(KeyCode.Space) && !Input.GetKey(KeyCode.W) && !Input.GetKeyDown(KeyCode.UpArrow))
        {
            rb2d.velocity += Vector2.up * Physics2D.gravity
                * (lowJumpMultiplier) * Time.deltaTime;
        }

        if (!dead)
        {
            //Set the animator parameter "isJumping"
            anim.SetBool("isJumping", !isGrounded);
        } else
        {
            anim.SetBool("isJumping", false);
        }
    }





    //Built-in function to draw debug elements such as lines, wire spheres and cubes
    private void OnDrawGizmos()
    {
        //This is to make the raycast visible, using the debug system
        if (isGrounded)
        {
            Gizmos.color = Color.green;
        } else
        {
            Gizmos.color = Color.white;
        }
        //This is not actually associated with our Raycast, but uses the same math
        Gizmos.DrawRay(feet.position, Vector2.down * groundCheckRay);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //"Pick up" the item
        if (collision.gameObject.tag == "Battery")
        {
            Destroy(collision.gameObject);
            AddCharge();
            pAudioSource.PlayOneShot(batteryPickUp);
            Debug.Log("Battery picked up, " + chargeAmmount + " charge added. You have "
                + chargeMeter + "charge!");
        }
        if (collision.gameObject.tag == "PowerUp")
        {

            pAudioSource.PlayOneShot(powerUp);
            Destroy(collision.gameObject);
            powerOn = true;
            Debug.Log("Powerup Activated!");
        }
        if (collision.gameObject.tag == "Cog")
        {
            Destroy(collision.gameObject);
            cogsCollected++;
            Debug.Log("Cog Collected, You Have " + cogsCollected + " Cogs");
        }

        if (collision.gameObject.tag == "ChargePad")
        {
            if (!cAudioSource.isPlaying)
            {
                cAudioSource.Play();
            }
            isCharging = true;
        }
        if (collision.gameObject.tag == "DrainPad")
        {
            if (!dAudioSource.isPlaying)
            {
                dAudioSource.Play();
            }
            isDraining = true;
        }

        //If the player collides with a gameObject with the tag platform
        if (collision.gameObject.tag == "Platform" /* && transform.parent == null*/)
        {
            //Child this object to the platform object
            transform.parent = collision.gameObject.transform;
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        //If the player collides with a gameObject with the tag platform
        if (collision.gameObject.tag == "Platform"/* && transform.parent != null*/) //j
        {
            //Remove the parent object
            transform.parent = null;
        }
        if (collision.gameObject.tag == "ChargePad")
        {
            if (cAudioSource.isPlaying)
            {
                cAudioSource.Stop();
            }
            isCharging = false;
        }
        if (collision.gameObject.tag == "DrainPad")
        {
            if (dAudioSource.isPlaying)
            {
                dAudioSource.Stop();
            }
            isDraining = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {

            float xDir = Mathf.Sign(transform.position.x - collision.transform.position.x);

            Vector2 knockbackDir = new Vector2(xDir, 0f);
            rb2d.velocity = Vector2.zero;

            pAudioSource.PlayOneShot(playerKnockback);
            rb2d.AddForce(knockbackDir * enemyKnockBack *100, ForceMode2D.Force);
            chargeMeter -= enemyDamage;
        }
    }

    //public functions are like public variables: they are visible in the editor
    //and can be accessed by other scripts.
    public void Death()
    {
        //Send player back to start.
        transform.position = startingLocation;
        dead = false;
        //Debug.Log will display custom messages in the Console
        Debug.Log("YOU DED");
        chargeMeter = 75;

        //This can also be used to display variables
        //Debug.Log(variableName);
        hasPlayed = false;
        SceneManager.LoadScene("Game");

    }

    public void AddCharge()
    {
    chargeMeter += chargeAmmount;
    }
}
