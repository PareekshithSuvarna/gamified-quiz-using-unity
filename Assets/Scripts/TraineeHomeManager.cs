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

        takeQuizButton.onClick.AddListener(() => SceneManager.LoadScene("TakeQuizScene"));
        resultsButton.onClick.AddListener(() => SceneManager.LoadScene("ResultsScene"));
        leaderboardButton.onClick.AddListener(() => SceneManager.LoadScene("LeaderboardScene"));
    }
}
