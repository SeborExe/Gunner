using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private TMP_Text musicLevelText;
    [SerializeField] private TMP_Text soundsLevelText;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator InitializeUI()
    {
        yield return null;

        soundsLevelText.SetText(SoundsEffectManager.Instance.soundsVolume.ToString());
        musicLevelText.SetText(MusicManager.Instance.musicVolume.ToString());
    }

    private void OnEnable()
    {
        Time.timeScale = 0f;

        StartCoroutine(InitializeUI());
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
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
}
