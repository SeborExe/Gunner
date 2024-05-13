using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaderBoardsUI : MonoBehaviour
{
    public void LoadHighScores()
    {
        SceneManager.UnloadSceneAsync("LeaderBoardSelect");
        SceneManager.LoadScene("HighScoreScene", LoadSceneMode.Additive);
    }

    public void LoadHighScoresOnline()
    {
        SceneManager.UnloadSceneAsync("LeaderBoardSelect");
        SceneManager.LoadScene("HighScoreSceneOnline", LoadSceneMode.Additive);
    }

    public void ReturnToLeaderboardsFromHighScore()
    {
        SceneManager.UnloadSceneAsync("HighScoreScene");
        SceneManager.LoadScene("LeaderBoardSelect", LoadSceneMode.Additive);
    }

    public void ReturnToLeaderboardsFromHighScoreOnline()
    {
        SceneManager.UnloadSceneAsync("HighScoreSceneOnline");
        SceneManager.LoadScene("LeaderBoardSelect", LoadSceneMode.Additive);
    }
}
