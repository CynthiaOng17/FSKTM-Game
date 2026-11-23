using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NullAbilityHUD : MonoBehaviour
{
    [Header("Player Reference")]
    public NullController nullPlayer;

    [Header("1. Binary Strike UI (Ranged Attack)")]
    public Slider strikeSlider;
    public Image strikeFillImage;
    public TextMeshProUGUI strikeText; // Label in front of the slider

    [Header("2. System Debug UI (Radial Stun Blast)")]
    public Slider debugSlider;
    public Image debugFillImage;
    public TextMeshProUGUI debugText; // Label in front of the slider

    [Header("3. Firewall Shield UI (Defense Barrier)")]
    public Slider firewallSlider;
    public Image firewallFillImage;
    public TextMeshProUGUI firewallText; // Label in front of the slider

    [Header("Status Theme Colors")]
    public Color readyColor = new Color(0f, 0.8f, 1f);       // Cyan
    public Color activeColor = new Color(0.2f, 1f, 0.4f);    // Green
    public Color cooldownColor = new Color(0.4f, 0.4f, 0.4f);  // Grey

    void Update()
    {
        if (nullPlayer == null) return;

        HandleBinaryStrikeUI();
        HandleSystemDebugUI();
        HandleFirewallUI();
    }

    private void HandleBinaryStrikeUI()
    {
        if (strikeSlider == null) return;

        // Pull private countdown timer safely from NullController via Reflection
        float currentTimer = (float)(nullPlayer.GetType()
            .GetField("strikeTimer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(nullPlayer) ?? 0f);
        float maxCooldown = nullPlayer.strikeCooldown;

        float progress = 1f - Mathf.Clamp01(currentTimer / maxCooldown);
        strikeSlider.value = progress;

        if (progress >= 1f)
        {
            if (strikeFillImage != null) strikeFillImage.color = readyColor;
            if (strikeText != null) strikeText.text = "STRIKE: READY";
        }
        else
        {
            if (strikeFillImage != null) strikeFillImage.color = cooldownColor;
            if (strikeText != null) strikeText.text = $"STRIKE: {currentTimer:F1}s";
        }
    }

    private void HandleSystemDebugUI()
    {
        if (debugSlider == null) return;

        // Pull private countdown timer safely from NullController via Reflection
        float currentTimer = (float)(nullPlayer.GetType()
            .GetField("debugTimer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(nullPlayer) ?? 0f);
        float maxCooldown = nullPlayer.debugCooldown;

        float progress = 1f - Mathf.Clamp01(currentTimer / maxCooldown);
        debugSlider.value = progress;

        if (progress >= 1f)
        {
            if (debugFillImage != null) debugFillImage.color = readyColor;
            if (debugText != null) debugText.text = "DEBUG: READY";
        }
        else
        {
            if (debugFillImage != null) debugFillImage.color = cooldownColor;
            if (debugText != null) debugText.text = $"DEBUG: {currentTimer:F1}s";
        }
    }

    private void HandleFirewallUI()
    {
        if (firewallSlider == null) return;

        float currentTimer = nullPlayer.firewallTimer;
        float maxCooldown = nullPlayer.firewallCooldown;

        float progress = 1f - Mathf.Clamp01(currentTimer / maxCooldown);
        firewallSlider.value = progress;

        if (nullPlayer.IsFirewallActive)
        {
            if (firewallFillImage != null) firewallFillImage.color = activeColor;
            if (firewallText != null) firewallText.text = "SHIELD: ACTIVE";
        }
        else if (progress >= 1f)
        {
            if (firewallFillImage != null) firewallFillImage.color = readyColor;
            if (firewallText != null) firewallText.text = "SHIELD: READY";
        }
        else
        {
            if (firewallFillImage != null) firewallFillImage.color = cooldownColor;
            if (firewallText != null) firewallText.text = $"SHIELD: {currentTimer:F1}s";
        }
    }
}