using UnityEngine;
using System.Collections;

public class VanishingTile : MonoBehaviour
{
    public float invisibleTime = 2f;
    public float visibleTime = 2f;
    public float startDelay = 0f; // Use this to make tiles blink in a sequence!

    private SpriteRenderer sprite;
    private BoxCollider2D col;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
        StartCoroutine(BlinkRoutine());
    }

    IEnumerator BlinkRoutine()
    {
        yield return new WaitForSeconds(startDelay);

        while (true)
        {
            // Turn OFF
            sprite.enabled = false;
            col.enabled = false;
            yield return new WaitForSeconds(invisibleTime);

            // Turn ON
            sprite.enabled = true;
            col.enabled = true;
            yield return new WaitForSeconds(visibleTime);
        }
    }
}