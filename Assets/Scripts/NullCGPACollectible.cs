using UnityEngine;

public class CGPACollectible : MonoBehaviour
{
    [Header("CGPA Settings")]
    [Tooltip("How much this item increases the CGPA pointer when collected.")]
    public float cgpaReward = 0.10f; 

    // 🔽 NEW OBJECTIVE SETTINGS
    [Header("Level Objective Settings")]
    [Tooltip("Check this if this specific collectible counts as an essential Dean's List fragment required to clear the level.")]
    public bool isDeanListPiece = false;

    [Header("Visual Effects (Optional)")]
    [Tooltip("Optional prefab for a particle effect when picked up.")]
    public GameObject collectEffectPrefab; 
    
    [Tooltip("How far down to push the +10 visual text indicator effect")]
    public float verticalSpawnOffset = 8.0f; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object colliding with the item is tagged as the Player
        if (collision.CompareTag("Player"))
        {
            CollectItem();
        }
    }

    private void CollectItem()
    {
        // 1. Locate the active CGPAManager instance in the scene and grant score points
        if (CGPAManager.Instance != null)
        {
            CGPAManager.Instance.GainPoints(cgpaReward);
        }
        else
        {
            Debug.LogWarning("CGPAManager instance not found in the scene! Make sure it is initialized.");
        }

        // 2. 🔽 NEW: Updates the Victory Condition Tracker
        // Alerts NullWinManager or WinManager that an objective fragment has been cleared
        if (NullWinManager.Instance != null)
        {
            NullWinManager.Instance.FragmentCollected();
        }
        else if (WinManager.Instance != null)
        {
            WinManager.Instance.FragmentCollected();
        }

        // 3. 🔽 NEW: Logs Special Fragments
        // Prints a celebratory system message if this item is marked as a major goal item
        if (isDeanListPiece)
        {
            Debug.Log("Dean List fragment collected!");
        }

        // 4. Spawn a visual burst text effect if one is assigned
        if (collectEffectPrefab != null)
        {
            Vector3 loweredPosition = new Vector3(
                transform.position.x, 
                transform.position.y - verticalSpawnOffset, 
                transform.position.z
            );

            Instantiate(collectEffectPrefab, loweredPosition, Quaternion.identity);
        }

        // 5. Destroy the collectible item so it can only be picked up once
        Destroy(gameObject);
    }
}