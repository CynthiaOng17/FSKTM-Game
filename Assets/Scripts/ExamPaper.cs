using UnityEngine;
using UnityEngine.SceneManagement;

public class ExamPaper : MonoBehaviour
{
    public float speed = 6f;
    public float damage = 0.25f;
    public float lifetime = 5f;
    private Vector2 direction;
    private bool launched = false;

    #region FIX
    public bool isThrownBack = false;
    #endregion

    void Start()
    {
        #region FIX
        if(SceneManager.GetActiveScene().name == "FSKTM Special Level_Zone2")
        {
            return;
        }
        #endregion
        Destroy(gameObject, lifetime);
    }

#region FIX
    void OnEnable()
    {
        Invoke(nameof(DestroySelf), lifetime);
    }

    void OnDisable()
    {
        CancelInvoke(nameof(DestroySelf));
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        
    }
#endregion

    public void Launch(Vector2 dir)
    {
        direction = dir.normalized;
        launched = true;
    }

    void Update()
    {
        if (launched)
            transform.Translate(
                direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // Justice catches paper
        if (col.CompareTag("Player") && !isThrownBack)
        {
            CGPAManager.Instance?.TakeDamage(damage);
            Debug.Log("Justice hit! CGPA -0.25");
            Destroy(gameObject);
        }

        // Paper thrown back hits TingTingShi
        if (col.CompareTag("Enemy") && isThrownBack)
        {
            col.GetComponent<TingTingShiController>()
                ?.TakeDamage(1);
            Debug.Log("TingTingShi hit!");
            Destroy(gameObject);
        }
    }

    // Call this when Justice throws paper back
    public void ThrowBack(Vector2 dir)
    {
        isThrownBack = true;
        direction = dir.normalized;
        launched = true;
    }
}