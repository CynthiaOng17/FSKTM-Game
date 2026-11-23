using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    public float damage = 0.15f;
    public float lifetime = 0.5f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            CGPAManager.Instance?.TakeDamage(damage);
            Debug.Log("Laser hit! -0.15 CGPA");
        }
    }
}