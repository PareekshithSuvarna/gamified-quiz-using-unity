using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TraineeHomeManager : MonoBehaviour
{
    public TextMeshProUGUI traineeNameText;
    public Button takeQuizButton;
    public Button resultsButton;
    public Button leaderboardButton;

    void Start()
    {
        string traineeName = PlayerPrefs.GetString("traineeName", "Trainee");
        traineeNameText.text = "Welcome, " + traineeName;

        takeQuizButton.onClick.AddListener(() => {
            SceneManager.LoadScene("TakeQuizScene");
        });

        resultsButton.onClick.AddListener(ShowResult); // Optional: still works via script
        leaderboardButton.onClick.AddListener(() => SceneManager.LoadScene("LeaderboardScene"));
    }

    // 🔽 This method is what you can now hook in the Inspector
    public void ShowResult()
    {
        PlayerPrefs.SetInt("showAllResults", 0);  // Latest result only
        SceneManager.LoadScene("ResultScene");    // Ensure the name matches your scene
    }
}
