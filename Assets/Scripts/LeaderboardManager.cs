using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class LeaderboardEntry
{
    public string trainee_name;
    public int score;
    public int rank;
}

[System.Serializable]
public class LeaderboardResponse
{
    public string status;
    public List<LeaderboardEntry> data;
}

public class LeaderboardManager : MonoBehaviour
{
    public Transform contentPanel;
    public GameObject leaderboardEntryPrefab;

    private string leaderboardURL = "http://localhost/quiz/get_leaderboard.php";

    void Start()
    {
        StartCoroutine(GetLeaderboard());
    }

    IEnumerator GetLeaderboard()
    {
        UnityWebRequest www = UnityWebRequest.Get(leaderboardURL);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error: " + www.error);
        }
        else
        {
            LeaderboardResponse response = JsonUtility.FromJson<LeaderboardResponse>(www.downloadHandler.text);

            if (response.status == "success")
            {
                foreach (LeaderboardEntry entry in response.data)
                {
                    GameObject go = Instantiate(leaderboardEntryPrefab, contentPanel);

                    TMP_Text[] texts = go.GetComponentsInChildren<TMP_Text>();
                    texts[0].text = entry.rank.ToString();       // RankText
                    texts[1].text = entry.trainee_name;          // NameText
                    texts[2].text = entry.score.ToString();      // ScoreText
                }
            }
        }
    }
}
