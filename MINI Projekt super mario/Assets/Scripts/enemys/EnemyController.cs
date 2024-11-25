using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform start;
    public Transform stop;
    public Rigidbody rig;
    public Transform player; // Reference to the player
    public float speed = 2f;
    public float chaseSpeed = 4f;
    public float detectionRange = 6f;
    public float jumpForce = 5f; // Strength of the jump
    public float pauseDuration = 0.5f; // Pause time at each end of patrol path
    public float killHeightThreshold = 0.4f; // Height difference to kill the enemy

    private float timer;
    private bool isChasing = false;
    private bool isPaused = false; // Indicates if the enemy is currently pausing
    private bool movingToStop = true; // Determines patrol direction (start -> stop or stop -> start)
    private Quaternion targetRotation; // Stores target rotation

    private void Start()
    {
        timer = 0f;
        targetRotation = transform.rotation; // Initialize target rotation
    }

    private void FixedUpdate()
    {
        if (isChasing)
        {
            ChasePlayer();
        }
        else if (!isPaused)
        {
            Patrol();
        }

        DetectPlayer();
    }

    // Method for patrol movement with pausing and rotation
    private void Patrol()
    {
        // Move toward either start or stop point
        Transform target = movingToStop ? stop : start;
        Vector3 direction = (target.position - transform.position).normalized;
        rig.MovePosition(transform.position + direction * speed * Time.fixedDeltaTime);

        // Check if we reached the patrol end point
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            //print("rotating");
            StartCoroutine(PauseAndRotate());
            movingToStop = !movingToStop; // Reverse direction
        }
    }

    // Coroutine to handle pausing and 180-degree rotation
    private IEnumerator PauseAndRotate()
    {
        //print("is it called");
        isPaused = true;
        Quaternion targetRot = Quaternion.Euler(0, transform.eulerAngles.y + 180, 0);
        while (!(transform.rotation.eulerAngles.y <= targetRot.eulerAngles.y * 1.01f && transform.rotation.eulerAngles.y >= targetRot.eulerAngles.y * 0.99f))
        {
            yield return new WaitForFixedUpdate();

            // Rotate 180 degrees around the y-axis
            targetRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + 1, transform.eulerAngles.z);
            print(targetRotation);
            transform.rotation = targetRotation;
        }
        //undersøg at gøre brug ad rigedbodyen 
        //i fremtiden brug taske. wait 
        isPaused = false;
    }

    // Method to handle chasing the player
    private void ChasePlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        rig.MovePosition(transform.position + directionToPlayer * chaseSpeed * Time.fixedDeltaTime);

        // Rotate to face the player
        if (directionToPlayer != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 10f);
        }
    }

    // Method to detect if the player is within range and trigger a jump if the player is spotted
    private void DetectPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            if (!isChasing) // Jump only when initially spotting the player
            {
                Jump();
            }
            isChasing = true;
        }
        else
        {
            isChasing = false;
        }
    }

    // Method for the enemy to jump
    private void Jump()
    {
        if (rig != null)
        {
            rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    // Detect collision with player and check if the player is above the enemy
    private void OnCollisionEnter(Collision collision)
    {
        //print("Collider with SOMETHING");
        if (collision.gameObject.CompareTag("Player"))
        {
            //print("Collider with PLAYER");
            Vector3 collisionPoint = collision.contacts[0].point;

            // Check if player is above the enemy (using a small threshold)
            if (collisionPoint.y > transform.position.y + killHeightThreshold)
            {
                Kill(); // Kill the enemy if the player is above
            }
        }
    }

    // Method to handle enemy death
    public void Kill()
    {
        Destroy(gameObject);
    }
}
