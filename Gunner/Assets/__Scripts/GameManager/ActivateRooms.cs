using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ActivateRooms : MonoBehaviour
{
    [SerializeField] private Camera miniMapCamera;

    private Camera cameraMain;

    private void Start()
    {
        cameraMain = Camera.main;

        InvokeRepeating("EnableRooms", 0.5f, 0.75f);
    }

    private void EnableRooms()
    {
        if (GameManager.Instance.gameState == GameState.dungeonOverviewMap) return;

        HelperUtilities.CameraWorldPositionBounds(out Vector2Int miniMapCameraWorldPositionLowerBounds, out Vector2Int
         minimapCameraWorldPositionUpperBounds, miniMapCamera);

        HelperUtilities.CameraWorldPositionBounds(out Vector2Int mainCameraWorldPositionLowerBounds, out Vector2Int
         mainCameraWorldPositionUpperBounds, cameraMain);

        foreach (KeyValuePair<string, Room> keyValuePair in DungeonBuilder.Instance.dungeonBuilderRoomDictionary)
        {
            Room room = keyValuePair.Value;

            if ((room.lowerBounds.x <= minimapCameraWorldPositionUpperBounds.x && room.lowerBounds.y <= minimapCameraWorldPositionUpperBounds.y)
                && (room.upperBounds.x >= miniMapCameraWorldPositionLowerBounds.x && room.upperBounds.y >= miniMapCameraWorldPositionLowerBounds.y))
            {
                room.instantiatedRoom.gameObject.SetActive(true);

                if ((room.lowerBounds.x <= mainCameraWorldPositionUpperBounds.x && room.lowerBounds.y <= mainCameraWorldPositionUpperBounds.y)
                    && (room.upperBounds.x >= mainCameraWorldPositionLowerBounds.x && room.upperBounds.y >= mainCameraWorldPositionLowerBounds.y))
                {
                    room.instantiatedRoom.ActiveEnviromentGameObjects();
                }
                else
                {
                    room.instantiatedRoom.DeactivateEnviromentRoomGameObjects();
                }
            }
            else
            {
                room.instantiatedRoom.gameObject.SetActive(false);
            }
        }
    }
}
