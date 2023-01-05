using System.Collections;
using UnityEngine;
using LootLocker.Requests;
using TMPro;

public class LeaderboardUI : MonoBehaviour
{
    string leaderBoardID = "DungeonGunnerHighScore";
    [SerializeField] private Transform contentAnchorTransform;

    private void Start()
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

                GameObject scoreGameObject;
                scoreGameObject = Instantiate(GameResources.Instance.scorePrefab, contentAnchorTransform);

                ScorePrefab scorePrefab = scoreGameObject.GetComponent<ScorePrefab>();
                scorePrefab.rankTMP.text = 1.ToString();
                scorePrefab.nameTMP.text = "NOT CONNECTED";
                scorePrefab.levelTMP.text = "-";
                scorePrefab.scoreTMP.text = 0.ToString();
            }
        });

        yield return new WaitWhile(() => done == false);
    }
}
