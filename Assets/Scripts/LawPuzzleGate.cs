using UnityEngine;
using TMPro;

public class LawPuzzleGate : MonoBehaviour
{
    public GameObject gateObject;
    public GameObject infoUI;
    public TextMeshProUGUI infoText;
    public TextMeshProUGUI titleText;
    private bool hasTriggered = false;

    string[] lawQuotes = {
        "Article 5: No person shall be deprived of life or personal liberty save in accordance with law.",
        "Article 8: All persons are equal before the law and entitled to equal protection of the law.",
        "The presumption of innocence is a fundamental right — every accused is innocent until proven guilty.",
        "Audi alteram partem: Always hear the other side before making a judgment.",
        "Nemo judex in causa sua: No one should be a judge in their own cause.",
        "Justice must not only be done, but must be seen to be done.",
        "The rule of law means that no one is above the law, not even the government.",
        "Habeas Corpus: Every person has the right to challenge unlawful detention in court.",
        "Sub judice rule: Active court cases must not be discussed publicly to ensure fair trial.",
        "Legal aid ensures that justice is accessible to all, regardless of financial status."
    };

    string[] lawTitles = {
        "Article 5 — Right to Liberty",
        "Article 8 — Equality Before Law",
        "Presumption of Innocence",
        "Audi Alteram Partem",
        "Nemo Judex In Causa Sua",
        "Justice Must Be Seen",
        "The Rule of Law",
        "Habeas Corpus",
        "Sub Judice Rule",
        "Right to Legal Aid"
    };

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && !hasTriggered)
        {
            // Only open if 3 fragments collected
            if (WinManager.Instance != null
                && WinManager.Instance.collectedFragments >= 3)
            {
                hasTriggered = true;
                int index = Random.Range(0, lawQuotes.Length);
                titleText.text = lawTitles[index];
                infoText.text = lawQuotes[index];
                infoUI.SetActive(true);
                Time.timeScale = 0f;
            }
            else
            {
                Debug.Log("Collect all 3 fragments first!");
            }
        }
    }

    public void OnUnderstoodClicked()
    {
        Time.timeScale = 1f;
        infoUI.SetActive(false);
        gateObject.SetActive(false);
    }
}