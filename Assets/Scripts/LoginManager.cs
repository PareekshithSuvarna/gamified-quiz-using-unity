using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_Text statusText;

    public string loginUrl = "http://localhost/picquizz/login_user.php"; // Update this to your hosting if needed

    public void OnLoginButtonClick()
    {
        StartCoroutine(LoginRoutine());
    }

    IEnumerator LoginRoutine()
    {
        statusText.text = "Logging in...";

        WWWForm form = new WWWForm();
        form.AddField("email", emailInput.text);
        form.AddField("password", passwordInput.text);

        UnityWebRequest www = UnityWebRequest.Post(loginUrl, form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string json = www.downloadHandler.text;
            Debug.Log("Received JSON: " + json);

            try
            {
                LoginResponse response = JsonUtility.FromJson<LoginResponse>(json);

                if (response.status == "success")
                {
                    statusText.text = "Login successful.";

                    // Redirect based on role
                    switch (response.role)
                    {
                        case "admin":
                            SceneManager.LoadScene("AdminHomeScene");
                            break;

                        case "trainee":
                            SceneManager.LoadScene("TraineeHomeScene");
                            break;

                        default:
                            statusText.text = "Unknown role: " + response.role;
                            break;
                    }
                }
                else
                {
                    statusText.text = "Login failed: " + response.message;
                }
            }
            catch (System.Exception e)
            {
                statusText.text = "Error parsing server response.";
                Debug.LogError("JSON parse error: " + e.Message);
                Debug.LogError("Raw JSON: " + json);
            }
        }
        else
        {
            statusText.text = "Network error: " + www.error;
            Debug.LogError("Network error: " + www.error);
        }
    }

    [System.Serializable]
    public class LoginResponse
    {
        public string status;
        public string message;
        public string role;
        public string id;
        public string name;
        public string email;
    }
}
