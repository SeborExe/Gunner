using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.SceneManagement;
using TMPro;

[DisallowMultipleComponent]
public class GameManager : SingletonMonobehaviour<GameManager>
{
    [Header("Dungeon Levels")]

    [SerializeField] private List<DungeonLevelSO> dungeonLevelList;
    [SerializeField] private int currentDungeonLevelListIndex = 0;
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject pauseMenu;
    private Room currentRoom;
    private Room previousRoom;
    private PlayerDetailsSO playerDetails;
    private Player player;

    [HideInInspector] public GameState gameState;
    [HideInInspector] public GameState previousGameState;

    [Header("Joysticks")]
    public Joystick joystick;
    public Joystick rotationJoystick;
    public Transform point;
    public WeaponChangeButton weaponChangeButton;
    public MinimapButton minimapButton;
    public ActionButton actionButton;
    public RollButton rollButton;
    public ReloadButton reloadButton;
    public UsableItemButton usableItemButton;
    public PauseButton pauseButton;
    public Button mapExitButton;
    public FinishLevelButton finishLevelButton;

    private long gameScore;
    private int scoreMultiplier;
    private InstantiatedRoom bossRoom;
    private bool isFading = false;

    [Header("Camera")]
    public CinemachineShake virtualCamera;

    [Header("Items Inpact")]
    public GameObject controls;

    protected override void Awake()
    {
        base.Awake();

        playerDetails = GameResources.Instance.currentPlayer.playerDetails;

        InstantiatePlayer();
    }
    private void Start()
    {
        previousGameState = GameState.gameStarted;
        gameState = GameState.gameStarted;

        gameScore = 0;
        scoreMultiplier = 1;

        StartCoroutine(Fade(0f, 1f, 0f, Color.black));
    }

    private void OnEnable()
    {
        StaticEventHandler.OnRoomChanged += StaticEventHandler_OnRoomChanged;
        StaticEventHandler.OnRoomEnemiesDefeated += StaticEventHandler_OnRoomEnemiesDefeated;
        StaticEventHandler.OnPointScored += StaticEventHandler_OnPointsScored;
        StaticEventHandler.OnMultiplier += StaticEventHandler_OnMultiplier;
        player.destroyedEvent.OnDestroyed += Player_OnDestroyed;
    }

    private void OnDisable()
    {
        StaticEventHandler.OnRoomChanged -= StaticEventHandler_OnRoomChanged;
        StaticEventHandler.OnRoomEnemiesDefeated -= StaticEventHandler_OnRoomEnemiesDefeated;
        StaticEventHandler.OnPointScored -= StaticEventHandler_OnPointsScored;
        StaticEventHandler.OnMultiplier -= StaticEventHandler_OnMultiplier;
        player.destroyedEvent.OnDestroyed -= Player_OnDestroyed;
    }

    private void Update()
    {
        HandleGameState();
    }

    private void InstantiatePlayer()
    {
        GameObject playerGameObject = Instantiate(playerDetails.playerPrefab);

        player = playerGameObject.GetComponent<Player>();
        player.Initialize(playerDetails);
    }

    private void StaticEventHandler_OnRoomChanged(RoomChangedEventArgs roomChangedEventArgs)
    {
        SetCurrentRoom(roomChangedEventArgs.room);
    }

    private void StaticEventHandler_OnRoomEnemiesDefeated(RoomEnemiesDefeatedArgs roomEnemiesDefeatedArgs)
    {
        RoomEnemiesDefeated();
    }

    private void StaticEventHandler_OnPointsScored(PointsScoredArgs pointsScoredArgs)
    {
        gameScore += pointsScoredArgs.points * scoreMultiplier;
        StaticEventHandler.CallScoreChangedEvent(gameScore, scoreMultiplier);
    }

    private void StaticEventHandler_OnMultiplier(MultiplierArgs multiplierArgs)
    {
        if (multiplierArgs.multiplier)
        {
            scoreMultiplier++;
        }
        else
        {
            scoreMultiplier--;
        }

        scoreMultiplier = Mathf.Clamp(scoreMultiplier, 1, 30);
        StaticEventHandler.CallScoreChangedEvent(gameScore, scoreMultiplier);
    }

    private void Player_OnDestroyed(DestroyedEvent destroyedEvent, DestroyEventArgs destroyEventArgs)
    {
        previousGameState = gameState;
        gameState = GameState.gameLost;
    }


