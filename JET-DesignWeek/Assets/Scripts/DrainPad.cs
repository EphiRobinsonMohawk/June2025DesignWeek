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
    private bool hasSaid;

    public GameObject door;
    public PlayerController playerController;
    public AudioSource drAudioSource;
    public AudioClip doorOpen;
    public Animator anim;

    void Start()
    {
        hasSaid = false;
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
            if (hasSaid == false)
            {
                Debug.Log(door.name + "Opened!");
                hasSaid = true;
            }
            //Here I want to make my door object play the animation to open the door, then switch to sprite
        }
        if (playerController.isDraining == true)
        {
            drained += drainingSpeed * Time.deltaTime;
        }
    }
}
