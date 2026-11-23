using System.Collections;
using UnityEngine;

// ============================================================
//  NullController.cs
//  Attach this script to your Null player GameObject
// ============================================================

public class NullController : MonoBehaviour
{
    // --------------------------------------------------------
    // MOVEMENT SETTINGS  (edit these numbers in the Inspector)
    // --------------------------------------------------------
    [Header("Movement")]
    public float moveSpeed = 8f;        // Null is fast! Higher than normal
    public float jumpForce = 14f;       // How high Null jumps

    // --------------------------------------------------------
    // BINARY STRIKE (ranged attack)
    // --------------------------------------------------------
    [Header("Binary Strike")]
    public GameObject binaryStrikePrefab;   // Drag your projectile prefab here
    public float strikeSpeed = 15f;         // How fast the projectile flies
    public float strikeCooldown = 0.4f;     // Seconds between shots
    private float strikeTimer = 0f;         // Internal countdown

    // --------------------------------------------------------
    // SYSTEM DEBUG (stun nearby enemies for 5 seconds)
    // --------------------------------------------------------
    [Header("System Debug")]
    public float debugRadius = 5f;          // How far the stun pulse reaches
    public float stunDuration = 5f;         // Stun lasts 5 seconds (per GDD)
    public float debugCooldown = 8f;        // Seconds before you can use it again
    private float debugTimer = 0f;
    public GameObject debugEffectPrefab;    // Optional: visual pulse effect

    // --------------------------------------------------------
    // FIREWALL PROTOCOL (block one hit)
    // --------------------------------------------------------
    [Header("Firewall Protocol")]
    public float firewallCooldown = 12f;    // Long cooldown (per GDD)
    public float firewallTimer = 0f;
    private bool firewallActive = false;    // TRUE = next hit is blocked
    public GameObject firewallEffectPrefab; // Optional: shield visual, linked automatically
    [Tooltip("Use this to manually center the shield on Null's body")]
    public Vector2 shieldOffset = new Vector2(-1.2f, 0f);
    
    // Properties to safely expose firewall status to external UI or scripts if needed
    public bool IsFirewallActive => firewallActive;

    // --------------------------------------------------------
    // GROUND CHECK (so Null can only jump when on ground)
    // --------------------------------------------------------
    [Header("Ground Check")]
    public Transform groundCheck;           // Drag the GroundCheck child object here
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;           // Set to your Ground layer in Inspector
    private bool isGrounded = false;

    // --------------------------------------------------------
    // PRIVATE REFERENCES (set automatically, don't touch)
    // --------------------------------------------------------
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private float moveInput;
    private bool facingRight = false;
    private GameObject spawnableShieldInstance;
    [HideInInspector] 
    public bool hasKey = false;

    // ============================================================
    //  START  —  runs once when the scene loads
    // ============================================================
    void Start()
    {
        // Grab components attached to this GameObject
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Make sure all cooldown timers start at 0 (ready to use)
        strikeTimer = 0f;
        debugTimer = 0f;
        firewallTimer = 0f;
    }

    // ============================================================
    //  UPDATE  —  runs every frame (keyboard input goes here)
    // ============================================================
    void Update()
    {
        // --- Ground check ---
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }

        // --- Read left/right input (A/D or Arrow keys) ---
        moveInput = Input.GetAxisRaw("Horizontal");  

        // Flip sprite to face the direction Null is moving
        if (moveInput < 0 && facingRight)  Flip();   
        if (moveInput > 0 && !facingRight) Flip(); 

        // --- JUMP (Space bar) ---
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // --- Count down cooldown timers every frame ---
        if (strikeTimer > 0)   strikeTimer   -= Time.deltaTime;
        if (debugTimer > 0)    debugTimer    -= Time.deltaTime;
        if (firewallTimer > 0) firewallTimer -= Time.deltaTime;

        // --------------------------------------------------------
        // ABILITY: Binary Strike  —  press J to shoot
        // --------------------------------------------------------
        if (Input.GetKeyDown(KeyCode.J) && strikeTimer <= 0)
        {
            ShootBinaryStrike();
            strikeTimer = strikeCooldown;   // Start cooldown
        }

        // --------------------------------------------------------
        // ABILITY: System Debug  —  press K to stun enemies nearby
        // --------------------------------------------------------
        if (Input.GetKeyDown(KeyCode.K) && debugTimer <= 0)
        {
            ActivateSystemDebug();
            debugTimer = debugCooldown;     // Start cooldown
        }

