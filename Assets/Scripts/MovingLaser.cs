using UnityEngine;

public class MovingLaser : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 0.15f;
    public float lifetime = 3f;
    private Vector2 direction;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime);
    }

    public void Launch(Vector2 dir)
    {
        direction = dir.normalized;
        if (rb != null)
            rb.linearVelocity = direction * speed;
    }

    void Update()
    {
        if (rb != null)
            rb.linearVelocity = direction * speed;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            col.GetComponent<NullController>()?.TakeDamage(damage);
            Debug.Log("Laser hit Justice! -0.15 CGPA");
            Destroy(gameObject);
        }

        // Don't destroy on hitting Whiplash
        if (col.CompareTag("Whiplash")) return;

        // Destroy on hitting walls/ground
        if (col.CompareTag("Ground")
            || col.gameObject.layer ==
            LayerMask.NameToLayer("Ground"))
            Destroy(gameObject);
    }
}