using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizManager : MonoBehaviour
{
    public List<GameObject> questionImages; // Image GameObjects with buttons
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;

    public GameObject instructionsPanel;
    public Button startButton;

    public GameObject resultPanel; // ✅ New: Panel to show after quiz ends
    public TextMeshProUGUI finalScoreText; // ✅ New: Final score text inside result panel

    private int currentIndex = 0;
    private int score = 0;
    private float timer = 10f;

    void Start()
    {
        foreach (GameObject img in questionImages)
            img.SetActive(false);

        instructionsPanel.SetActive(true);
        resultPanel.SetActive(false); // ✅ Hide result panel initially
        startButton.onClick.AddListener(StartQuiz);
    }

    void StartQuiz()
    {
        instructionsPanel.SetActive(false);
        ShowImage(currentIndex);
        StartCoroutine(TimerCountdown());
    }

    void ShowImage(int index)
    {
        if (index < questionImages.Count)
            questionImages[index].SetActive(true);
    }

    void HideImage(int index)
    {
        if (index < questionImages.Count)
            questionImages[index].SetActive(false);
    }

    IEnumerator TimerCountdown()
    {
        timer = 10f;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            timerText.text = "Time: " + Mathf.Ceil(timer).ToString();
            yield return null;
        }

        NextQuestion();
    }

    public void OnRightButtonClick()
    {
        score += 10;
        scoreText.text = "Score: " + score;
        NextQuestion();
    }

    public void OnWrongButtonClick()
    {
        score -= 5;
        scoreText.text = "Score: " + score;
        NextQuestion();
    }

    void NextQuestion()
    {
        StopAllCoroutines();
        HideImage(currentIndex);
        currentIndex++;

        if (currentIndex < questionImages.Count)
        {
            ShowImage(currentIndex);
            StartCoroutine(TimerCountdown());
        }
        else
        {
            Debug.Log("Quiz completed! Final Score: " + score);
            ShowResultPanel(); // ✅ Show result
        }
    }

    void ShowResultPanel()
    {
        resultPanel.SetActive(true);
        finalScoreText.text = "Your Score: " + score;
    }
}
