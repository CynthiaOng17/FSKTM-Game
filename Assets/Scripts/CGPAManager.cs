using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CGPAManager : MonoBehaviour
{
    public static CGPAManager Instance;
    public float currentCGPA = 4.0f;
    public float maxCGPA = 4.0f;
    public Slider cgpaSlider;
    public Image screenTint;
    public TextMeshProUGUI cgpaText;
    public AudioClip hitSound;
    public AudioClip collectSound;
    private AudioSource audioSource;
    private float changeDisplay = 0f;
    private string changeText = "";

    void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        // Load CGPA from previous level
        if (PlayerPrefs.HasKey("CurrentCGPA"))
        {
            currentCGPA = PlayerPrefs.GetFloat("CurrentCGPA");
            Debug.Log("Loaded CGPA from previous level: " + currentCGPA);
        }
        else
        {
            // First level or Full Game Restart - start at 4.0
            currentCGPA = 4.0f;
            Debug.Log("No saved CGPA found. Starting fresh at 4.0");
        }
    }

    void Update()
    {
        if (cgpaSlider != null)
            cgpaSlider.value = currentCGPA / maxCGPA;

        if (cgpaText != null)
        {
            string change = "";
            if (changeDisplay > 0)
            {
                change = "\n\n" + changeText; 
                changeDisplay -= Time.deltaTime;
            }
            cgpaText.text = "CGPA: " + currentCGPA.ToString("F2") + change;
        }

        // ====== ENHANCED DANGER SELECTION EFFECT ======
        if (screenTint != null)
        {
            if (currentCGPA < 1.5f)
            {
                float dangerSeverity = 1f - (currentCGPA / 1.5f); 
                float pulseSpeed = Mathf.Lerp(3f, 7f, dangerSeverity);
                float pulseWave = Mathf.Sin(Time.time * pulseSpeed) * 0.15f + 0.2f;
                float finalAlpha = Mathf.Clamp(dangerSeverity * 0.5f + pulseWave, 0.2f, 0.85f);

                screenTint.color = new Color(0.7f, 0f, 0f, finalAlpha);
            }
            else
            {
                screenTint.color = new Color(0, 0, 0, 0);
            }
        }

        // ====== WHEN NULL FAILS THE LEVEL ======
        if (currentCGPA <= 0)
        {
            Debug.Log("Null failed the level! Reloading stage with their original grade entry score.");
            
            // Reload the level scene immediately
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // Call this function from your Main Menu "New Game" button or your Total Reset flow!
    public void ResetEntireGameCGPA()
    {
        Debug.Log("Resetting all game data! Next run will start at a perfect 4.0.");
        PlayerPrefs.DeleteKey("CurrentCGPA");
        PlayerPrefs.Save();
    }

    public void GainPoints(float amount)
    {
        currentCGPA = Mathf.Min(maxCGPA, currentCGPA + amount);
        changeText = "+" + amount.ToString("F2");
        changeDisplay = 2f;
        if (collectSound != null)
            audioSource.PlayOneShot(collectSound);
        Debug.Log("CGPA now: " + currentCGPA);
    }

    public void TakeDamage(float amount)
    {
        currentCGPA = Mathf.Max(0, currentCGPA - amount);
        changeText = "-" + amount.ToString("F2");
        changeDisplay = 2f;
        if (hitSound != null)
            audioSource.PlayOneShot(hitSound);
        Debug.Log("CGPA now: " + currentCGPA);
    }
}