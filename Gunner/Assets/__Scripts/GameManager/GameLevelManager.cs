using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelManager: SingletonMonobehaviour<GameLevelManager>
{
    private GameLevel level;

    protected override void Awake()
    {
        DontDestroyOnLoad(this);
        base.Awake();
    }

    public void SetGameLevel(GameLevel gameLevel)
    {
        level = gameLevel;
    }

    public GameLevel GetGameLevel()
    {
        return level;
    }
}
