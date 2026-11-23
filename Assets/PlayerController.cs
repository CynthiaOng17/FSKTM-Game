using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 8f;
    public float jumpForce = 12f;
    
    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        // Link this script to the Rigidbody 2D component on the player
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 1. Left/Right Movement
        // GetAxisRaw detects A/D keys or Left/Right arrow keys
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        // 2. Jumping
        // "Jump" is mapped to the Spacebar by default in Unity
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false; // Player is now in the air
        }
    }

    // This detects when the player's collider touches a surface tagged "Ground"
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}