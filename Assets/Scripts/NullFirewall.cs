using UnityEngine;
using UnityEngine.UI;

public class FirewallHUD : MonoBehaviour
{
    [Header("References")]
    public NullController nullPlayer;

    [Header("Cooldown Bar")]
    public Image cooldownBarFill;        // Image set to Filled type
    public GameObject readyGlow;         // Optional glow/icon that shows when ready
    public TMPro.TextMeshProUGUI statusText; // Optional text label

    [Header("Colors")]
    public Color readyColor = new Color(0f, 0.8f, 1f);      // Cyan when ready
    public Color activeColor = new Color(0.2f, 1f, 0.4f);   // Green when shield is up
    public Color cooldownColor = new Color(0.4f, 0.4f, 0.4f); // Grey during cooldown

    void Update()
    {
        if (nullPlayer == null || cooldownBarFill == null) return;

        float fillAmount = 1f - Mathf.Clamp01(nullPlayer.firewallTimer / nullPlayer.firewallCooldown);

        cooldownBarFill.fillAmount = fillAmount;

        // Shield is currently active (blocking next hit)
        if (nullPlayer.IsFirewallActive)
        {
            cooldownBarFill.color = activeColor;
            if (statusText != null) statusText.text = "FIREWALL: ACTIVE";
            if (readyGlow != null) readyGlow.SetActive(true);
        }
        // Fully recharged and ready to use
        else if (fillAmount >= 1f)
        {
            cooldownBarFill.color = readyColor;
            if (statusText != null) statusText.text = "FIREWALL: READY";
            if (readyGlow != null) readyGlow.SetActive(true);
        }
        // Still cooling down
        else
        {
            cooldownBarFill.color = cooldownColor;
            float secondsLeft = nullPlayer.firewallTimer;
            if (statusText != null) statusText.text = $"FIREWALL: {secondsLeft:F1}s";
            if (readyGlow != null) readyGlow.SetActive(false);
        }
    }
}