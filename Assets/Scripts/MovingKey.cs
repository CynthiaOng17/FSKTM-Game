using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("How far up and down the platform will travel from its starting spot.")]
    public float verticalDistance = 3f;
    
    [Tooltip("How fast the platform completes an up-and-down cycle.")]
    public float moveSpeed = 2f;

    private Vector3 startPosition;

    void Start()
    {
        // Save the exact spot where you placed the tile in the scene
        startPosition = transform.position;
    }

    // Change "void Update()" to "void FixedUpdate()"
    void FixedUpdate()
    {
        // Calculate the smooth up/down offset using a Sine wave
        float movementFactor = Mathf.Sin(Time.time * moveSpeed);
        
        float newY = startPosition.y + (movementFactor * verticalDistance);

        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }

    // 🤝 FIXES PLAYER SLIDING OFF: Sticks the player to the platform when riding it
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Make the player a child of the platform so they move together
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Unparent the player when they jump or walk off
            collision.transform.SetParent(null);
        }
    }
}