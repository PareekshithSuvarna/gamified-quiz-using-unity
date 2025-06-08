using UnityEngine;
using UnityEngine.SceneManagement;

public class AdminHomeManager : MonoBehaviour
{
    public void OnCreateBatchClicked()
    {
        SceneManager.LoadScene("CreateBatchScene");
    }

    public void OnLeaderboardClicked()
    {
        SceneManager.LoadScene("LeaderboardScene");
    }

    public void OnTraineeInfoClicked()
    {
        SceneManager.LoadScene("TraineeInfoScene");
    }
}
