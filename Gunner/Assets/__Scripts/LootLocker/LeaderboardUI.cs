using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using System;

public class LeaderboardUI : MonoBehaviour
{
    int leaderBoardID = 10046;
    //int leaderBoardID = 8963;
    [SerializeField] private Transform contentAnchorTransform;

    private void OnEnable()
    {
        StartCoroutine(FetchLeaderBoard());
    }

    private IEnumerator FetchLeaderBoard()
    {
        yield return FetchTopHighscoreRoutine();
    }

    public IEnumerator FetchTopHighscoreRoutine()
    {
        bool done = false;
        LootLockerSDKManager.GetScoreList(leaderBoardID, 100, 0, (response) =>
        {
            if (response.success)
            {
                LootLockerLeaderboardMember[] members = response.items;

                GameObject scoreGameObject;
                foreach (LootLockerLeaderboardMember member in members)
                {
                    scoreGameObject = Instantiate(GameResources.Instance.scorePrefab, contentAnchorTransform);

                    ScorePrefab scorePrefab = scoreGameObject.GetComponent<ScorePrefab>();
                    scorePrefab.rankTMP.text = member.rank.ToString();
                    scorePrefab.nameTMP.text = member.player.name;
                    scorePrefab.levelTMP.text = member.metadata;
                    scorePrefab.scoreTMP.text = member.score.ToString();
                }

                done = true;
            }
            else
            {
                Debug.Log("Failed" + response.Error);
                done = true;
            }
        });

        yield return new WaitWhile(() => done == false);
    }
}
