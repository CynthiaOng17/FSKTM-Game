using System.Collections;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    // How long the player can stand on it before it vanishes
    public float timeBeforeVanishing = 0.8f; 
    // How long it stays invisible before coming back
    public float timeBeforeReappearing = 3.0f; 

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    void Start()
    {
        // Get the visual and physical parts of this object
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // This detects when the player lands on the platform
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Start a timer sequence (Coroutine)
            StartCoroutine(VanishSequence());
        }
    }

    // A special function that allows us to pause time
    IEnumerator VanishSequence()
    {
        // Wait for the player to stand on it for a moment
        yield return new WaitForSeconds(timeBeforeVanishing);

        // Turn off the visuals and the physics (make it disappear)
        spriteRenderer.enabled = false;
        boxCollider.enabled = false;

        // Wait while it is gone
        yield return new WaitForSeconds(timeBeforeReappearing);

        // Turn everything back on
        spriteRenderer.enabled = true;
        boxCollider.enabled = true;
    }
}