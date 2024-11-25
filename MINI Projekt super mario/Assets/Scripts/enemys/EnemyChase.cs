using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyChase : MonoBehaviour
{
    public Transform player;           // Reference to the player's transform
    public float sightRange = 10f;     // How far the enemy can see the player
    public float speed = 3f;           // Movement speed of the enemy
    public int maxHealth = 5;          // The number of hits the enemy can take before dying
    public float wanderRadius = 5f;    // Radius for random wandering
    public float wanderCooldown = 2f; // Time between choosing new random positions

    private int currentHealth;         // The enemy's current health
    private Vector3 wanderTarget;      // Current target for random wandering
    private float wanderTimer;         // Timer to track when to pick a new position

    void Start()
    {
        // Initialize health
        currentHealth = maxHealth;

        // Initialize random wander target
        PickNewWanderTarget();
    }

    void Update()
    {
        // Check if the player is within sight range
        if (Vector3.Distance(transform.position, player.position) <= sightRange)
        {
            // Chase the player
            ChasePlayer();
        }
        else
        {
            // Wander randomly
            Wander();
        }
    }

    void ChasePlayer()
    {
        // Move towards the player
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Make the enemy face the player
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
    }

    void Wander()
    {
        // Move towards the current wander target
        Vector3 direction = (wanderTarget - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Check if the enemy is close to the wander target
        if (Vector3.Distance(transform.position, wanderTarget) < 0.5f || wanderTimer <= 0f)
        {
            PickNewWanderTarget();
        }

        // Update the wander timer
        wanderTimer -= Time.deltaTime;
    }

    void PickNewWanderTarget()
    {
        // Pick a random position within the wander radius
        Vector2 randomCircle = Random.insideUnitCircle * wanderRadius;
        wanderTarget = new Vector3(transform.position.x + randomCircle.x, transform.position.y, transform.position.z + randomCircle.y);

        // Reset the wander timer
        wanderTimer = wanderCooldown;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to a projectile
        if (other.CompareTag("Projectile"))
        {
            // Destroy the projectile
            Destroy(other.gameObject);

            // Enemy takes damage
            TakeDamage();
        }

        // Check if the collider belongs to the player
        if (other.CompareTag("Player"))
        {
            // Optional: Enemy could take damage or interact differently with the player
            TakeDamage();
        }
    }

    public void TakeDamage()
    {
        // Reduce health when hit
        currentHealth--;

        // Check if the enemy is dead
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Destroy the enemy GameObject
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        // Visualize the sight range and wander radius in the Scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
    }
}
