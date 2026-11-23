using UnityEngine;
using System.Collections;

public class FacultyDoor : MonoBehaviour
{
    [Header("Opening Animation settings")]
    public float slideDistance = 3.5f; // How high up the door slides to clear the path
    public float slideSpeed = 4f;       // How fast it moves upward

    private BoxCollider2D doorCollider;
    private bool isUnlocked = false;

    void Start()
    {
        doorCollider = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isUnlocked) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            // Check if Null has the key item
            NullController player = collision.gameObject.GetComponent<NullController>();

            if (player != null && player.hasKey)
            {
                // 🔑 TAKE THE KEY AWAY FROM NULL RIGHT HERE!
                player.hasKey = false; 

                // Start the smooth moving animation
                StartCoroutine(AnimateOpenDoor()); 
            }
            else
            {
                Debug.Log("The faculty door is locked! Go find the key.");
            }
        }
    }

    private IEnumerator AnimateOpenDoor()
    {
        isUnlocked = true;
        Debug.Log("Door unlocked! Sliding open...");

        // 1. Instantly turn off the wall collision so Null can walk through right away
        if (doorCollider != null)
        {
            doorCollider.enabled = false;
        }

        // 2. Smoothly slide the door sprite straight up into the ceiling
        Vector3 targetPosition = transform.position + Vector3.up * slideDistance;
        
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, slideSpeed * Time.deltaTime);
            yield return null; // Wait for the next frame
        }

        // 3. Clean up the object once it's fully hidden out of sight
        Destroy(gameObject);
    }
}