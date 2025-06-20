using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicMan : MonoBehaviour
{

    public static MusicMan instance;
    public AudioSource mAudioSource;
    public AudioClip mainTheme;
    public bool isPlaying;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates
        }
        isPlaying = true;
        mAudioSource.Play();

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            //Can reference the scene by name in quotes (string)
            //or by the index number (0, 1, 2, etc.)
            if (isPlaying)
            {

                mAudioSource.Stop();
                isPlaying = false;
            }
            else if (isPlaying == false)
            {
                mAudioSource.Play();
                isPlaying = true;
            }
        }
    }
}
