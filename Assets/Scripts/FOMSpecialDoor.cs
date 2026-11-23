using UnityEngine;

public class FOMSpecialDoor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ensure your Player object has the Tag "Player" set in the Inspector
        if (collision.CompareTag("Player"))
        {
            FOMSpecialLevelManager.Instance.CheckDoorTransition();
        }
    }
}