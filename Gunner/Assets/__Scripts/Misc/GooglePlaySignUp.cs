using System.Collections;
using UnityEngine;
using Google.Play.AppUpdate;
using Google.Play.Common;
using TMPro;
using GooglePlayGames.BasicApi;
using GooglePlayGames;

public class GooglePlaySignUp : SingletonMonobehaviour<GooglePlaySignUp>
{
    public bool isConnectedToGooglePlay;

    protected override void Awake()
    {
        base.Awake();
        PlayGamesPlatform.Activate();
    }

    private void Start()
    {
        SignToGooglePlay();
    }

    
    public void SignToGooglePlay()
    {
        PlayGamesPlatform.Instance.Authenticate((result) => {
            switch (result)
            {
                case SignInStatus.Success:
                    isConnectedToGooglePlay = true;
                    break;

                default:
                    isConnectedToGooglePlay = false;
                    break;
            }
        });
    }
}
