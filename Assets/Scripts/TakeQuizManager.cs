using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class TakeQuizManager : MonoBehaviour
{
    public TMP_InputField traineeNameInput;
    public TMP_InputField batchIDText;
    public TMP_InputField batchNameText;
    public TMP_Dropdown quizTypeDropdown;
    public Button startQuizButton;

    private string batchID;
    private string batchName;

    void Start()
    {
        batchID = PlayerPrefs.GetString("batchId", "N/A");
        batchName = PlayerPrefs.GetString("batchName", "N/A");

        batchIDText.text = batchID;
        batchNameText.text = batchName;

        StartCoroutine(LoadQuizTypes());
        startQuizButton.onClick.AddListener(OnStartQuizClicked);
    }

    IEnumerator LoadQuizTypes()
    {
        string url = "http://localhost/picquizz/get_quiz_types.php?batch_id=" + batchID;
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + www.error);
            yield break;
        }

        string json = www.downloadHandler.text;

        if (json == "no_quiz")
        {
            quizTypeDropdown.gameObject.SetActive(false);
            Debug.Log("No quiz types available for this batch.");
            yield break;
        }

        string[] quizTypes = JsonHelper.FromJson<string>(FixJson(json));
        quizTypeDropdown.ClearOptions();
        quizTypeDropdown.AddOptions(new List<string>(quizTypes));
    }

    void OnStartQuizClicked()
    {
        string traineeName = traineeNameInput.text;
        string selectedQuiz = quizTypeDropdown.options[quizTypeDropdown.value].text;

        if (string.IsNullOrEmpty(traineeName))
        {
            Debug.Log("Please enter your name.");
            return;
        }

        PlayerPrefs.SetString("traineeName", traineeName);
        PlayerPrefs.SetString("selectedQuiz", selectedQuiz);

        // Load Quiz Scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("StartQuizScene");
    }

    string FixJson(string value)
    {
        return "{\"array\":" + value + "}";
    }

    [System.Serializable]
    public class Wrapper<T>
    {
        public T[] array;
    }

    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.array;
        }
    }
}
