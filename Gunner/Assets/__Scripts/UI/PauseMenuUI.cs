using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private TMP_Text musicLevelText;
    [SerializeField] private TMP_Text soundsLevelText;

    [Header("Level")]
    [SerializeField] TMP_Text levelNumberText;
    [SerializeField] TMP_Text levelNameText;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator InitializeUI()
    {
        yield return null;

        GameManager.Instance.controls.SetActive(false);
        soundsLevelText.SetText(SoundsEffectManager.Instance.soundsVolume.ToString());
        musicLevelText.SetText(MusicManager.Instance.musicVolume.ToString());

        ShowLevelName();
    }

    private void OnEnable()
    {
        Time.timeScale = 0f;

        StartCoroutine(InitializeUI());
    }

    private void OnDisable()
    {
        GameManager.Instance.controls.SetActive(true);
        Time.timeScale = 1f;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void IncreaseMusicVolume()
    {
        MusicManager.Instance.IncreaseMusicVolume();
        musicLevelText.SetText(MusicManager.Instance.musicVolume.ToString());
    }

    public void DecreaseMusicVolume()
    {
        MusicManager.Instance.DecreaseMusicVolume();
        musicLevelText.SetText(MusicManager.Instance.musicVolume.ToString());
    }

    public void IncreaseSoundsVolume()
    {
        SoundsEffectManager.Instance.IncreaseSoundsVolume();
        soundsLevelText.SetText(SoundsEffectManager.Instance.soundsVolume.ToString());
    }

    public void DecreaseSoundsVolume()
    {
        SoundsEffectManager.Instance.DecreaseSoundsVolume();
        soundsLevelText.SetText(SoundsEffectManager.Instance.soundsVolume.ToString());
    }

    private void ShowLevelName()
    {
        levelNumberText.text = GameManager.Instance.GetCurrentLevelNumber();
        levelNameText.text = GameManager.Instance.GetCurrentLevelName();
    }
}
