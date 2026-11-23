using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class SystemDebugPulseProjectile : MonoBehaviour
{
    public float stunDuration = 5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        #region Fix // Debug命中
        if(SceneManager.GetActiveScene().name == "FSKTM Special Level_Zone1")
        {
            if(collision.CompareTag("Whiplash")|| collision.CompareTag("Enemy"))
            {
                WhiplashController whiplash = collision.GetComponent<WhiplashController>();
                MrBinController mrBin = collision.GetComponent<MrBinController>();

                // Stun them out!
                if (whiplash != null) whiplash.StartCoroutine(StunWhiplash(whiplash, stunDuration));
                if (mrBin != null)    mrBin.StartCoroutine(StunMrBin(mrBin, stunDuration));

                // Also trigger any generic messaging receivers
                collision.SendMessage("Paralyze", stunDuration, SendMessageOptions.DontRequireReceiver);

                // Destroy the data wave upon hitting the target
                Destroy(gameObject);
            }

            return;
        } 
        #endregion


        // When the traveling pulse hits a tagged enemy
        if (collision.CompareTag("Enemy"))
        {
            WhiplashController whiplash = collision.GetComponent<WhiplashController>();
            MrBinController mrBin = collision.GetComponent<MrBinController>();

            // Stun them out!
            if (whiplash != null) whiplash.StartCoroutine(StunWhiplash(whiplash, stunDuration));
            if (mrBin != null)    mrBin.StartCoroutine(StunMrBin(mrBin, stunDuration));

            // Also trigger any generic messaging receivers
            collision.SendMessage("Paralyze", stunDuration, SendMessageOptions.DontRequireReceiver);

            // Destroy the data wave upon hitting the target
            Destroy(gameObject);
        }
    }

    private IEnumerator StunWhiplash(WhiplashController enemy, float duration)
    {
        enemy.enabled = false;
        yield return new WaitForSeconds(duration);
        if (enemy != null) enemy.enabled = true;
    }

    private IEnumerator StunMrBin(MrBinController enemy, float duration)
    {
        enemy.enabled = false;
        yield return new WaitForSeconds(duration);
        if (enemy != null) enemy.enabled = true;
    }
}