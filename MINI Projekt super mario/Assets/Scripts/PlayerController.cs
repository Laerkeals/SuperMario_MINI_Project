using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;               // Movement speed (same for all directions)
    public float jumpForce = 7f;          // Jump force
    public float rotationSpeed = 100f;    // Rotation speed for key input (A/D)
    public float mouseSensitivity = 2f;   // Sensitivity for mouse rotation

    private Rigidbody rb;
    private bool isGrounded;
    private float rotationY = 0f;         // Tracks cumulative rotation for smooth blending

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevent Rigidbody from interfering with rotation
    }

    void Update()
    {
        // Handle forward/backward movement (W/S)
        float moveVertical = Input.GetAxis("Vertical"); // Forward/Backward
        Vector3 forwardMovement = transform.forward * moveVertical;

        // Handle strafing (A/D) at the same speed as forward/backward
        float moveHorizontal = Input.GetAxis("Horizontal"); // Left/Right
        Vector3 horizontalMovement = transform.right * moveHorizontal;

        // Combine forward and horizontal movement
        Vector3 movement = (forwardMovement + horizontalMovement).normalized * speed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);

        // Handle rotation by mouse
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        rotationY += mouseX;

        // Apply rotation
        transform.rotation = Quaternion.Euler(0, rotationY, 0);

        // Jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void OnCollisionStay()
    {
        isGrounded = true;
    }

    void OnCollisionExit()
    {
        isGrounded = false;
    }
}
