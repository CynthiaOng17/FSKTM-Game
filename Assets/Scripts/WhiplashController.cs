using UnityEngine;
using UnityEngine.SceneManagement;

public class WhiplashController : MonoBehaviour
{
    [Header("Combat")]
    public float laserInterval = 2f;
    public float damage = 0.15f;
    public int health = 500;
    public GameObject laserPrefab;

    [Header("Movement")]
    public float moveSpeed = 3f;
    public float patrolDistance = 5f;
    public float jumpForce = 8f;
    public float jumpInterval = 3f;       // seconds between random jumps
    public LayerMask groundLayer;

    private Transform player;
    private Rigidbody2D rb;
    private bool facingRight = true;
    private bool movingRight = true;
    private float laserTimer = 0f;
    private float jumpTimer = 0f;
    private float startX;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startX = transform.position.x;

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;

        laserTimer = laserInterval;
        jumpTimer = Random.Range(1f, jumpInterval);
    }

    void Update()
    {
        if (player == null) return;

        facingRight = player.position.x > transform.position.x;
        transform.localScale = new Vector3(facingRight ? 1 : -1, 1, 1);

        laserTimer += Time.deltaTime;
        if (laserTimer >= laserInterval)
        {
            laserTimer = 0f;
            FireLaser();
        }

        jumpTimer += Time.deltaTime;
        if (jumpTimer >= jumpInterval && isGrounded)
        {
            jumpTimer = 0f;
            Jump();
        }
    }

    void FixedUpdate()
    {
        CheckGround();
        Patrol();
    }

    void CheckGround()
    { 
        // TODO:落地检测问题
        #region Fix
        if(SceneManager.GetActiveScene().name == "FSKTM Special Level_Zone1")
        {
            Vector2 checkPos = transform.position + Vector3.down * 0.5f;
            Collider2D col = GetComponent<Collider2D>();
            if (col != null)
                checkPos.y = col.bounds.min.y + 0.05f;

            LayerMask mask = groundLayer != 0 ? groundLayer : Physics2D.AllLayers;
            isGrounded = Physics2D.OverlapCircle(checkPos, 0.25f, mask);
            return;
        }
        #endregion

        isGrounded = Physics2D.OverlapCircle(
            transform.position + Vector3.down * 0.5f, 0.2f, groundLayer);
    }

    void Patrol()
    {
        float dir = movingRight ? 1f : -1f;
        rb.linearVelocity = new Vector2(dir * moveSpeed, rb.linearVelocity.y);

        if (Mathf.Abs(transform.position.x - startX) >= patrolDistance)
        {
            movingRight = !movingRight;
            startX = transform.position.x;
        }
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

#region Fix
    public Sprite LaserSprite; 
    public float LaserLength = 0.5f;
    public float LaserWidth = 0.05f;
    #endregion
    void FireLaser()
    {
        if (laserPrefab == null) return;
        GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
        #region FIX 
        if(SceneManager.GetActiveScene().name == "FSKTM Special Level_Zone1")
        {
            laser.transform.localScale = new Vector3(LaserLength, LaserWidth, 1); 
            laser.GetComponent<SpriteRenderer>().color = Color.red;
            laser.GetComponent<SpriteRenderer>().sprite = LaserSprite;
        }   
        #endregion
        MovingLaser ml = laser.GetComponent<MovingLaser>();
        if (ml != null)
            ml.Launch(facingRight ? Vector2.right : Vector2.left);
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        if (health <= 0) Destroy(gameObject);
    }
}
