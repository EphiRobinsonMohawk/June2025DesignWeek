using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrainPad : MonoBehaviour
{
    public float drainingSpeed;
    public float drained;
    public float drainCap;
    public bool isDrained;
    private bool hasPlayed;

    public GameObject door;
    public PlayerController playerController;
    public AudioSource drAudioSource;
    public AudioClip doorOpen;
    public Animator anim;

    void Start()
    {
        hasPlayed = false;   
    }
    void Update()
    {
        if (drained >= drainCap)
        {
            isDrained = true;
            door.GetComponent<BoxCollider2D>().enabled = false;
            if (!hasPlayed)
            {

                door.GetComponent<DoorController>().OpenDoor();
                drAudioSource.PlayOneShot(doorOpen);
                hasPlayed = true;
            }
            //Here I want to make my door object play the animation to open the door, then switch to sprite
            Debug.Log(door.name + "Opened!");
        }
        if (playerController.isDraining == true)
        {
            drained += drainingSpeed * Time.deltaTime;
        }
    }
}
