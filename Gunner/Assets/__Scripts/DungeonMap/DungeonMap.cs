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
