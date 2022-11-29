using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using TMPro;

public class Leaderboard : SingletonMonobehaviour<Leaderboard>
{
    int leaderBoardID = 8963;

    protected override void Awake()
    {
        DontDestroyOnLoad(this);
        base.Awake();

        StartCoroutine(SetupRoutine());
    }

    IEnumerator SetupRoutine()
    {
        yield return LoginRoutine();
    }

    IEnumerator LoginRoutine()
    {
        bool done = false;
        LootLockerSDKManager.StartSession(PlayerPrefs.GetString("PlayerID") ,(response) =>
        {
            if (response.success)
            {
                Debug.Log("Player was logged in");
                PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                done = true;
            }
            else
            {
                Debug.Log("Could not start session");
                done = true;
            }
        });

        yield return new WaitWhile(() => done == false);
    }

    public IEnumerator SubmitScoreRoutine(string playerID ,int scoreToUpload, string levelName)
    {
        bool done = false;
        //string playerID = PlayerPrefs.GetString("PlayerID");
        LootLockerSDKManager.SubmitScore(playerID, scoreToUpload, leaderBoardID, levelName, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Successfyly upload score");
                done = true;
            }
            else
            {
                Debug.Log("Failed " + response.Error);
                done = true;
            }
        });

        yield return new WaitWhile(() => done == false);
    }
}