    private void HandleGameState()
    {
        switch (gameState)
        {
            case GameState.gameStarted:
                PlayDungeonLevel(currentDungeonLevelListIndex);
                gameState = GameState.playingLevel;
                RoomEnemiesDefeated();
                break;

            case GameState.playingLevel:
                if (pauseButton.pauseButtonPressed)
                {
                    PauseGameMenu();
                }
                if (minimapButton.minimapButtonButtonPressed)
                {
                    DisplayDungeonOverviewMap();
                }
                break;

            case GameState.engagingEnemies:
                if (pauseButton.pauseButtonPressed)
                {
                    PauseGameMenu();
                }
                break;

            case GameState.dungeonOverviewMap:
                if (!minimapButton.minimapButtonButtonPressed)
                {
                    DungeonMap.Instance.ClearDungeonOverviewMap();
                }
                break;

            case GameState.bossStage:
                if (pauseButton.pauseButtonPressed)
                {
                    PauseGameMenu();
                }
                if (minimapButton.minimapButtonButtonPressed)
                {
                    DisplayDungeonOverviewMap();
                }
                break;

            case GameState.engagingBoss:
                if (pauseButton.pauseButtonPressed)
                {
                    PauseGameMenu();
                }
                break;

            case GameState.levelCompleted:
                StartCoroutine(LevelComplete());
                break;

            case GameState.gameWon:
                if (previousGameState != GameState.gameWon)
                    StartCoroutine(GameWon());
                break;

            case GameState.gameLost:
                if (previousGameState != GameState.gameLost)
                {
                    StopAllCoroutines();
                    StartCoroutine(GameLost());
                }
                break;

            case GameState.restartGame:
                RestartGame();
                break;

            case GameState.gamePaused:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    PauseGameMenu();
                }
                break;
        }
    }

    public void PauseGameMenu()
    {
        if (gameState != GameState.gamePaused) 
        {
            pauseMenu.SetActive(true);
            GetPlayer().playerControl.DisablePlayer();

            previousGameState = gameState;
            gameState = GameState.gamePaused;
        }

        else if (gameState == GameState.gamePaused)
        {
            pauseMenu.SetActive(false);
            GetPlayer().playerControl.EnablePlayer();

            gameState = previousGameState;
            previousGameState = GameState.gamePaused;
        }
    }

    public void SetCurrentRoom(Room room)
    {
        previousRoom = currentRoom;
        currentRoom = room;
    }

    private void RoomEnemiesDefeated()
    {
        bool isDungeonClearOfRegularEnemies = true;
        bossRoom = null;

        foreach (KeyValuePair<string, Room> keyValuePair in DungeonBuilder.Instance.dungeonBuilderRoomDictionary)
        {
            if (keyValuePair.Value.roomNodeType.isBossRoom)
            {
                bossRoom = keyValuePair.Value.instantiatedRoom;
                continue;
            }

            if (!keyValuePair.Value.isClearedOfEnemies)
            {
                isDungeonClearOfRegularEnemies = false;
                break;
            }
        }

        if ((isDungeonClearOfRegularEnemies && bossRoom == null) || (isDungeonClearOfRegularEnemies && bossRoom.room.isClearedOfEnemies))
        {
            if (currentDungeonLevelListIndex < dungeonLevelList.Count - 1)
            {
                gameState = GameState.levelCompleted;
            }
            else
            {
                gameState = GameState.gameWon;
            }
        }

        else if (isDungeonClearOfRegularEnemies)
        {
            gameState = GameState.bossStage;
            StartCoroutine(BossStage());
        }
    }

    private IEnumerator BossStage()
    {
        bossRoom.gameObject.SetActive(true);
        bossRoom.UnlockDoors(0f);
        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(Fade(0f, 1f, 2f, new Color(0f, 0f, 0f, 0.4f)));

        yield return StartCoroutine(DisplayMessageRoutine("WELL DONE " + GameResources.Instance.currentPlayer.playerName + "! YOU'VE " +
            "SURVIVED... SO FAR...\n NOW FIND AND DEFEAT THE BOSS... GOOD LUCK...", Color.white, 5f));

        yield return StartCoroutine(Fade(1f, 0f, 2f, new Color(0f, 0f, 0f, 0.4f)));
    }

    private IEnumerator LevelComplete()
    {
        gameState = GameState.playingLevel;

        if (currentDungeonLevelListIndex == 5)
        {
            finishLevelButton.GetComponentInChildren<TMP_Text>().text = "FINISH";
        }

        finishLevelButton.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(Fade(0f, 1f, 2f, new Color(0f, 0f, 0f, 0.4f)));

        yield return StartCoroutine(DisplayMessageRoutine("WELL DONE " + GameResources.Instance.currentPlayer.playerName + "! YOU'VE " +
            "SURVIVED THIS DUNGEON LEVEL!", Color.white, 5f));

        yield return StartCoroutine(DisplayMessageRoutine("COLLECT YOUR REWARD FROM THE BOX AND PRESS\n'COMPLETE BUTTON' TO ADVANCE TO THE NEXT LEVEL",
            Color.white, 5f));

        yield return StartCoroutine(Fade(1f, 0f, 2f, new Color(0f, 0f, 0f, 0.4f)));

        while (!finishLevelButton.finishLevelButtonPressed)
        {
            yield return null;
        }

        yield return null;

        SetRank(playerDetails.playerPrefab.name, currentDungeonLevelListIndex + 1);
        currentDungeonLevelListIndex++;
        finishLevelButton.gameObject.SetActive(false);
        PlayDungeonLevel(currentDungeonLevelListIndex);
    }

    public IEnumerator Fade(float startFadeAlpha, float targetFadeAlpha, float fadeSeconds, Color backgroundColor)
    {
        isFading = true;
        Image image = canvasGroup.GetComponent<Image>();
        image.color = backgroundColor;

        float time = 0f;

        while (time <= fadeSeconds)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startFadeAlpha, targetFadeAlpha, time / fadeSeconds);
            yield return null;
        }

        isFading = false;
    }

    private IEnumerator GameWon()
    {
        previousGameState = GameState.gameWon;

        GetPlayer().playerControl.DisablePlayer();

        int rank = HighScoreManager.Instance.GetRank(gameScore);
        string rankText;

        if (rank > 0 && rank <= Settings.numberOfHighScoresToSava)
        {
            rankText = $"YOUR SCORE IS RANKED {rank.ToString("#0")} IN THE TOP {Settings.numberOfHighScoresToSava.ToString("#0")}";
            string name = GameResources.Instance.currentPlayer.playerName;

            if (name =="")
            {
                name = playerDetails.playerCharacterName.ToUpper();
            }

            HighScoreManager.Instance.AddScore(new Score()
            {
                playerName = name,
                levelDescription = $"LEVEL {currentDungeonLevelListIndex + 1} - " +
                $"{GetCurrentDungeonLevel().levelName.ToUpper()}",
                playerScore = gameScore
            }, rank);
        }
        else
        {
            rankText = $"YOUR SCORE ISN'T RANKED IN THE TOP {Settings.numberOfHighScoresToSava.ToString("#0")}";
        }

        yield return new WaitForSeconds(1f);

        SetRank(playerDetails.playerPrefab.name, 6);

        yield return StartCoroutine(Fade(0f, 1f, 2f, Color.black));

        yield return StartCoroutine(DisplayMessageRoutine("WELL DONE " + GameResources.Instance.currentPlayer.playerName + "! YOU'VE " +
            "DEFEATED THE DUNGEON!", Color.white, 3f));

        yield return StartCoroutine(DisplayMessageRoutine("YOU SCORED: " + gameScore.ToString("###,###0") + "\n\n" + rankText, Color.white, 4f));

        yield return StartCoroutine(DisplayMessageRoutine("TAP TO BACK TO MENU", Color.white, 0f));

        gameState = GameState.restartGame;
    }

    private IEnumerator GameLost()
    {
        previousGameState = GameState.gameLost;

        GetPlayer().playerControl.DisablePlayer();

        int rank = HighScoreManager.Instance.GetRank(gameScore);
        string rankText;

        if (rank > 0 && rank <= Settings.numberOfHighScoresToSava)
        {
            rankText = $"YOUR SCORE IS RANKED {rank.ToString("#0")} IN THE TOP {Settings.numberOfHighScoresToSava.ToString("#0")}";
            string name = GameResources.Instance.currentPlayer.playerName;

            if (name == "")
            {
                name = playerDetails.playerCharacterName.ToUpper();
            }

            HighScoreManager.Instance.AddScore(new Score()
            {
                playerName = name,
                levelDescription = $"LEVEL {currentDungeonLevelListIndex + 1} - " +
                $"{GetCurrentDungeonLevel().levelName.ToUpper()}",
                playerScore = gameScore
            }, rank);
        }
        else
        {
            rankText = $"YOUR SCORE ISN'T RANKED IN THE TOP {Settings.numberOfHighScoresToSava.ToString("#0")}";
        }

        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(Fade(0f, 1f, 2f, Color.black));

        Enemy[] enemyArray = GameObject.FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemyArray)
        {
            enemy.gameObject.SetActive(false);
        }

        yield return StartCoroutine(DisplayMessageRoutine("BAD LUCK " + GameResources.Instance.currentPlayer.playerName +
            "... THIS DUNGEON HAS BECOME YOUR GRAVE", Color.white, 2f));

        yield return StartCoroutine(DisplayMessageRoutine("YOU SCORED: " + gameScore.ToString("###,###0") + "\n\n" + rankText, Color.white, 4f));

        while (!Input.GetMouseButtonDown(0))
        {
            yield return StartCoroutine(DisplayMessageRoutine("TAP TO BACK TO MENU", Color.white, 0f));
        }

        gameState = GameState.restartGame;
    }

    private void RestartGame()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    private void SetRank(string name, int level)
    {
        if (name == "TheGeneral")
        {
            int actualRank = Rank.GetRank("general");
            if (level > actualRank)
            {
                Rank.SetRank("general", level);
            }
        }

        if (name == "TheScientist")
        {
            int actualRank = Rank.GetRank("scientist");
            if (level > actualRank)
            {
                Rank.SetRank("scientist", level);
            }
        }

        if (name == "TheThief")
        {
            int actualRank = Rank.GetRank("thief");
            if (level > actualRank)
            {
                Rank.SetRank("thief", level);
            }
        }
    }

    private void PlayDungeonLevel(int dungeonLevelListIndex)
    {
        bool dungeonBuiltSuccessfully = DungeonBuilder.Instance.GenerateDungeon(dungeonLevelList[dungeonLevelListIndex]);

        if (!dungeonBuiltSuccessfully)
        {
            Debug.LogError("Coudn't build dungeon from specified room and node graphs");
        }

        StaticEventHandler.CallRoomChangedEvent(currentRoom);

        player.gameObject.transform.position = new Vector3((currentRoom.lowerBounds.x + currentRoom.upperBounds.x) / 2f, (currentRoom.lowerBounds.y +
            currentRoom.upperBounds.y) / 2f, 0f);

        player.gameObject.transform.position = HelperUtilities.GetSpawnPositionNearestToPlayer(player.gameObject.transform.position);

        StartCoroutine(DisplayDungeonLevelText());
    }

    private IEnumerator DisplayDungeonLevelText()
    {
        StartCoroutine(Fade(0f, 1f, 0f, Color.black));
        GetPlayer().playerControl.DisablePlayer();

        string messageText = "LEVEL: " + (currentDungeonLevelListIndex + 1).ToString() + "\n\n" + dungeonLevelList[
            currentDungeonLevelListIndex].levelName.ToUpper();

        yield return StartCoroutine(DisplayMessageRoutine(messageText, Color.white, 2f));

        GetPlayer().playerControl.EnablePlayer();
        yield return StartCoroutine(Fade(1f, 0f, 2f, Color.black));
    }

    private IEnumerator DisplayMessageRoutine(string text, Color textColor, float displaySeconds)
    {
        messageText.SetText(text);
        messageText.color = textColor;

        if (displaySeconds > 0f)
        {
            float timer = displaySeconds;

            while (timer > 0f)
            {
                timer -= Time.deltaTime;
                yield return null;
            }
        }

        yield return null;
        messageText.SetText("");
    }

    public Room GetCurrentRoom()
    {
        return currentRoom;
    }

    public Player GetPlayer()
    {
        return player;
    }

    public Sprite GetPlayerMinimapIcon()
    {
        return playerDetails.playerMiniMapIcon;
    }

    public DungeonLevelSO GetCurrentDungeonLevel()
    {
        return dungeonLevelList[currentDungeonLevelListIndex];
    }

    private void DisplayDungeonOverviewMap()
    {
        if (isFading) return;

        DungeonMap.Instance.DisplayDungeonOverviewMap();
    }

    #region Validation
#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(dungeonLevelList), dungeonLevelList);
    }

#endif
    #endregion
}
