using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class NullWinManager : MonoBehaviour
{
    public static NullWinManager Instance;

    [Header("UI Panels")]
    public GameObject winScreen;       // Drag your Victory Canvas Panel here[cite: 3]
    public TextMeshProUGUI winTitleText; //[cite: 3]
    public TextMeshProUGUI winCGPAText; //[cite: 3]

    [Header("Audio")]
    public AudioClip deanListSound; //[cite: 3]
    public AudioClip levelCompleteSound; //[cite: 3]
    private AudioSource audioSource; //[cite: 3]

    [Header("Navigation")]
    public string nextSceneName = "FOM_NormalLevel"; // Name of your next level scene[cite: 3]

    // 🔽 NEW FRAGMENT TRACKING VARIABLES
    [Header("Level Objectives")]
    [Tooltip("Total number of fragments required to unlock victory in this level.")]
    public int totalFragmentsNeeded = 8; 
    private int fragmentsCollected = 0;

    void Awake()
    {
        Instance = this; //[cite: 3]
        audioSource = GetComponent<AudioSource>(); //[cite: 3]
        if (audioSource == null) //[cite: 3]
            audioSource = gameObject.AddComponent<AudioSource>(); //[cite: 3]
    }

    // 🔽 NEW FUNCTION: Called by your PointerFragment script when a piece is picked up
    public void FragmentCollected()
    {
        fragmentsCollected++;
        Debug.Log($"Fragment secured! ({fragmentsCollected} / {totalFragmentsNeeded})");
    }

    // This is called automatically the exact second Null touches the WinZone wall[cite: 3]
    public void CheckWinZone()
    {
        // 1. OBJECTIVE CHECK: Verify if the player collected enough certificate pieces
        if (fragmentsCollected < totalFragmentsNeeded)
        {
            Debug.LogWarning($"Cannot graduate yet! Missing fragments. Collected: {fragmentsCollected}/{totalFragmentsNeeded}");
            return; // 🛑 BLOCKS VICTORY: Exits the function early so the win panel doesn't open
        }

        // 2. Fetch current CGPA score from the game manager[cite: 3]
        float currentCGPA = CGPAManager.Instance != null ? CGPAManager.Instance.currentCGPA : 4.0f; //[cite: 3]

        // 3. Check if the player is in the failing zone (< 1.50 or <= 0)[cite: 3]
        if (currentCGPA < 1.50f) //[cite: 3]
        {
            Debug.Log("Null reached the end, but CGPA is in Danger zone! Cannot graduate."); //[cite: 3]
            return; //[cite: 3]
        }

        // 4. Save the CGPA progress safely into memory for the next faculty scene[cite: 3]
        PlayerPrefs.SetFloat("CurrentCGPA", currentCGPA); //[cite: 3]
        PlayerPrefs.Save(); //[cite: 3]
        Debug.Log("Null's CGPA saved successfully: " + currentCGPA); //[cite: 3]

        // 5. Pop up the victory display panel and stop game action timers[cite: 3]
        winScreen.SetActive(true); //[cite: 3]
        Time.timeScale = 0f; //[cite: 3]

        // 6. Apply the customized victory text screens based on performance tiers[cite: 3]
        if (currentCGPA >= 3.70f && currentCGPA <= 4.00f) //[cite: 3]
        {
            // First Class Honours / Dean List[cite: 3]
            winTitleText.text = "LEVEL COMPLETE!\nHonours Track!"; //[cite: 3]
            winCGPAText.text = "Current CGPA: " + currentCGPA.ToString("F2") + "\nExcellent job, Null! Keep it up!"; //[cite: 3]
            winTitleText.color = Color.yellow; //[cite: 3]

            if (deanListSound != null) audioSource.PlayOneShot(deanListSound); //[cite: 3]
        }
        else //[cite: 3]
        {
            // Safe Zone (1.50 - 3.69)[cite: 3]
            winTitleText.text = "LEVEL COMPLETE!\nPass Safe!"; //[cite: 3]
            winCGPAText.text = "Current CGPA: " + currentCGPA.ToString("F2") + "\nYou passed the faculty! Moving on."; //[cite: 3]
            winTitleText.color = Color.white; //[cite: 3]

            if (levelCompleteSound != null) audioSource.PlayOneShot(levelCompleteSound); //[cite: 3]
        }
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1f; //[cite: 3]
        if (!string.IsNullOrEmpty(nextSceneName)) //[cite: 3]
            SceneManager.LoadScene(nextSceneName); //[cite: 3]
        else //[cite: 3]
            Debug.LogError("Please assign the next scene name in the Inspector window!"); //[cite: 3]
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; //[cite: 3]
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //[cite: 3]
    }
}