using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrainPad : MonoBehaviour
{
    public float drainingSpeed;
    public float drained;
    public bool isDrained;

    public GameObject door;
    public PlayerController playerController;

    void Update()
    {
        if (drained >= 50)
        {
            isDrained = true;
            door.GetComponent<BoxCollider2D>().enabled = false;
        }
        if (playerController.isDraining == true)
        {
            drained += drainingSpeed * Time.deltaTime;
        }
    }
}
