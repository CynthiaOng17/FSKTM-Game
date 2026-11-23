using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object falling into the pit is Null
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Null fell into the pit! Executing immediate failure...");

            if (CGPAManager.Instance != null)
            {
                // Force CGPA down to 0 to trigger the automatic scene reset loop
                CGPAManager.Instance.TakeDamage(CGPAManager.Instance.currentCGPA);
            }
            else
            {
                Debug.LogError("KillZone Error: CGPAManager instance was not found in this scene!");
            }
        }
    }
}