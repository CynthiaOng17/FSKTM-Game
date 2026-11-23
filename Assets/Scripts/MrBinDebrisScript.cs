using UnityEngine;

public class DebrisScript : MonoBehaviour
{
    public float minDamage = 0.10f;
    public float maxDamage = 0.25f;
    public float autoDestroyTime = 4f;

    private float damageAmount; 

    void Start()
    {
        damageAmount = Random.Range(minDamage, maxDamage); 
        Destroy(gameObject, autoDestroyTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Debris hit: " + collision.gameObject.name + " | Tag: " + collision.tag);

        if (collision.CompareTag("Player"))
        {
            // 🛡️ 1. Get the NullController script component from the player object that was hit
            NullController player = collision.gameObject.GetComponent<NullController>();

            if (player != null)
            {
                // 🛡️ 2. Call TakeDamage on Null directly so the Firewall can negate the hit!
                player.TakeDamage(damageAmount);
                Debug.Log("Debris applied damage check to Null's controller: " + damageAmount);
            }
            else
            {
                // Fallback safety layer if your player tag is correct but component is missing
                if (CGPAManager.Instance != null)
                {
                    CGPAManager.Instance.TakeDamage(damageAmount);
                }
            }
            
            // Destroy the projectile object after dealing damage
            Destroy(gameObject);
        }

        if (collision.CompareTag("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }
    }
}