using UnityEngine;

public class LevelKey : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Find the NullController on the player character
            NullController player = collision.GetComponent<NullController>();
            
            if (player != null)
            {
                player.hasKey = true; // Give the key to Null
                Debug.Log("Key collected! The faculty door can now be opened.");
                
                Destroy(gameObject); // Clear key from map
            }
        }
    }
}