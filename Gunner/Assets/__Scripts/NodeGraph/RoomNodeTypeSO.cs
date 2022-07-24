using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomNodeType_", menuName = "Scriptable Objects/Dungeon/Room Node Type")]
public class RoomNodeTypeSO : ScriptableObject
{
    public string roomNodeTypeName;

    #region Header
    [Header("Only flag the RoomNodeTypes that should be visible in the editor")]
    #endregion
    public bool displayInNodeGraphEditor = true;
    #region Header
    [Header("One type should be corridor")]
    #endregion
    public bool isCorridor;
    #region Header
    [Header("One type should be corridorNS")]
    #endregion
    public bool isCorridorNS;
    #region Header
    [Header("One type should be corridorEW")]
    #endregion
    public bool isCorridorEW;
    #region Header
    [Header("One type should be Entrance")]
    #endregion
    public bool isEntrance;
    #region Header
    [Header("One type should be a Boss Room")]
    #endregion
    public bool isBossRoom;
    #region Header
    [Header("One type should be None (Unsigned)")]
    #endregion
    public bool isNone;

    #region Validate
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(roomNodeTypeName), roomNodeTypeName);
    }
#endif
    #endregion
}
