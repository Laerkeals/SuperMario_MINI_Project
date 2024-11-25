using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FireballAnimation : MonoBehaviour
{
    public float rotationSpeed = 200f;  // Speed of the fireball's rotation
    public float bounceHeight = 0.5f;  // Height of the bounce effect
    public float bounceSpeed = 2f;     // Speed of the bounce

    private Vector3 initialPosition;

    void Start()
    {
        // Save the initial position to calculate the bounce effect
        initialPosition = transform.position;
    }

    void Update()
    {
        // Rotate the fireball around its Y-axis
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);

        // Create a bounce effect
        float bounce = Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;
        transform.position = new Vector3(transform.position.x, initialPosition.y + bounce, transform.position.z);
    }
}
