using UnityEngine;
using LootLocker.Requests;

public class LootLockerPlayerManager : MonoBehaviour
{
    private void Start()
    {
        SetPlayerName();
    }

    public void SetPlayerName()
    {
        LootLockerSDKManager.SetPlayerName(GetPlayerName(), (response) =>
        {
            if (response.success)
            {
                Debug.Log("Successfuly set player name");
            }
            else
            {
                Debug.Log("Could not set player name " + response.Error);
            }
        });
    }

    private string GetPlayerName()
    {
        if (!string.IsNullOrEmpty(GameResources.Instance.currentPlayer.playerName))
        {
            return GameResources.Instance.currentPlayer.playerName;
        }
        else
        {
            return "Unknown Hero";
        }
    }
}
