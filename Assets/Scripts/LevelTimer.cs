using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelTimer : MonoBehaviour
{
    public float timeLimit = 180f;
    private float timeRemaining;
    public TextMeshProUGUI timerText;

    void Start()
    {
        timeRemaining = timeLimit;
    }

    void Update()
    {
        timeRemaining -= Time.deltaTime;

        int mins = Mathf.FloorToInt(timeRemaining / 60);
        int secs = Mathf.FloorToInt(timeRemaining % 60);

        if (timerText != null)
            timerText.text = string.Format(
                "{0}:{1:00}", mins, secs);

        if (timeRemaining <= 0)
        {
            SceneManager.LoadScene(
                SceneManager.GetActiveScene().name);
        }
    }
}