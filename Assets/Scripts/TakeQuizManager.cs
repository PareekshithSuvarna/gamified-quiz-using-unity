using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

[System.Serializable]
public class QuizType
{
    public string name;
    public string scene_name;
}

[System.Serializable]
public class QuizTypeList
{
    public QuizType[] quizTypes;
}

public class TakeQuizManager : MonoBehaviour
{
    public TMP_InputField traineeNameInput;
    public TMP_InputField batchIDText;
    public TMP_InputField batchNameText;
    public TMP_Dropdown quizTypeDropdown;
    public Button startQuizButton;

    private List<QuizType> quizTypes = new List<QuizType>();

    void Start()
    {
        StartCoroutine(LoadQuizTypes());
        startQuizButton.onClick.AddListener(OnStartQuizClicked);
    }

    IEnumerator LoadQuizTypes()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://localhost/picquizz/get_quiz_types.php");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to fetch quiz types: " + www.error);
            yield break;
        }

        string json = "{\"quizTypes\":" + www.downloadHandler.text + "}";
        QuizTypeList quizTypeList = JsonUtility.FromJson<QuizTypeList>(json);
        quizTypes = new List<QuizType>(quizTypeList.quizTypes);

        quizTypeDropdown.ClearOptions();
        List<string> names = new List<string>();
        foreach (var qt in quizTypes)
        {
            names.Add(qt.name);
        }
        quizTypeDropdown.AddOptions(names);
    }

    void OnStartQuizClicked()
    {
        int selectedIndex = quizTypeDropdown.value;
        string sceneToLoad = quizTypes[selectedIndex].scene_name;

        Debug.Log("Loading Scene: " + sceneToLoad);
        SceneManager.LoadScene(sceneToLoad);
    }
}
