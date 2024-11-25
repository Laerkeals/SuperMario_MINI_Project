using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowOnCollect : MonoBehaviour
{
    public float growMultiplier = 2.0f; // How much bigger the player will become
    public float growDuration = 5.0f;  // Duration of the growth effect in seconds
    public float animationDuration = 1.0f; // Time it takes to grow to the full size
    private Vector3 originalScale;     // To store the player's original scale
    private bool isGrown = false;      // To check if the player is currently grown

    void Start()
    {
        // Save the original scale of the player
        originalScale = transform.localScale;
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the player collides with a collectible item
        if (other.gameObject.CompareTag("Collectible"))
        {
            if (!isGrown)
            {
                StartCoroutine(Grow());
            }

            // Destroy the collectible item
            Destroy(other.gameObject);
        }
    }

    System.Collections.IEnumerator Grow()
    {
        isGrown = true;

        // Smoothly grow the player
        yield return StartCoroutine(SmoothScale(originalScale, originalScale * growMultiplier, animationDuration));

        // Wait for the duration of the growth effect
        yield return new WaitForSeconds(growDuration);

        // Smoothly shrink the player back to original size
        yield return StartCoroutine(SmoothScale(transform.localScale, originalScale, animationDuration));

        isGrown = false;
    }

    System.Collections.IEnumerator SmoothScale(Vector3 startScale, Vector3 endScale, float duration)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            // Interpolate between the start and end scale over time
            transform.localScale = Vector3.Lerp(startScale, endScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure the final scale is set to the exact target scale
        transform.localScale = endScale;
    }
}
