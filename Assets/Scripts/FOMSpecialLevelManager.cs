using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class FOMSpecialLevelManager : MonoBehaviour
{
    public static FOMSpecialLevelManager Instance;

    [Header("UI Elements")]
    public TextMeshProUGUI objectiveText; // Drag your UI Text here

    [Header("Level Settings")]
    public string nextSceneName = "FOM Special Level_Zone2";
    public int totalMedicineRequired = 3;

    private int currentMedicineCount = 0;

    void Awake()
    {
        // Simple singleton pattern to easily access this script
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateUI();
    }

    // Call this method whenever the player collects a medicine
    public void CollectMedicine()
    {
        currentMedicineCount++;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (objectiveText != null)
        {
            objectiveText.text = $"Medicines Collected :\n{currentMedicineCount} / {totalMedicineRequired}";
        }
    }

    // Call this when player touches the door
    public void CheckDoorTransition()
    {
        if (currentMedicineCount >= totalMedicineRequired)
        {
            Debug.Log("Success! Loading next scene...");
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            // Simple visual warning in console, you can replace this with a UI pop-up
            Debug.LogWarning($"You must collect all {totalMedicineRequired} medicines!");
            if (objectiveText != null)
            {
                objectiveText.text = $"<color=red>Collect ALL {totalMedicineRequired} medicines!</color>";
                // Revert text color back after 2 seconds
                Invoke(nameof(UpdateUI), 2f);
            }
        }
    }
}