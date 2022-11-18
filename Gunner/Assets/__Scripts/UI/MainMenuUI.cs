using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] GameObject playButton;
    [SerializeField] GameObject highScoreButton;
    [SerializeField] GameObject returnToMainMenuButton;
    [SerializeField] GameObject quitButton;
    [SerializeField] GameObject instructionsButton;
    [SerializeField] GameObject highScoreButtonOnline;

    [Header("Loading Screen")]
    [SerializeField] GameObject loadingScreen;
    [SerializeField] Image fillBar;
    private float target;

    private bool isHighScoresSceneLoaded = false;
    private bool isHighScoresOnlineSceneLoaded = false;
    private bool isInstructionsSceneLoaded = false;

    private void Start()
    {
        MusicManager.Instance.PlayMusic(GameResources.Instance.mainMenuMusic, 0f, 2f);

        SceneManager.LoadScene("CharacterSelectorScene", LoadSceneMode.Additive);

        returnToMainMenuButton.SetActive(false);
    }

    public async void PlayGame()
    {
        target = 0;
        fillBar.fillAmount = 0;

        AsyncOperation scene = SceneManager.LoadSceneAsync("MainGameScene");
        scene.allowSceneActivation = false;

        loadingScreen.SetActive(true);

        do
        {
            await Task.Delay(100);
            target = scene.progress;

        } while (scene.progress < 0.9f);

        scene.allowSceneActivation = true;
        loadingScreen.SetActive(false);
    }

    private void Update()
    {
        fillBar.fillAmount = Mathf.MoveTowards(fillBar.fillAmount, target, 3 * Time.deltaTime);
    }

    public void LoadHighScores()
    {
        playButton.SetActive(false);
        instructionsButton.SetActive(false);
        highScoreButtonOnline.SetActive(false);
        highScoreButton.SetActive(false);
        isHighScoresSceneLoaded = true;

        SceneManager.UnloadSceneAsync("CharacterSelectorScene");
        returnToMainMenuButton.SetActive(true);
        SceneManager.LoadScene("HighScoreScene", LoadSceneMode.Additive);
    }

    public void LoadHighScoresOnline()
    {
        playButton.SetActive(false);
        instructionsButton.SetActive(false);
        highScoreButtonOnline.SetActive(false);
        highScoreButton.SetActive(false);
        isHighScoresOnlineSceneLoaded = true;

        SceneManager.UnloadSceneAsync("CharacterSelectorScene");
        returnToMainMenuButton.SetActive(true);
        SceneManager.LoadScene("HighScoreSceneOnline", LoadSceneMode.Additive);
    }

    public void LoadInstructions()
    {
        playButton.SetActive(false);
        instructionsButton.SetActive(false);
        highScoreButtonOnline.SetActive(false);
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
        else if (isHighScoresOnlineSceneLoaded)
        {
            SceneManager.UnloadSceneAsync("HighScoreSceneOnline");
            isHighScoresOnlineSceneLoaded = false;
        }

        playButton.SetActive(true);
        highScoreButton.SetActive(true);
        instructionsButton.SetActive(true);
        highScoreButtonOnline.SetActive(true);

        SceneManager.LoadScene("CharacterSelectorScene", LoadSceneMode.Additive);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
