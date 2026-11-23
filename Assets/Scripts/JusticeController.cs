using UnityEngine;
//using UnityEngine.SceneManagement;

public class JusticeController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 4f;
    public float jumpForce = 10f;

    [Header("Inventory")]
    public bool hasKey = false; 
    public bool hasHammer = false; 

    [Header("Ground Check")]
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator anim;
    private bool isGrounded;
    private bool facingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        if (groundCheck == null)
        {
            Debug.LogError("Please assign a GroundCheck object to Justice in the Inspector ya!");
        }
    }

    void Update()
    {
        // Safety check to avoid the "NullReference" error in your console
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        }

        float h = Input.GetAxisRaw("Horizontal");
        
        rb.linearVelocity = new Vector2(h * moveSpeed, rb.linearVelocity.y);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // When jumping, we clear the parent so Justice moves independently again
            transform.SetParent(null); 
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        if (h > 0 && !facingRight) Flip();
        if (h < 0 && facingRight) Flip();

        if (anim != null)
        {
            anim.SetBool("isRunning", h != 0);
            anim.SetBool("isGrounded", isGrounded);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform")) 
        {
            if (transform.position.y > collision.transform.position.y)
            {
                transform.SetParent(collision.transform);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (transform.parent != null && collision.gameObject.CompareTag("MovingPlatform"))
        {
            transform.SetParent(null);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        // 1. Hammer Items
        if (other.CompareTag("Hammer")) 
        {
            hasHammer = true; 
            Destroy(other.gameObject);
            Debug.Log("Hammer collected! Justice is now armed de!");
        }

        // 2. Key Items
        if (other.CompareTag("Key")) 
        {
            hasKey = true; 
            Destroy(other.gameObject); 
            Debug.Log("Key collected! Now head to the door.");
        }

        // 3. Goal Door Destination
        if (other.CompareTag("Door"))
        {
            if (hasKey)
            {
                Debug.Log("Door Opened! Level Complete!");
               // SceneManager.LoadScene("FOM_Special_Level"); // Uncomment this line and replace with your actual next level name
            }
            else
            {
                Debug.Log("The door is locked... you need the key ha!");
            }
        }

    }
}