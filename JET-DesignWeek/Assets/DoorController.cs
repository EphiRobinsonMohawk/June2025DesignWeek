using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Sprite closedSprite;
    public Animator doorAnim;
    public SpriteRenderer sR;

    public void OpenDoor()
    {
        StartCoroutine(PlayOpenAnimationThenClose());
    }

    private IEnumerator PlayOpenAnimationThenClose()
    {
        doorAnim.enabled = true;
        doorAnim.SetTrigger("Open");

        // Wait for the animation to finish
        yield return new WaitForSeconds(doorAnim.GetCurrentAnimatorStateInfo(0).length);

        // Disable animator so it doesn't override the sprite
        doorAnim.enabled = false;

        // Set sprite back to closed sprite
        sR.sprite = closedSprite;
    }
}