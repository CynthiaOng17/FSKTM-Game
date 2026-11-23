using UnityEngine;

public class CollectibleMedicine : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object colliding with the medicine is the Player
        if (collision.CompareTag("Player"))
        {
            // Tell the LevelManager that a medicine was picked up
            FOMSpecialLevelManager.Instance.CollectMedicine();

            // Destroy this medicine object so it disappears from the screen
            Destroy(gameObject);
        }
    }
}