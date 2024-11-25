using System.Collections;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject projectilePrefab; // Prefab of the projectile
    public Transform shootPoint;       // Position where the projectile is spawned
    public float projectileSpeed = 10f; // Speed of the projectile

    void Update()
    {
        // Check if the player presses the fire button
        if (Input.GetButtonDown("Fire1")) // Default fire button is left mouse button or Ctrl
        {
            ShootProjectile();
        }
    }

    void ShootProjectile()
    {
        // Instantiate the projectile at the shoot point
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);

        // Add velocity to the projectile
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = shootPoint.forward * projectileSpeed;
        }

        // Destroy the projectile after a certain time to avoid clutter
        Destroy(projectile, 5f);
    }
}
