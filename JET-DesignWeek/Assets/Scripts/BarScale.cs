using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarScale : MonoBehaviour
{
    // Start is called before the first frame update
    public float maxValue = 100f;
    public float currentValue = 1f; // Change this dynamically
    public DrainPad drainPad;
    private Vector3 originalScale;
    [Tooltip("Which # door is this?")]
    public int doorNumber;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        currentValue = (drainPad.drained - ((doorNumber-1) * 50));
        float normalized = Mathf.Clamp01(currentValue / maxValue);
        transform.localScale = new Vector3(originalScale.x * normalized, originalScale.y, originalScale.z);
    }
}
