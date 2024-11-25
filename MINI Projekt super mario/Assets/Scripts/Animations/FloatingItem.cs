using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingItem : MonoBehaviour
{
    public float floatAmplitude = 0.5f; // How high and low the item floats
    public float floatSpeed = 2.0f;     // Speed of the floating animation

    private Vector3 startPosition;

    void Start()
    {
        // Record the item's initial position
        startPosition = transform.position;
    }

    void Update()
    {
        // Calculate the new Y position using a sine wave
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;

        // Apply the new position to the item
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
