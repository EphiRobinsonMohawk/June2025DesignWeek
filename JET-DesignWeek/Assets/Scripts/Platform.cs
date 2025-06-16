using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    //Vector3 for the path
    public Vector3 pointA, pointB;

    //Platform movement speed
    public float speed = 3;

    //Track where the platform is as a separate variable.
    private Vector3 currentPos;

    // Start is called before the first frame update
    void Start()
    {
        //Set the currentPos to where the platform is at start
        currentPos = transform.position;

        //Optional - makes it so that pointA is always where the platform is on Start
        pointA = currentPos;
    }

    //FixedUpdate is like Update (once per frame) but at a set interval instead of
    //relying on the framerate. Could also use Time.deltaTime as a scalar.
    void FixedUpdate()
    {
        //Local float to track "time"
        //Mathf.PingPong acts like a "metronome" where you set the "tempo" (rate)
        //and the length/target
        float t = Mathf.PingPong(Time.time * speed, 1);

        //Set the currentPos to a Lerp (moving between two points) at the PingPong rate
        currentPos = Vector3.Lerp(pointA, pointB, t);

        //Set the platform's position to that calculation
        transform.position = currentPos;
    }
}
