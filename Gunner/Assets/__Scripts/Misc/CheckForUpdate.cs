using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System;
using UnityEngine.UI;

public class CheckForUpdate : MonoBehaviour
{
    [SerializeField] string versionUrl;
    [SerializeField] GameObject newVersionPopUp;
    [SerializeField] string googleStoreGameUrl;

    [Header("Buttons")]
    [SerializeField] Button updateBtn;
    [SerializeField] Button closePopUpBtn;

    private string currentVersion;
    private string latestVersion;

    private void Start()
    {
        currentVersion = Application.version;

        StartCoroutine(LoadTxtData(versionUrl));
    }

    private void OnEnable()
    {
        updateBtn.onClick.AddListener(OpenUrl);
        closePopUpBtn.onClick.AddListener(ClosePopUp);
    }

    private void OnDisable()
    {
        updateBtn.onClick.RemoveListener(OpenUrl);
        closePopUpBtn.onClick.RemoveListener(ClosePopUp);
    }

    private IEnumerator LoadTxtData(string url)
    {
        UnityWebRequest loaded = new UnityWebRequest(url);
        loaded.downloadHandler = new DownloadHandlerBuffer();

        yield return loaded.SendWebRequest();

        latestVersion = loaded.downloadHandler.text;
        CheckVersion();
    }

    private void CheckVersion()
    {
        Version versionDevice = new Version(currentVersion);
        Version versionServer = new Version(latestVersion);

        int result = versionDevice.CompareTo(versionServer);

        if ((latestVersion != "") && (result < 0))
        {
            newVersionPopUp.SetActive(true);
        }
    }

    private void ClosePopUp()
    {
        newVersionPopUp.SetActive(false);
    }

    private void OpenUrl()
    {
        Application.OpenURL(googleStoreGameUrl);
        newVersionPopUp.SetActive(false);
    }
}
