using UnityEngine;

public class MovingMouse : MonoBehaviour
{
    public Transform leftPoint;
    public Transform rightPoint;
    public float speed = 2f;

    private Rigidbody2D rb;
    private Vector3 targetDestination;

    private Transform passengerTransform;
    private bool hasPassenger = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        targetDestination = rightPoint.position;
    }

    void FixedUpdate()
    {
        if (leftPoint == null || rightPoint == null) return;

        Vector3 nextPosition = Vector3.MoveTowards(transform.position, targetDestination, speed * Time.fixedDeltaTime);
        Vector3 delta = nextPosition - transform.position;

        rb.MovePosition(nextPosition);

        // Directly shift the passenger's position by the same delta
        if (hasPassenger && passengerTransform != null)
        {
            Rigidbody2D passengerRb = passengerTransform.GetComponent<Rigidbody2D>();
            if (passengerRb != null)
            {
                // Teleport their rigidbody by the delta — bypasses velocity conflicts
                passengerRb.position += new Vector2(delta.x, 0f);
            }
        }

        if (Vector3.Distance(transform.position, targetDestination) < 0.1f)
        {
            targetDestination = (targetDestination == rightPoint.position)
                ? leftPoint.position
                : rightPoint.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                passengerTransform = collision.transform;
                hasPassenger = true;
                return;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        passengerTransform = null;
        hasPassenger = false;
    }
}