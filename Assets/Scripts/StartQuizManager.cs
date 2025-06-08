using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartQuizManager : MonoBehaviour
{
    public List<GameObject> quizImages; // Each element = image + buttons group
    public TMP_Text timerText;
    public TMP_Text scoreText;
    public GameObject instructionPanel;

    private int currentQuestionIndex = 0;
    private int score = 0;
    private float timePerQuestion = 10f;
    private float timer;

    void Start()
    {
        instructionPanel.SetActive(true);
        HideAllQuestions();
    }

    public void OnNextClicked()
    {
        instructionPanel.SetActive(false);
        ShowQuestion(currentQuestionIndex);
        timer = timePerQuestion;
        StartCoroutine(StartTimer());
    }

    void ShowQuestion(int index)
    {
        HideAllQuestions();
        quizImages[index].SetActive(true);
    }

    void HideAllQuestions()
    {
        foreach (var q in quizImages)
            q.SetActive(false);
    }

    IEnumerator StartTimer()
    {
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            timerText.text = "Time: " + Mathf.CeilToInt(timer);
            yield return null;
        }
        GoToNextQuestion();
    }

    public void OnCorrectClick()
    {
        score += 10;
        scoreText.text = "Score: " + score;
        StopAllCoroutines();
        GoToNextQuestion();
    }

    public void OnWrongClick()
    {
        score -= 5;
        scoreText.text = "Score: " + score;
        StopAllCoroutines();
        GoToNextQuestion();
    }

    void GoToNextQuestion()
    {
        currentQuestionIndex++;
        if (currentQuestionIndex < quizImages.Count)
        {
            ShowQuestion(currentQuestionIndex);
            timer = timePerQuestion;
            StartCoroutine(StartTimer());
        }
        else
        {
            // Quiz done, show result or go to result scene
            Debug.Log("Quiz complete. Final Score: " + score);
            PlayerPrefs.SetInt("finalScore", score);
            UnityEngine.SceneManagement.SceneManager.LoadScene("ResultScene");
        }
    }
}
