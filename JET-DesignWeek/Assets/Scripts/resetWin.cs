using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class resetWin : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //Can reference the scene by name in quotes (string)
            //or by the index number (0, 1, 2, etc.)
            SceneManager.LoadScene("Game");
        }
    }
}
