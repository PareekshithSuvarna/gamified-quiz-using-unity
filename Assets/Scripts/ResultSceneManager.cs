using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class ResultSceneManager : MonoBehaviour
{
    public TMP_Text correctText, wrongText, notAttemptedText, scoreText, statusText;
    public Button retakeButton, homeButton;

    public int userId;

    void Start()
    {
        userId = PlayerPrefs.GetInt("user_id", 0); // Default to 0 if not found
        StartCoroutine(GetQuizResult(userId));

        // Button Listeners
        retakeButton.onClick.AddListener(OnRetakeQuiz);
        homeButton.onClick.AddListener(OnGoHome);
    }

    IEnumerator GetQuizResult(int userId)
    {
        string url = "http://localhost/picquizz/get_quiz_result.php?user_id=" + userId;
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string json = www.downloadHandler.text;

            QuizResult result;
            try
            {
                result = JsonUtility.FromJson<QuizResult>(json);
                correctText.text = "Correct: " + result.correct_answers;
                wrongText.text = "Wrong: " + result.wrong_answers;
                notAttemptedText.text = "Not Attempted: " + result.not_attempted; // ✅ Added this
                scoreText.text = "Score: " + result.total_score;
                statusText.text = "Status: " + result.status;
            }
            catch
            {
                Debug.LogError("JSON Parse Error: " + json);
                statusText.text = "Could not load results.";
            }
        }
        else
        {
            Debug.LogError("Error fetching result: " + www.error);
            statusText.text = "Failed to fetch results.";
        }
    }

    void OnRetakeQuiz()
    {
        SceneManager.LoadScene("StartQuizScene"); // Ensure this matches your quiz scene name
    }

    void OnGoHome()
    {
        SceneManager.LoadScene("TraineeHomeScene"); // Ensure this matches your home scene name
    }
}

[System.Serializable]
public class QuizResult
{
    public int correct_answers;
    public int wrong_answers;
    public int not_attempted;
    public int total_score;
    public string status;
}
