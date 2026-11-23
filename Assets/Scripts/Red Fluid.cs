using UnityEngine;
using UnityEngine.SceneManagement;

public class RedFluid : MonoBehaviour
{
    // This function detects when something enters the trigger zone
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that touched the fluid is the Player
        if (other.CompareTag("Player"))
        {
            RestartLevel();
        }
    }

    void RestartLevel()
    {
        // Gets the build index of the currently active scene and reloads it
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
