using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class WinManager : MonoBehaviour
{
    public static WinManager Instance;
    public GameObject winScreen;
    public int totalFragments = 3;
    public int collectedFragments = 0;
    private bool bossDefeated = false;
    private bool reachedEndZone = false;
    public float requiredCGPA = 3.75f;
    public TextMeshProUGUI winTitleText;
    public TextMeshProUGUI winCGPAText;
    public AudioClip deanListSound;
    public AudioClip levelCompleteSound;
    private AudioSource audioSource;

  
    //  replace "FOM_NormalLevel" with next scene name
    public string nextSceneName = "FOM_NormalLevel";

    void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject
                .AddComponent<AudioSource>();
    }

    public void FragmentCollected()
    {
        collectedFragments++;
        Debug.Log("Fragments: " + collectedFragments
            + "/" + totalFragments);
    }

    public void BossDefeated()
    {
        bossDefeated = true;
        Debug.Log("Boss defeated!");
    }

    public void CheckWinZone()
    {
        reachedEndZone = true;
        CheckWin();
    }

    void CheckWin()
    {
        if (collectedFragments >= totalFragments
            && bossDefeated
            && reachedEndZone)
        {
            float currentCGPA = CGPAManager
                .Instance.currentCGPA;

            // Save CGPA for next level
            PlayerPrefs.SetFloat("CurrentCGPA",
                currentCGPA);
            PlayerPrefs.Save();
            Debug.Log("CGPA saved: " + currentCGPA);

            winScreen.SetActive(true);
            Time.timeScale = 0f;

            if (currentCGPA >= requiredCGPA)
            {
                winTitleText.text =
                    "LEVEL COMPLETE!\nDean List Track!";
                winCGPAText.text =
                    "Current CGPA: "
                    + currentCGPA.ToString("F2")
                    + "\nExcellent! Keep it up!";
                winTitleText.color = Color.yellow;

                if (deanListSound != null)
                    audioSource.PlayOneShot(
                        deanListSound);
            }
            else
            {
                winTitleText.text =
                    "LEVEL COMPLETE!\nKeep Going!";
                winCGPAText.text =
                    "Current CGPA: "
                    + currentCGPA.ToString("F2")
                    + "\nImprove your CGPA in "
                    + "the next level!";
                winTitleText.color = Color.white;

                if (levelCompleteSound != null)
                    audioSource.PlayOneShot(
                        levelCompleteSound);
            }
        }
        else
        {
            if (!bossDefeated)
                Debug.Log("Defeat TingTingShi first!");
            if (collectedFragments < totalFragments)
                Debug.Log("Collect all fragments first!");
            if (!reachedEndZone)
                Debug.Log("Reach end zone first!");
        }
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        if (nextSceneName != "")
            SceneManager.LoadScene(nextSceneName);
        else
            Debug.Log("No next scene assigned!");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name);
    }
}