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
    public AudioSource dAudioSource;
    public AudioClip doorOpen;

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
                dAudioSource.PlayOneShot(doorOpen);
                hasPlayed = true;
            }
            Debug.Log(door.name + "Opened!");
        }
        if (playerController.isDraining == true)
        {
            drained += drainingSpeed * Time.deltaTime;
        }
    }
}
