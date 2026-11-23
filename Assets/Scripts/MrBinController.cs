using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MrBinController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;
    public float patrolDistance = 4f;

    [Header("Debris")]
    public float debrisInterval = 3f;
    public GameObject debrisPrefab;

    [Header("Health")]
    public int maxHealth = 500;

    // ---- private fields ----
    private int currentHealth;
    private bool movingRight = true;
    private Vector3 startPos;

    #region Fix
    private float leftBound;
    private float rightBound;
    #endregion

    private bool isDisabled = false;
    private bool wheelsDestroyed = false;   // track 50% phase separately
    private Coroutine debrisCoroutine;      // moved to top where it belongs

    void Start()
    {
        currentHealth = maxHealth;
        startPos = transform.position;

        #region Fix
        leftBound = startPos.x - patrolDistance;
        rightBound = startPos.x;
        #endregion

        debrisCoroutine = StartCoroutine(ThrowDebris()); // only ONE call
        
    }

    void Update()
    {
        if (isDisabled || wheelsDestroyed) return; // stationary turret can't patrol
        Patrol();
    }

    void Patrol()
    {
        float direction = movingRight ? 1f : -1f;

        #region Fix
        if(SceneManager.GetActiveScene().name == "FSKTM Special Level_Zone1")
        {
            transform.Translate(Vector2.right * direction * moveSpeed * Time.deltaTime);

            // Check boundaries and reverse direction
            if (movingRight && transform.position.x >= rightBound)
            {
                movingRight = false;
            }
            else if (!movingRight && transform.position.x <= leftBound)
            {
                movingRight = true;
            }

            return;
        } 
        #endregion


        
        transform.Translate(Vector2.right * direction * moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, startPos) >= patrolDistance)
        {
            movingRight = !movingRight;
            startPos = transform.position;
        }
    }

    IEnumerator ThrowDebris()
    {
        while (true)
        {
            yield return new WaitForSeconds(debrisInterval);
            if (debrisPrefab != null && !isDisabled)
            {
                float dir = movingRight ? 1f : -1f;
                var d = Instantiate(debrisPrefab, transform.position + Vector3.up, Quaternion.identity);
                var rb = d.GetComponent<Rigidbody2D>();
                if (rb != null)
                    rb.AddForce(new Vector2(dir * 5f, 4f), ForceMode2D.Impulse);
            }
        }
    }

    public void TakeDamage(int damage)
{
    currentHealth -= damage;
    Debug.Log("Mr. Bin HP: " + currentHealth + "/" + maxHealth);

    // 50% threshold — lose wheels, become stationary turret
    if (currentHealth <= maxHealth / 2 && !wheelsDestroyed)
    {
        wheelsDestroyed = true;
        Debug.Log("Mr. Bin wheels destroyed — now stationary turret!");

        // FIX: If Mr. Bin has a Rigidbody2D, stop his physical velocity instantly
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero; 
            rb.bodyType = RigidbodyType2D.Kinematic; // Optional: Makes him unmovable by physics pushes
        }
    }

    if (currentHealth <= 0)
    {
        Debug.Log("Mr. Bin destroyed!");
        Destroy(gameObject);
    }
}

    public void Paralyze(float duration)
    {
        StartCoroutine(ParalyzeRoutine(duration));
    }

    private IEnumerator ParalyzeRoutine(float duration)
    {
        isDisabled = true;
        if (debrisCoroutine != null)
            StopCoroutine(debrisCoroutine);

        yield return new WaitForSeconds(duration);

        if (this != null && gameObject != null)
        {
            isDisabled = false;
            debrisCoroutine = StartCoroutine(ThrowDebris());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Antenna melee — damages player on close contact
         if (collision.CompareTag("Player"))
        {
            if (CGPAManager.Instance != null) 
                CGPAManager.Instance.TakeDamage(0.20f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (CGPAManager.Instance != null) 
                CGPAManager.Instance.TakeDamage(0.20f);
        }
    }
}