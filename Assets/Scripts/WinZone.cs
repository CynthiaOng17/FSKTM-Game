using UnityEngine;

public class WinZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        // 🚨 TEST 1: This will print out WHATEVER touches the zone, no matter what its tag is!
        //Debug.Log("SOMETHING TOUCHED THE WIN ZONE! Object name: " + col.gameObject.name + " | Object tag: " + col.gameObject.tag);

        if (col.CompareTag("Player"))
        {
            Debug.Log("🎉 TEST 2: Tag match successful! Notifying WinManager...");
            
            if (WinManager.Instance != null)
            {
                WinManager.Instance.CheckWinZone();
            }
            else
            {
                Debug.LogError("❌ ERROR: WinManager.Instance is NULL! Is the WinManager script attached to an object in your scene?");
            }
        }
    }
}