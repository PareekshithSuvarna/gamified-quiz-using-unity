using System.Collections;               
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class RegisterManager : MonoBehaviour
{
    public TMP_InputField nameInput;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_Text statusText;

    private string registerURL = "http://localhost/picquizz/register_user.php"; // update to your correct local or server path

    public void Register()
    {
        string name = nameInput.text;
        string email = emailInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            statusText.text = "Please fill in all fields.";
            return;
        }

        StartCoroutine(RegisterUser(name, email, password));
    }

    IEnumerator RegisterUser(string name, string email, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("name", name);
        form.AddField("email", email);
        form.AddField("password", password);

        UnityWebRequest www = UnityWebRequest.Post(registerURL, form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            statusText.text = "Error: " + www.error;
        }
        else
        {
            // parse JSON response
            string responseText = www.downloadHandler.text;
            Debug.Log("Response: " + responseText);

            if (responseText.Contains("success"))
            {
                statusText.text = "Registration successful! Redirecting to login...";
                yield return new WaitForSeconds(2);
                SceneManager.LoadScene("StartScene"); // go back to login
            }
            else
            {
                statusText.text = "Registration failed. Try a different email.";
            }
        }
    }

    public void GoToLoginScene()
    {
        SceneManager.LoadScene("StartScene");
    }
}
