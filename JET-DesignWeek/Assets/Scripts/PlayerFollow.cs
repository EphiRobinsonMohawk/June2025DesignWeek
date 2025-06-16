using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    //Access the target. Uses Transform which every Unity object has
    private Transform target;

    //To control the specific placement of the camera.
    [SerializeField]    //Makes a private variable visible in the editor
    private Vector3 offset;

    //Float for smooooooooth factor
    public float smoothFactor = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        //Automatically find the target by tag
        //Find a GameObject with the tag "Player" and access its transform
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    //LateUpdate is called at the end of every frame after
    //all Updates have been called
    void LateUpdate()
    {
        //Temp variable. We are creating this Vector so we can "separate"
        //the z value of the location. Transform is always Vector3
        //Set the new position to the target's x and y position, but leave z alone
        Vector3 targetPos = new Vector3(target.position.x, target.position.y,
                        transform.position.z);

        //Now move positions to the target using Lerp.
        //Lerp means "linear interpolation" which is a smooth movement between two vectors.
        //Parameters are (starting vector, ending vector, time)
        //Here we are moving from current position to target position.
        //deltaTime is "change of time" or the time between frames.
        transform.position = Vector3.Lerp(transform.position, targetPos + offset,
                smoothFactor * Time.deltaTime);
    }
}
