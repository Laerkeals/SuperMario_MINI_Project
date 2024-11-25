using System.Collections;
using UnityEngine;

public class AbilityToShoot : MonoBehaviour
{
    public float abilityDuration = 5.0f; // Duration of the shooting ability
    private Shooter shooter;             // Reference to the Shooter script

    void Start()
    {
        // Get the reference to the Shooter component
        shooter = GetComponent<Shooter>();

        // Ensure the player cannot shoot initially
        if (shooter != null)
        {
            shooter.enabled = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the player collides with a collectible
        if (other.gameObject.CompareTag("CollectibleShooter"))
        {
            // Destroy the collectible
            Destroy(other.gameObject);

            // Enable the shooting ability
            if (shooter != null && !shooter.enabled)
            {
                StartCoroutine(EnableShooting());
            }
        }
    }

    IEnumerator EnableShooting()
    {
        // Enable the shooting script
        if (shooter != null)
        {
            shooter.enabled = true;
        }

        // Wait for the ability duration
        yield return new WaitForSeconds(abilityDuration);

        // Disable the shooting script
        if (shooter != null)
        {
            shooter.enabled = false;
        }
    }
}
