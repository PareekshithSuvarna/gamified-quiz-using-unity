using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class StartQuizManager : MonoBehaviour
{
    public List<GameObject> pictureSets;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;

    [Header("Feedback UI")]
    public GameObject feedbackPanel;
    public TextMeshProUGUI feedbackText;
    public Image feedbackIcon;
    public Sprite correctSprite;
    public Sprite wrongSprite;

    [Header("Backend Settings")]
    public string saveResultURL = "http://localhost/picquizz/save_quiz_result.php";
    public int userId;

    private int currentIndex = 0;
    private int score = 0;
    private int correctAnswers = 0;
    private int wrongAnswers = 0;
    private int notAttempted = 0;
    private float timePerQuestion = 10f;
    private float currentTime = 0f;
    private bool questionActive = false;

    void Start()
    {
        userId = PlayerPrefs.GetInt("user_id");
        feedbackPanel.SetActive(false);
        ShowPicture(currentIndex);
    }

    void Update()
    {
        if (questionActive)
        {
            currentTime -= Time.deltaTime;
            timerText.text = "Time: " + Mathf.Ceil(currentTime).ToString();

            if (currentTime <= 0f)
            {
                questionActive = false;
                notAttempted++;
                ShowFeedback("Time Up", wrongSprite);
                MoveToNextPicture();
            }
        }
    }

    void ShowPicture(int index)
    {
        foreach (GameObject go in pictureSets)
            go.SetActive(false);

        if (index < pictureSets.Count)
        {
            pictureSets[index].SetActive(true);
            currentTime = timePerQuestion;
            questionActive = true;
        }
        else
        {
            timerText.text = "Quiz Over!";
            StartCoroutine(SubmitQuizResult());
        }
    }

    public void OnRightButtonClicked()
    {
        if (!questionActive) return;

        score += 10;
        correctAnswers++;
        UpdateScore();
        questionActive = false;
        ShowFeedback("+10", correctSprite);
        MoveToNextPicture();
    }

    public void OnWrongButtonClicked()
    {
        if (!questionActive) return;

        score -= 5;
        wrongAnswers++;
        UpdateScore();
        questionActive = false;
        ShowFeedback("-5", wrongSprite);
        MoveToNextPicture();
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    void MoveToNextPicture()
    {
        currentIndex++;
        Invoke(nameof(ShowNext), 1f);
    }

    void ShowNext()
    {
        ShowPicture(currentIndex);
    }

    void ShowFeedback(string message, Sprite icon)
    {
        feedbackText.text = message;
        feedbackIcon.sprite = icon;
        feedbackPanel.SetActive(true);
        Invoke(nameof(HideFeedback), 1f);
    }

    void HideFeedback()
    {
        feedbackPanel.SetActive(false);
    }
IEnumerator SubmitQuizResult()
{
    float percentage = ((float)correctAnswers / pictureSets.Count) * 100f;
    string status = (percentage >= 80f) ? "PASS" : "FAIL";

    WWWForm form = new WWWForm();
    form.AddField("user_id", userId);
    form.AddField("correct_answers", correctAnswers);
    form.AddField("wrong_answers", wrongAnswers);
    form.AddField("not_attempted", notAttempted);
    form.AddField("total_score", score);
    form.AddField("total_questions", pictureSets.Count);
    form.AddField("status", status); // ✅ Send status

    UnityWebRequest www = UnityWebRequest.Post(saveResultURL, form);
    yield return www.SendWebRequest();

    if (www.result == UnityWebRequest.Result.Success)
    {
        Debug.Log("Quiz result submitted successfully!");
        SceneManager.LoadScene("ResultScene");
    }
    else
    {
        Debug.LogError("Error submitting result: " + www.error);
    }
}

}
