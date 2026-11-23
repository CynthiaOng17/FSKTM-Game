using UnityEngine;
using UnityEngine.SceneManagement;

public class BinaryStrikeScript : MonoBehaviour
{
    public float speed = 12f;
    public float lifetime = 2f;
    public int damage = 50;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject player = GameObject.Find("Null_0");

        if (player != null)
        {
            float direction = (player.transform.localScale.x < 0f) ? 1f : -1f;
            rb.linearVelocity = new Vector2(direction * speed, 0f);
        }
        else
        {
            rb.linearVelocity = new Vector2(speed, 0f);
        }

        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Binary Strike hit: " + collision.gameObject.name + " | Tag: " + collision.tag);

        // Ignore the player so you don't shoot yourself
        if (collision.CompareTag("Player")) return;

        #region FIX
        if(SceneManager.GetActiveScene().name == "FSKTM Special Level_Zone2")
        {
            if (collision.CompareTag("Enemy"))
            {
                TingTingShiController tts = collision.GetComponent<TingTingShiController>();
                Debug.Log("WhiplashController found: " + (tts != null));

                if (tts != null)
                {
                    tts.TakeDamage(damage);
                }
                Destroy(gameObject);
            }
            return;
        }else if(SceneManager.GetActiveScene().name == "FSKTM Special Level_Zone1")
        {
            if (collision.CompareTag("Enemy") || collision.CompareTag("Whiplash"))
            {
                WhiplashController whiplash = collision.GetComponent<WhiplashController>();
                MrBinController mrBin = collision.GetComponent<MrBinController>();
                Debug.Log("WhiplashController found: " + (whiplash != null));
                Debug.Log("MrBinController found: " + (mrBin != null));

                if (whiplash != null)
                {
                    whiplash.TakeDamage(damage);
                }
                if (mrBin != null)
                {
                    mrBin.TakeDamage(damage);
                }
                Destroy(gameObject);
            }
            return;
        }
        #endregion

        if (collision.CompareTag("Enemy"))
        {
            MrBinController mrBin = collision.GetComponent<MrBinController>();
            Debug.Log("MrBinController found: " + (mrBin != null));

            if (mrBin != null)
            {
                mrBin.TakeDamage(damage);
            }
            Destroy(gameObject);
        }


    }
}