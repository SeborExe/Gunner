using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class DungeonMap : SingletonMonobehaviour<DungeonMap>
{
    [SerializeField] private GameObject minimapUI;

    private Camera dungeonCamera;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;

        Transform playerTransform = GameManager.Instance.GetPlayer().transform;

        CinemachineVirtualCamera cinemachineVirtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        cinemachineVirtualCamera.Follow = playerTransform;

        dungeonCamera = GetComponentInChildren<Camera>();
        dungeonCamera.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && GameManager.Instance.gameState == GameState.dungeonOverviewMap)
        {
            GetRoomClicked();   
        }
    }

    private void GetRoomClicked()
    {
        Vector3 worldPosition = dungeonCamera.ScreenToWorldPoint(Input.mousePosition);
        worldPosition = new Vector3(worldPosition.x, worldPosition.y, 0f);

        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(new Vector2(worldPosition.x, worldPosition.y), 1f);

        foreach (Collider2D collider2D in collider2DArray)
        {
            if (collider2D.GetComponent<InstantiatedRoom>() != null)
            {
                InstantiatedRoom instantiatedRoom = collider2D.GetComponent<InstantiatedRoom>();

                if (instantiatedRoom.room.isClearedOfEnemies && instantiatedRoom.room.isPreviouslyVisited)
                {
                    StartCoroutine(MovePlayerToRoom(worldPosition, instantiatedRoom.room));
                }
            }
        }
    }

    private IEnumerator MovePlayerToRoom(Vector3 worldPosition, Room room)
    {
        StaticEventHandler.CallRoomChangedEvent(room);

        yield return StartCoroutine(GameManager.Instance.Fade(0f, 1f, 0f, Color.black));

        ClearDungeonOverviewMap();

        GameManager.Instance.GetPlayer().playerControl.DisablePlayer();

        Vector3 spawnPosition = HelperUtilities.GetSpawnPositionNearestToPlayer(worldPosition);

        GameManager.Instance.GetPlayer().transform.position = spawnPosition;
        GameManager.Instance.minimapButton.SetMapActiveFalse();

        yield return StartCoroutine(GameManager.Instance.Fade(1f, 0f, 1f, Color.black));

        GameManager.Instance.GetPlayer().playerControl.EnablePlayer();
    }

    public void DisplayDungeonOverviewMap()
    {
        GameManager.Instance.previousGameState = GameManager.Instance.gameState;
        GameManager.Instance.gameState = GameState.dungeonOverviewMap;

        GameManager.Instance.GetPlayer().playerControl.DisablePlayer();

        mainCamera.gameObject.SetActive(false);
        dungeonCamera.gameObject.SetActive(true);

        ActivateRoomsForDisplay();

        minimapUI.SetActive(false);
    }

    public void ClearDungeonOverviewMap()
    {
        GameManager.Instance.gameState = GameManager.Instance.previousGameState;
        GameManager.Instance.previousGameState = GameState.dungeonOverviewMap;

        GameManager.Instance.GetPlayer().playerControl.EnablePlayer();

        mainCamera.gameObject.SetActive(true);
        dungeonCamera.gameObject.SetActive(false);

        minimapUI.SetActive(true);
    }

    private void ActivateRoomsForDisplay()
    {
        foreach (KeyValuePair<string, Room> keyValuePair in DungeonBuilder.Instance.dungeonBuilderRoomDictionary)
        {
            Room room = keyValuePair.Value;
            room.instantiatedRoom.gameObject.SetActive(true);
        }
    }
}