        // --------------------------------------------------------
        // ABILITY: Firewall Protocol  —  press L to raise shield
        // --------------------------------------------------------
        if (Input.GetKeyDown(KeyCode.L) && firewallTimer <= 0 && !firewallActive)
        {
            ActivateFirewall();
            firewallTimer = firewallCooldown;  // Start cooldown
        }
        // LIVE POSITION TRACKING: Force it to stay locked at local 0
        if (firewallActive && spawnableShieldInstance != null)
        {
            spawnableShieldInstance.transform.localPosition = new Vector3(0f, 0f, 0f);
        }
    }

    // ============================================================
    //  FIXED UPDATE  —  runs at fixed intervals (physics goes here)
    // ============================================================
    void FixedUpdate()
    {
        if (rb == null)
        {
            Debug.LogError("NullController: No Rigidbody2D found! Please add one to " + gameObject.name);
            return;
        }
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    // ============================================================
    //  BINARY STRIKE  —  shoot a projectile in the direction Null faces
    // ============================================================
    void ShootBinaryStrike()
    {
        if (binaryStrikePrefab == null) return; // Safety check

        // Just spawn the projectile at Null's position. 
        // The BinaryStrikeScript attached to the prefab will take care of the direction!
        Instantiate(binaryStrikePrefab, transform.position, Quaternion.identity);
    }
    // ============================================================
    //  SYSTEM DEBUG  —  stun all enemies within debugRadius
    // ============================================================
    // ============================================================
    //  SYSTEM DEBUG  —  Fire a traveling pulse toward enemies
    // ============================================================
    void ActivateSystemDebug()
    {
        if (debugEffectPrefab == null) return; // Safety check

        Debug.Log("SYSTEM DEBUG PULSE LAUNCHED!");

        // Decide which direction to shoot based on where Null faces
        float direction = facingRight ? 1f : -1f;

        // Spawn the System Debug Pulse projectile at Null's position
        GameObject pulse = Instantiate(debugEffectPrefab, transform.position, Quaternion.identity);

        // Give it physical traveling velocity (slower and heavier than binary strike for dramatic effect)
        Rigidbody2D pulseRb = pulse.GetComponent<Rigidbody2D>();
        if (pulseRb != null)
        {
            pulseRb.linearVelocity = new Vector2(direction * 10f, 0f); 
        }

        // Clean it up after 2 seconds if it doesn't hit an enemy
        Destroy(pulse, 2f);
    }

    private IEnumerator StunWhiplash(WhiplashController enemy, float duration)
    {
        enemy.enabled = false; // Freeze behaviors, logic, and shooting
        yield return new WaitForSeconds(duration);
        if (enemy != null) enemy.enabled = true; // Turn back on if not dead
    }

    private IEnumerator StunMrBin(MrBinController enemy, float duration)
    {
        enemy.enabled = false; // Freeze movement logic
        yield return new WaitForSeconds(duration);
        if (enemy != null) enemy.enabled = true;
    }

    // ============================================================
    //  FIREWALL PROTOCOL  —  block the next hit
    // ============================================================
    // ============================================================
    //  FIREWALL PROTOCOL  —  block the next hit
    // ============================================================
    // ============================================================
    //  FIREWALL PROTOCOL  —  block the next hit
    // ============================================================
    void ActivateFirewall()
    {
        firewallActive = true;
        Debug.Log("FIREWALL PROTOCOL ACTIVE: Shield Up!");

        if (firewallEffectPrefab != null)
        {
            // Spawn the shield attached cleanly as a child to Null
            spawnableShieldInstance = Instantiate(firewallEffectPrefab, transform.position, Quaternion.identity, transform);

            // Hardcode it to sit EXACTLY at Null's local center (0,0)
            spawnableShieldInstance.transform.localPosition = new Vector3(0f, 0f, 0f);
        }
    }
    // ============================================================
    //  TAKE DAMAGE  —  call this from enemy/laser scripts
    //  Supports float variations (CGPA) and int damage calls safely
    // ============================================================
    public void TakeDamage(float amount)
    {
        // If Firewall is active, block the hit instead
        if (firewallActive)
        {
            firewallActive = false;
            Debug.Log("FIREWALL PROTOCOL NEGATED ATTACK! Shield Broken.");
            
            if (spawnableShieldInstance != null)
            {
                Destroy(spawnableShieldInstance);
            }
            return; // Exit — zero damage taken
        }

        // Otherwise, subtract CGPA using the CGPAManager
        if (CGPAManager.Instance != null)
        {
            CGPAManager.Instance.TakeDamage(amount);
        }
        else
        {
            Debug.Log($"Null took {amount} damage! Fragile defense warning.");
        }
    }

    // Overload variant to protect against calls passing standard integer structures
    public void TakeDamage(int amount)
    {
        TakeDamage((float)amount);
    }

    // ============================================================
    //  FLIP  —  mirror the sprite when changing direction
    // ============================================================
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;                  // Flip horizontally
        transform.localScale = scale;
    }

    // ============================================================
    //  DRAW GIZMOS  —  shows the stun radius in Scene view (editor only)
    // ============================================================
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, debugRadius);

        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}