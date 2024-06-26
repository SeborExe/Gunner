using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Threading.Tasks;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using System;

[DisallowMultipleComponent]
public class GameManager : SingletonMonobehaviour<GameManager>
{
    [Header("Dungeon Levels")]

    [SerializeField] private List<DungeonLevelSO> dungeonLevelList;
    [SerializeField] private int currentDungeonLevelListIndex = 0;
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] HealthUI healthUI;
    private Room currentRoom;
    private Room previousRoom;
    private PlayerDetailsSO playerDetails;
    private Player player;
    private GameLevel gameLevel;

    public GameState gameState;
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
    public HoldingItemButton holdingItemButton;
    public PauseButton pauseButton;
    public Button mapExitButton;
    public FinishLevelButton finishLevelButton;

    private long gameScore;
    private int scoreMultiplier;
    private InstantiatedRoom bossRoom;
    private bool isFading = false;

    private float timer;

    //Usable Items
    private float usableItemCoolDownTimer;
    [HideInInspector] public bool usableItemCoolDownActive = false;
    [HideInInspector] public float usableItemCoolDownTime;
    [HideInInspector] public bool canUseUsableItem = true;
    [HideInInspector] public UsableItem usableItemThatWasUsed;
    public Dictionary<UsableItem, int> usableItemsThatPlayerHad = new Dictionary<UsableItem, int>();

    //Holding Items
    private float holdingItemCoolDownTimer;
    [HideInInspector] public bool holdingItemCoolDownActive = false;
    [HideInInspector] public bool canUseHoldingItem = true;

    [Header("Camera")]
    public CinemachineShake virtualCamera;

    [Header("Items Inpact")]
    public GameObject controls;

    [Header("Visual Effects")]
    public GameObject diseaseEffect;
    public GameObject hellEffect;

    [Header("LootLocker")]
    private Leaderboard leaderboard;

    [Header("Damage Text")]
    public GameObject damageText;

    protected override void Awake()
    {
        base.Awake();

        gameLevel = GameLevelManager.Instance.GetGameLevel();
        playerDetails = GameResources.Instance.currentPlayer.playerDetails;

        InstantiatePlayer();
    }
    private async void Start()
    {
        leaderboard = FindObjectOfType<Leaderboard>();

        previousGameState = GameState.gameStarted;
        gameState = GameState.gameStarted;

        gameScore = 0;
        scoreMultiplier = 1;

        await Fade(0f, 1f, 0f, Color.black);

        SetPlayerStatsBasedOnGameLevel();
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
        UpdateTimers();
    }

    private void UpdateTimers()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;

            if (timer < 0) timer = 0f;
        }

        if (usableItemCoolDownTimer > 0)
        {
            usableItemCoolDownTimer -= Time.deltaTime;

            UsableItemUI.Instance.SetFill(1f, usableItemCoolDownTimer / usableItemCoolDownTime);

            if (usableItemCoolDownTimer < 0 && usableItemCoolDownActive)
            {
                usableItemCoolDownTimer = 0f;
                usableItemThatWasUsed.ActiveAfterCoolDownTimerEndCount();
                SoundsEffectManager.Instance.PlaySoundEffect(GameResources.Instance.weaponPickup);
                UsableItemUI.Instance.SetFill(GetPlayer().GetCurrentUsableItem().chargingPoints, 0);
                canUseUsableItem = true;
            }
        }

        if (holdingItemCoolDownTimer > 0)
        {
            holdingItemCoolDownTimer -= Time.deltaTime;

            if (holdingItemCoolDownTimer < 0 && holdingItemCoolDownActive)
            {
                holdingItemCoolDownTimer = 0f;
                GetPlayer().lastHoldingItem.ActiveAfterCoolDownTimerEndCount();
                SoundsEffectManager.Instance.PlaySoundEffect(GameResources.Instance.weaponPickup);
                canUseHoldingItem = true;
            }
        }
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

        scoreMultiplier = Mathf.Clamp(scoreMultiplier, 1, GetmaxMultiplierValue());
        StaticEventHandler.CallScoreChangedEvent(gameScore, scoreMultiplier);
    }

    private void Player_OnDestroyed(DestroyedEvent destroyedEvent, DestroyEventArgs destroyEventArgs)
    {
        previousGameState = gameState;
        gameState = GameState.gameLost;
    }

    private int GetmaxMultiplierValue()
    {
        int maxMultiplier = 1;
        if (gameLevel == GameLevel.Easy)
        {
            maxMultiplier = 20;
        }
        else if (gameLevel == GameLevel.Hard)
        {
            maxMultiplier = 30;
        }

        return maxMultiplier;
    }

    private async void HandleGameState()
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
                await LevelComplete();
                break;

            case GameState.gameWon:
                if (previousGameState != GameState.gameWon)
                {
                    StopAllCoroutines();
                    await GameWon();
                }
                break;

            case GameState.gameLost:
                if (previousGameState != GameState.gameLost)
                {
                    StopAllCoroutines();
                    await GameLost();
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

    private async void RoomEnemiesDefeated()
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
            await BossStage();
        }

        await Task.Yield(); 
    }

    private async Task BossStage()
    {
        bossRoom.gameObject.SetActive(true);
        bossRoom.UnlockDoors(0f);
        await Task.Delay(2000);

        await Fade(0f, 1f, 2f, new Color(0f, 0f, 0f, 0.4f));

        await DisplayMessageRoutine("WELL DONE " + GameResources.Instance.currentPlayer.playerName + "! YOU'VE " +
            "SURVIVED... SO FAR...\n NOW FIND AND DEFEAT THE BOSS... GOOD LUCK...", Color.white, 5f);

        await Fade(1f, 0f, 2f, new Color(0f, 0f, 0f, 0.4f));
    }

    private async Task LevelComplete()
    {
        gameState = GameState.playingLevel;

        if (currentDungeonLevelListIndex == 6)
        {
            finishLevelButton.GetComponentInChildren<TMP_Text>().text = "FINISH";
        }

        finishLevelButton.gameObject.SetActive(true);

        await Task.Delay(2000);

        await Fade(0f, 1f, 2f, new Color(0f, 0f, 0f, 0.4f));

        await DisplayMessageRoutine("WELL DONE " + GameResources.Instance.currentPlayer.playerName + "! YOU'VE " +
            "SURVIVED THIS DUNGEON LEVEL!", Color.white, 5f);

        await DisplayMessageRoutine("COLLECT YOUR REWARD FROM THE BOX AND PRESS\n'COMPLETE BUTTON' TO ADVANCE TO THE NEXT LEVEL",
            Color.white, 5f);

        await Fade(1f, 0f, 2f, new Color(0f, 0f, 0f, 0.4f));

        while (!finishLevelButton.finishLevelButtonPressed)
        {
            await Task.Yield();
        }

        await Task.Yield();

        usableItemsThatPlayerHad.Clear();
        if (GetPlayer().GetCurrentUsableItem() != null)
        {
            usableItemsThatPlayerHad.Add(GetPlayer().GetCurrentUsableItem(), GetPlayer().GetCurrentChargingPoints());
        }

        UnlockAchievement();

        //Increase max player health
        GetPlayer().playerStats.SetAdditionalHealth(10);
        StatsDisplayUI.Instance.UpdateStatsUI();

        SetRank(playerDetails.playerPrefab.name, currentDungeonLevelListIndex + 1);
        currentDungeonLevelListIndex++;
        finishLevelButton.gameObject.SetActive(false);
        PlayDungeonLevel(currentDungeonLevelListIndex);
    }

    private void UnlockAchievement()
    {
        if (PlayerPrefs.GetInt("FirstBoss", 0) == 0)
        {
            Social.ReportProgress("CgkI4fCvip0QEAIQAQ", 100.0f, (bool success) =>
            {
                if (success)
                {
                    Social.ShowAchievementsUI();
                    PlayerPrefs.SetInt("FirstBoss", 1);
                }
                else
                {
                    return;
                }
            });
        }
    }

    public async Task Fade(float startFadeAlpha, float targetFadeAlpha, float fadeSeconds, Color backgroundColor)
    {
        isFading = true;

        if (canvasGroup != null)
        {
            Image image = canvasGroup.GetComponent<Image>();
            image.color = backgroundColor;
        }

        float time = 0f;

        while (time <= fadeSeconds)
        {
            time += Time.deltaTime;

            if (canvasGroup != null)
                canvasGroup.alpha = Mathf.Lerp(startFadeAlpha, targetFadeAlpha, time / fadeSeconds);

            await Task.Yield();
        }

        isFading = false;
    }

    public IEnumerator FadeCoroutine(float startFadeAlpha, float targetFadeAlpha, float fadeSeconds, Color backgroundColor)
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

    private async Task GameWon()
    {
        previousGameState = GameState.gameWon;

        GetPlayer().playerControl.DisablePlayer();
        GetPlayer().health.AddHealth(100);

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

        await Task.Delay(1000);

        SetRank(playerDetails.playerPrefab.name, 7);

        await Fade(0f, 1f, 2f, Color.black);

        await DisplayMessageRoutine("WELL DONE " + GameResources.Instance.currentPlayer.playerName + "! YOU'VE " +
            "DEFEATED THE DUNGEON!", Color.white, 3f);

        await DisplayMessageRoutine("YOU SCORED: " + gameScore.ToString("###,###0") + "\n\n" + rankText, Color.white, 4f);

        await DisplayMessageRoutine("TAP TO BACK TO MENU", Color.white, 0f);

        await AddToGlobalLeaderBoard();

        gameState = GameState.restartGame;
    }

    private async Task AddToGlobalLeaderBoard()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable) { return; }

        await leaderboard.SubmitScoreRoutine(GameResources.Instance.currentPlayer.playerName, (int)gameScore,
            $"LEVEL {currentDungeonLevelListIndex + 1} - " + $"{GetCurrentDungeonLevel().levelName.ToUpper()}");
    }

    private async Task GameLost()
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

        await Task.Delay(1000);

        await Fade(0f, 1f, 2f, Color.black);

        Enemy[] enemyArray = GameObject.FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemyArray)
        {
            enemy.gameObject.SetActive(false);
        }

        await DisplayMessageRoutine("BAD LUCK " + GameResources.Instance.currentPlayer.playerName +
            "... THIS DUNGEON HAS BECOME YOUR GRAVE", Color.white, 2f);

        await DisplayMessageRoutine("YOU SCORED: " + gameScore.ToString("###,###0") + "\n\n" + rankText, Color.white, 4f);

        while (!Input.GetMouseButtonDown(0))
        {
            await DisplayMessageRoutine("TAP TO BACK TO MENU", Color.white, 0f);
        }

        await AddToGlobalLeaderBoard();

        gameState = GameState.restartGame;
    }

    private void RestartGame()
    {
        SceneManager.LoadScene("MainMenuScene");
        leaderboard.Login();
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

        if (name == "TheKnight")
        {
            int actualRank = Rank.GetRank("knight");
            if (level > actualRank)
            {
                Rank.SetRank("knight", level);
            }
        }
    }

    private async void PlayDungeonLevel(int dungeonLevelListIndex)
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

        await DisplayDungeonLevelText();
    }

    private async Task DisplayDungeonLevelText()
    {
        await Fade(0f, 1f, 0f, Color.black);
        GetPlayer().playerControl.DisablePlayer();

        string messageText = "LEVEL: " + (currentDungeonLevelListIndex + 1).ToString() + "\n\n" + dungeonLevelList[
            currentDungeonLevelListIndex].levelName.ToUpper();

        await DisplayMessageRoutine(messageText, Color.white, 2f);

        GetPlayer().playerControl.EnablePlayer();
        await Fade(1f, 0f, 2f, Color.black);
    }

    private async Task DisplayMessageRoutine(string text, Color textColor, float displaySeconds)
    {
        messageText.SetText(text);
        messageText.color = textColor;

        if (displaySeconds > 0f)
        {
            float timer = displaySeconds;

            while (timer > 0f)
            {
                timer -= Time.deltaTime;
                await Task.Yield();
            }
        }

        await Task.Yield();
        messageText.SetText("");
    }

    private void SetPlayerStatsBasedOnGameLevel()
    {
        if (gameLevel == GameLevel.Easy)
        {
            GetPlayer().playerStats.SetAdditionalHealth(60);
            GetPlayer().SetPlayerNewHealth();
            GetPlayer().health.AddHealth(60);

            GetPlayer().playerStats.SetBaseDamage(50);
            StatsDisplayUI.Instance.UpdateStatsUI();
        }
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
        //if (isFading) return;

        DungeonMap.Instance.DisplayDungeonOverviewMap();
    }

    public float GetTimer()
    {
        return timer;
    }

    public void SetTimer(float time)
    {
        timer = time;
    }

    public float GetCoolDownTimer()
    {
        return usableItemCoolDownTimer;
    }

    public void SetCoolDownTimer(float time)
    {
        usableItemCoolDownTimer = time;
    }

    public float GetHoldingItemCooldownTimer()
    {
        return holdingItemCoolDownTimer;
    }

    public void SetHoldingItemCooldownTimer(float time)
    {
        holdingItemCoolDownTimer = time;
    }

    public void StartImmortalityRoutine(float time, SpriteRenderer spriteRenderer)
    {
        StartCoroutine(player.health.ImmortalityRoutine(time, spriteRenderer));
    }

    public Leaderboard GetLeaderboard()
    {
        return leaderboard;
    }

    public void ClearItemText()
    {
        foreach (RectTransform child in GetPlayer().itemTextSpawner.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public string GetCurrentLevelName()
    {
        return GetCurrentDungeonLevel().levelName.ToUpper();
    }

    public string GetCurrentLevelNumber()
    {
        return $"LEVEL {currentDungeonLevelListIndex + 1}";
    }

    public HealthUI GetHealthUI()
    {
        return healthUI;
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
