using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class TraineeInfoManager : MonoBehaviour
{
    public GameObject rowPrefab;            // Assign your row prefab in Inspector
    public Transform contentPanel;          // Assign the Content object under ScrollView

    [System.Serializable]
    public class Trainee
    {
        public string trainee_id;
        public string name;
        public string email;
        public string phone;
        public string batch_id;
    }

    [System.Serializable]
    public class TraineeListWrapper
    {
        public Trainee[] items;  
    }

    void Start()
    {
        StartCoroutine(LoadTraineeData());
    }

    IEnumerator LoadTraineeData()
    {
        string url = "http://localhost/picquizz/get_all_trainees.php";
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + www.error);
            yield break;
        }

        string json = www.downloadHandler.text;
        Debug.Log("Raw JSON: " + json);  // ✅ Add this to check if response is valid

        if (json == "no_data")
        {
            Debug.Log("No data found.");
            yield break;
        }

        // ✅ Wrap the array response into a JSON object
        string wrappedJson = "{\"items\":" + json + "}";

        // ✅ Deserialize using Unity's JsonUtility
        TraineeListWrapper wrapper = JsonUtility.FromJson<TraineeListWrapper>(wrappedJson);

        foreach (Trainee trainee in wrapper.items)
        {
            GameObject row = Instantiate(rowPrefab, contentPanel);

            TextMeshProUGUI[] fields = row.GetComponentsInChildren<TextMeshProUGUI>();
            if (fields.Length >= 4)
            {
                fields[0].text = trainee.name;
                fields[1].text = trainee.email;
                fields[2].text = trainee.phone;
                fields[3].text = trainee.batch_id;
            }
            else
            {
                Debug.LogError("Row prefab doesn't have enough TMP text fields!");
            }
        }
    }
}
