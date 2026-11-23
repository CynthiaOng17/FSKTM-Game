using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NullHUDDisplay : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Drag the Null player character here.")]
    public NullController nullPlayer;

    [Header("Firewall Ability UI")]
    public Image firewallBarFill;         // Image set to "Filled" image type
    public TextMeshProUGUI firewallText;  // Text indicating status or countdown
    public GameObject firewallReadyGlow;  // Optional pop-out visual glow object

    [Header("Level Objectives UI")]
    public TextMeshProUGUI fragmentsObjectiveText; // Text displaying remaining count

    [Header("HUD Progress Colors")]
    public Color readyColor = new Color(0f, 0.8f, 1f);       // Cyan Blue
    public Color activeColor = new Color(0.2f, 1f, 0.4f);    // Neon Green
    public Color cooldownColor = new Color(0.4f, 0.4f, 0.4f);  // Flat Grey

    void Update()
    {
        UpdateFirewallSkill();
        UpdateObjectiveTracker();
    }

    private void UpdateFirewallSkill()
    {
        if (nullPlayer == null || firewallBarFill == null) return;

        // Calculate fill percentage (reverses countdown from 0% to 100%)
        float fillAmount = 1f - Mathf.Clamp01(nullPlayer.firewallTimer / nullPlayer.firewallCooldown);
        firewallBarFill.fillAmount = fillAmount;

        // Case A: Shield is currently absorbing hits
        if (nullPlayer.IsFirewallActive)
        {
            firewallBarFill.color = activeColor;
            if (firewallText != null) firewallText.text = "FIREWALL: ACTIVE";
            if (firewallReadyGlow != null) firewallReadyGlow.SetActive(true);
        }
        // Case B: Shield is fully charged and ready to deploy
        else if (fillAmount >= 1f)
        {
            firewallBarFill.color = readyColor;
            if (firewallText != null) firewallText.text = "FIREWALL: READY";
            if (firewallReadyGlow != null) firewallReadyGlow.SetActive(true);
        }
        // Case C: Shield is currently down and cooling down
        else
        {
            firewallBarFill.color = cooldownColor;
            if (firewallText != null) firewallText.text = $"RECHARGING: {nullPlayer.firewallTimer:F1}s";
            if (firewallReadyGlow != null) firewallReadyGlow.SetActive(false);
        }
    }

    private void UpdateObjectiveTracker()
    {
        if (fragmentsObjectiveText == null) return;

        int totalNeeded = 0;
        int currentlyCollected = 0;

        // Dynamically pull data from whichever active Win Manager is managing the faculty level
        if (NullWinManager.Instance != null)
        {
            totalNeeded = NullWinManager.Instance.totalFragmentsNeeded;
            // Accessing internal private or public counters
            // If fields are completely hidden, we calculate remaining difference:
            currentlyCollected = System.Convert.ToInt32(NullWinManager.Instance.GetType()
                .GetField("fragmentsCollected", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(NullWinManager.Instance) ?? 0);
        }
        else if (WinManager.Instance != null)
        {
            totalNeeded = WinManager.Instance.totalFragments;
            currentlyCollected = WinManager.Instance.collectedFragments;
        }
        else
        {
            fragmentsObjectiveText.text = "Objective: Locate Exit";
            return;
        }

        // Calculate how many more fragments Null needs to collect
        int remainingFragments = totalNeeded - currentlyCollected;

        if (remainingFragments > 0)
        {
            fragmentsObjectiveText.text = $"Dean's List Pieces: Collect {remainingFragments} more!";
            fragmentsObjectiveText.color = Color.white;
        }
        else
        {
            fragmentsObjectiveText.text = "Objective Complete: Go to the Win Zone!";
            fragmentsObjectiveText.color = Color.yellow; // Turn text yellow when finished
        }
    }
}