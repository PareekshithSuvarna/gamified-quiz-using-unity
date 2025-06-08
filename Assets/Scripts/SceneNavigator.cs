using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigator : MonoBehaviour
{
    public void GoToRegisterScene()
    {
        SceneManager.LoadScene("RegisterScene");
    }
}
 