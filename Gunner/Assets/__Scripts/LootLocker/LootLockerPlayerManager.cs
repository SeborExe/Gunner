using System.Collections;
using System.Collections.Generic;
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
        LootLockerSDKManager.SetPlayerName(GameResources.Instance.currentPlayer.playerName, (response) =>
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
}
