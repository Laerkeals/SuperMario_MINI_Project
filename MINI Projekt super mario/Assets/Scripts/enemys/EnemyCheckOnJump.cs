using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCheckOnJump : MonoBehaviour
{
    public float rayDistance = 1.5f;   // Adjust this distance to ensure it reaches the enemy
    public LayerMask enemyLayer;       // Set this to the enemy's layer in the Inspector

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Only check if the player is moving downwards (falling or landing)
        if (rb.velocity.y <= 0)
        {
            CheckForEnemyBelow();
        }
    }

    void CheckForEnemyBelow()
    {
        // Raycast from slightly above the player's feet to detect enemies below
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.down * 0.1f; // Start ray just below player's feet

        // Debug the ray to see if it reaches the enemy
        Debug.DrawRay(rayOrigin, Vector3.down * rayDistance, Color.red, 0.1f);

        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, rayDistance, enemyLayer))
        {
            // If the ray hits an enemy, attempt to kill it
            EnemyController enemy = hit.collider.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.Kill();

                // Apply bounce effect on player after hitting enemy
                rb.velocity = new Vector3(rb.velocity.x, 7f, rb.velocity.z); // Adjust the bounce height if needed
            }
        }
    }
}
