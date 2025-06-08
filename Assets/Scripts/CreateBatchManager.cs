using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class CreateBatchManager : MonoBehaviour
{
    public TMP_InputField batchIdField;
    public TMP_InputField batchNameField;
    public TMP_InputField dateField;
    public TMP_Dropdown quizTypeDropdown;
    public string apiUrl = "http://localhost/picquizz/create_batch.php";  // Replace with your actual local URL if needed

    public void OnSubmitBatch()
    {
        StartCoroutine(SendBatchData());
    }

    IEnumerator SendBatchData()
    {
        // Create form and add fields
        WWWForm form = new WWWForm();
        form.AddField("batch_id", batchIdField.text);
        form.AddField("batch_name", batchNameField.text);
        form.AddField("quiz_type", quizTypeDropdown.options[quizTypeDropdown.value].text);
        form.AddField("date", dateField.text); // Format: YYYY-MM-DD

        // Send request
        UnityWebRequest www = UnityWebRequest.Post(apiUrl, form);
        yield return www.SendWebRequest();

        // Handle response
        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Batch Created Successfully: " + www.downloadHandler.text);
            // You can navigate to another scene or show a popup here
        }
        else
        {
            Debug.LogError("Failed to create batch: " + www.error);
        }
    }
}
