using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompletion : MonoBehaviour
{
    public GameObject winTextObject; // Reference to the win message (optional)

    // Call this method when the level is completed
    public void CompleteLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Check if it's not the last level
        if (currentSceneIndex < SceneManager.sceneCountInBuildSettings - 1)
        {
            // Load the next level
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
        else
        {
            // Player has completed the last level
            EndGame();
        }
    }

    void EndGame()
    {
        // Show win message if available
        if (winTextObject != null)
        {
            winTextObject.SetActive(true);
        }

        Debug.Log("Player has completed all levels.");         
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CompleteLevel();
        }
    }
}
