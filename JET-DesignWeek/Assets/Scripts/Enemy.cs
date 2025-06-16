using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Enemy variables are similar to a player!
    private Animator anim;
    public float speed = 2.5f;
    private float startingSpeed;
    private Rigidbody2D rb2d;
    public GameObject battery;

    //Edge detection
    public Transform eyeballs;
    public LayerMask groundLayer;
    public float groundCheckerLength = 5;
    public bool atEdge;

    public bool facingRight = true;

    //Variable to access the enum list
    public EnemyState currentState = EnemyState.Walking;

    //Transform to store where the player is.
    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        //Initialize the variables
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();

        //Set the starting speed to whatever the speed is at the outset
        startingSpeed = speed;

        //Initialize the playerTransform by finding the player gameObject
        playerTransform = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //Switch-case statement. Only one state can be true at a time.
        switch(currentState)    //The parameter is the thing we are checking
        {
            case EnemyState.Walking:
                Walking();
                break;

            case EnemyState.Idle:
                Idle();
                break;

            case EnemyState.Dying:
                Dying();
                break;
        }
    }

    private void Walking()
    {
        anim.Play("enemyWalk");     //Play the enemy walking animation

        //Move the enemy using the rigidbody2d. X value is the speed variable
        //y is whatever it currently is.
        rb2d.velocity = new Vector2(speed, rb2d.velocity.y);

        //Similar method to grounded/jumping on the player to check for the
        //edge of the platform
        atEdge = !Physics2D.Raycast(eyeballs.position, Vector2.down,
            groundCheckerLength, groundLayer);

        if (atEdge)
        {
            //turn around
            speed *= -1;

            facingRight = !facingRight; //Switch the facingRight boolean
        }

        //Flip the player using Quaternion.Euler
        if (facingRight)
        {
            //Rotate to the right
            transform.rotation = Quaternion.Euler(0, 0, 0);
        } else
        {
            //Rotate to the left on the y-axis.
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        //Check when the player is within range
        //If the player is within 0.5 units of the enemy, switch to the idle state
        if (Vector2.Distance(transform.position, playerTransform.position) < 1f)
        {
            currentState = EnemyState.Idle;
        }
    }

    private void Idle()
    {
        //Play the idle animation and set the vector's x to 0
        anim.Play("enemyIdle");
        rb2d.velocity = new Vector2(0, rb2d.velocity.y);

        //If the player is out of range, go back to walking
        if (Vector2.Distance(transform.position, playerTransform.position) > 1)
        {
            currentState = EnemyState.Walking;
        }
    }

    public void Dying()
    {
        anim.Play("enemyDeath");    //Play the death animation
    }

    //This gets triggered by the animation event.
    public void Dead()
    {
        Destroy(gameObject);
        Instantiate(battery, this.transform.position, Quaternion.identity);
    }

    //If you get smacked, die.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "PUNCH")
        {
            currentState = EnemyState.Dying;
        }
    }
}

//Enum is "enumerator" and is a list of things. Unity C# recognizes this as an index
//and not the word. This is a list of three enemy states. To Unity they are 0, 1, and 2
//Enums are their own class
public enum EnemyState
{
    Walking, Idle, Dying
}
