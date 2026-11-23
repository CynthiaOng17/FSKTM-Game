using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Zone2SpecialLevelManager : MonoBehaviour
{
    public static Zone2SpecialLevelManager Instance;

    [Header("Timer")]
    public float timeLimit = 30f;
    public TextMeshProUGUI timerText;

    [Header("Questions")]
    [Tooltip("SpriteRenderer on the Question_1(Answer 1) GameObject in the scene")]
    public SpriteRenderer questionDisplay;
    [Tooltip("Drag Question_1, Question_2, Question_3 sprites in order")]
    public Sprite[] questionSprites;  // 3 sprites from Assets/Sprite/FSKTM special/Zone2/

    [Header("UI Screens")]
    public GameObject winScreen;
    public GameObject loseScreen;

    [Header("Scene Transition")]
    public string nextSceneName = "";

    private float timeRemaining;
    private bool levelOver = false;

    // Static so it survives scene reloads within the same session
    // Cycles: 0 → 1 → 2 → 0 (Question_1 → Question_2 → Question_3 → Question_1)
    private static int currentQuestionIndex = 0;

    void Awake()
    {
        Instance = this;

        // Clear any 0 stored in PlayerPrefs so scene reloads don't re-load a dead CGPA
        if (PlayerPrefs.HasKey("CurrentCGPA") && PlayerPrefs.GetFloat("CurrentCGPA") <= 0f)
        {
            PlayerPrefs.DeleteKey("CurrentCGPA");
            PlayerPrefs.Save();
        }
    }

    void Start()
    {
        timeRemaining = timeLimit;

        // If CGPA loaded as 0 from PlayerPrefs, reset it so Zone2 doesn't immediately fail
        if (CGPAManager.Instance != null && CGPAManager.Instance.currentCGPA <= 0f)
            CGPAManager.Instance.currentCGPA = CGPAManager.Instance.maxCGPA;

        if (winScreen != null) winScreen.SetActive(false);
        if (loseScreen != null) loseScreen.SetActive(false);

        ShowCurrentQuestion();
    }

    void Update()
    {
        if (levelOver) return;

        timeRemaining -= Time.deltaTime;
        int secs = Mathf.Max(0, Mathf.CeilToInt(timeRemaining));
        if (timerText != null)
            timerText.text = "Time: " + secs + "s";

        // Timer expired → fail
        if (timeRemaining <= 0f)
            HandleFailure();

        // CGPA = 0 → fail
        // CGPAManager auto-reloads scene, but we advance question first so next load shows new question
        if (CGPAManager.Instance != null && CGPAManager.Instance.currentCGPA <= 0f)
            HandleFailure();
    }

    void ShowCurrentQuestion()
    {
        if (questionDisplay == null || questionSprites == null) return;
        if (currentQuestionIndex < questionSprites.Length && questionSprites[currentQuestionIndex] != null)
        {
            questionDisplay.sprite = questionSprites[currentQuestionIndex];
            Debug.Log("Zone2: Showing Question " + (currentQuestionIndex + 1));
        }
    }

    // Called by AnswerCheckerController when a ball enters the checker
    public void OnAnswerSubmitted(int ballType)
    {
        if (levelOver) return;

        // All questions have answer 1: Ball_1 = correct, Ball_0 = wrong
        if (ballType == 1)
            HandleSuccess();
        else
            HandleFailure();
    }

    void HandleSuccess()
    {
        levelOver = true;
        Time.timeScale = 0f;
        if (winScreen != null) winScreen.SetActive(true);
        Debug.Log("Zone2: Correct! Challenge complete.");
    }

    void HandleFailure()
    {
        if (levelOver) return;
        levelOver = true;

        // Advance to next question before showing lose screen / before CGPAManager reloads
        currentQuestionIndex = (currentQuestionIndex + 1) % 3;
        Debug.Log("Zone2: Failed. Next question will be Question " + (currentQuestionIndex + 1));

        // Reset CGPA so CGPAManager doesn't immediately reload the scene again
        if (CGPAManager.Instance != null)
            CGPAManager.Instance.currentCGPA = CGPAManager.Instance.maxCGPA;

        Time.timeScale = 0f;
        if (loseScreen != null) loseScreen.SetActive(true);
    }

    // ---- Buttons ----

    // Wire to WinScreen "Next Level" button
    public void LoadNextLevel()
    {
        currentQuestionIndex = 0; // Reset for full playthrough restart
        Time.timeScale = 1f;
        if (!string.IsNullOrEmpty(nextSceneName))
            SceneManager.LoadScene(nextSceneName);
        else
            Debug.LogWarning("Zone2: nextSceneName is not set in Zone2SpecialLevelManager!");
    }

    // Wire to LoseScreen "Restart Zone2" button
    public void RestartZone2()
    {
        Time.timeScale = 1f;
        // Clear any stored CGPA so reload starts fresh at 4.0
        PlayerPrefs.DeleteKey("CurrentCGPA");
        PlayerPrefs.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Optional: expose current question index for UI display
    public int GetCurrentQuestionNumber() => currentQuestionIndex + 1;
}
