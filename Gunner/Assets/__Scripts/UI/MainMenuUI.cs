using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] GameObject playButton;
    [SerializeField] GameObject highScoreButton;
    [SerializeField] GameObject returnToMainMenuButton;
    [SerializeField] GameObject quitButton;
    [SerializeField] GameObject instructionsButton;

    private bool isHighScoresSceneLoaded = false;
    private bool isInstructionsSceneLoaded = false;

    private void Start()
    {
        MusicManager.Instance.PlayMusic(GameResources.Instance.mainMenuMusic, 0f, 2f);

        SceneManager.LoadScene("CharacterSelectorScene", LoadSceneMode.Additive);

        returnToMainMenuButton.SetActive(false);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("MainGameScene");
    }

    public void LoadHighScores()
    {
        playButton.SetActive(false);
        instructionsButton.SetActive(false);
        highScoreButton.SetActive(false);
        isHighScoresSceneLoaded = true;

        SceneManager.UnloadSceneAsync("CharacterSelectorScene");
        returnToMainMenuButton.SetActive(true);
        SceneManager.LoadScene("HighScoreScene", LoadSceneMode.Additive);
    }

    public void LoadInstructions()
    {
        playButton.SetActive(false);
        instructionsButton.SetActive(false);
        highScoreButton.SetActive(false);
        isInstructionsSceneLoaded = true;

        SceneManager.UnloadSceneAsync("CharacterSelectorScene");
        returnToMainMenuButton.SetActive(true);
        SceneManager.LoadScene("InstructionsScene", LoadSceneMode.Additive);
    }

    public void LoadCharacterSelector()
    {
        returnToMainMenuButton.SetActive(false);

        if (isHighScoresSceneLoaded)
        {
            SceneManager.UnloadSceneAsync("HighScoreScene");
            isHighScoresSceneLoaded = false;
        }
        else if (isInstructionsSceneLoaded)
        {
            SceneManager.UnloadSceneAsync("InstructionsScene");
            isInstructionsSceneLoaded = false;
        }

        playButton.SetActive(true);
        highScoreButton.SetActive(true);
        instructionsButton.SetActive(true);

        SceneManager.LoadScene("CharacterSelectorScene", LoadSceneMode.Additive);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
