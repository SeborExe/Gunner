using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Settings
{
    #region Units
    public const float pixelPerUnit = 16f;
    public const float tileSizePixels = 16f;
    #endregion

    #region Dungeon build settings
    public const int maxDungeonRebuildAttemptsForRoomGraph = 1000;
    public const int maxDungeonBuildAttempts = 10;
    #endregion

    #region Room Settings

    public const int maxChildCorridors = 3;
    public const float fadeInTime = 0.5f;
    public const float doorUnlockDelay = 1f;

    #endregion

    #region Animator parameters

    public static int aimUp = Animator.StringToHash("aimUp");
    public static int aimDown = Animator.StringToHash("aimDown");
    public static int aimUpRight = Animator.StringToHash("aimUpRight");
    public static int aimUpLeft = Animator.StringToHash("aimUpLeft");
    public static int aimRight = Animator.StringToHash("aimRight");
    public static int aimLeft = Animator.StringToHash("aimLeft");
    public static int isIdle = Animator.StringToHash("isIdle");
    public static int isMoving = Animator.StringToHash("isMoving");
    public static int rollUp = Animator.StringToHash("rollUp");
    public static int rollRight = Animator.StringToHash("rollRight");
    public static int rollLeft = Animator.StringToHash("rollLeft");
    public static int rollDown = Animator.StringToHash("rollDown");

    public static int flipUp = Animator.StringToHash("flipUp");
    public static int flipRight = Animator.StringToHash("flipRight");
    public static int flipLeft = Animator.StringToHash("flipLeft");
    public static int flipDown = Animator.StringToHash("flipDown");
    public static int use = Animator.StringToHash("use");

    public static float baseSpeedForPlayerAnimations = 8f;
    public static float baseSpeedForEnemyAnimations = 3f;

    public static int open = Animator.StringToHash("open");

    public static int destroy = Animator.StringToHash("destroy");
    public static string stateDestroyed = "Destroyed";
    #endregion

    #region GameObject tags

    public const string playerTag = "Player";
    public const string playerWeapon = "playerWeapon";

    #endregion

    #region Audio

    public const float musicFadeOutTime = 0.5f;
    public const float musicFadeInTime = 0.5f;

    #endregion

    #region Firing control

    public const float useAimAngleDistance = 3.5f;

    #endregion

    #region PathFinding

    public const int defaultAStarMovementPenalty = 40;
    public const int prefferedPathAStarMovementPenalty = 1;
    public const float playerMoveDistanceToRebuildPath = 3f;
    public const float enemyPathRebuildCooldown = 2f;
    public const int targetFrameRateToSpreadPathfindingOver = 60;

    #endregion

    #region UI

    public const float uiAmmoIconSpacing = 4f;
    public const float uiHearthSpacing = 16f;

    #endregion

    #region Enemy parameters

    public const int defaultEnemyHealth = 30;
    public const float contactDamageCollisionRestartDelay = 0.5f;

    #endregion
}
