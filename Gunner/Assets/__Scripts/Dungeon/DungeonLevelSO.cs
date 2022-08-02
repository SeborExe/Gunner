using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonLevel_", menuName = "Scriptable Objects/Dungeon/Dungeon Level")]
public class DungeonLevelSO : ScriptableObject
{
    [Header("Basic level details")]
    public string levelName;

    [Header("Room templates for level")]
    public List<RoomTemplateSO> roomTemplateList;

    [Header("Room node graphs for level")]
    public List<RoomNodeGraphSO> roomNodeGraphList;

    #region Validation
#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(levelName), levelName);

        if (HelperUtilities.ValidateCheckEnumerableValues(this, nameof(roomTemplateList), roomTemplateList)) { return; }
        if (HelperUtilities.ValidateCheckEnumerableValues(this, nameof(roomNodeGraphList), roomNodeGraphList)) { return; }

        bool isEWCorridor = false;
        bool isNSCorridor = false;
        bool isEntrance = false;

        foreach (RoomTemplateSO roomTemplateSO in roomTemplateList)
        {
            if (roomTemplateSO == null) { return; }

            if (roomTemplateSO.roomNodeType.isCorridorEW) { isEWCorridor = true; }
            if (roomTemplateSO.roomNodeType.isCorridorNS) { isNSCorridor = true; }
            if (roomTemplateSO.roomNodeType.isEntrance) { isEntrance = true; }
        }

        if (!isEWCorridor)
        {
            Debug.Log("In " + this.name + " No E/W corridor room type specified");
        }

        if (!isNSCorridor)
        {
            Debug.Log("In " + this.name + " No N/S corridor room type specified");
        }

        if (!isEntrance)
        {
            Debug.Log("In " + this.name + " No Entrance room type specified");
        }

        foreach (RoomNodeGraphSO roomNodeGraph in roomNodeGraphList)
        {
            if (roomNodeGraph == null) { return; }

            foreach (RoomNodeSO roomNodeSO in roomNodeGraph.roomNodeList)
            {
                if (roomNodeSO == null) { return; }

                if (roomNodeSO.roomNodeType.isEntrance || roomNodeSO.roomNodeType.isCorridorEW || roomNodeSO.roomNodeType.isCorridorNS ||
                    roomNodeSO.roomNodeType.isCorridor || roomNodeSO.roomNodeType.isNone)
                {
                    continue;
                }

                bool isRoomNodeTypeFound = false;

                foreach (RoomTemplateSO roomTemplateSO in roomTemplateList)
                {
                    if (roomTemplateSO == null) { return; }

                    if (roomTemplateSO.roomNodeType == roomNodeSO.roomNodeType)
                    {
                        isRoomNodeTypeFound = true;
                        break;
                    }
                }

                if (!isRoomNodeTypeFound)
                {
                    Debug.Log("In " + this.name + " : No Room template " + roomNodeSO.roomNodeType.name + " found for node graph " +
                        roomNodeGraph.name);
                }
            }
        }
    }

#endif
    #endregion
}
