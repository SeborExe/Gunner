using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
 
[CreateAssetMenu(fileName = "Room_", menuName = "Scriptable Objects/Dungeon/Room")]
public class RoomTemplateSO : ScriptableObject
{
    [HideInInspector] public string guid;

    #region ROOM PREFAB

    [Space(10)]
    [Header("ROOM PREFAB")]

    #endregion
    public GameObject prefab;

    [HideInInspector] public GameObject previousPrefab; //This is used to regenerate the guid if the so is copied and the prfab is changed

    #region ROOM CONFIGURATION

    [Space(10)]
    [Header("ROOM CONFIGURATION")]

    #endregion
    public RoomNodeTypeSO roomNodeType;

    public Vector2Int lowerBounds;
    public Vector2Int upperBounds;

    [SerializeField]
    public List<Doorway> doorwayList;

    public Vector2Int[] spawnPositionArray;

    public List<Doorway> GetDoorwayList()
    {
        return doorwayList;
    }

    #region Validation
#if UNITY_EDITOR

    private void OnValidate()
    {
        if (guid == "" || previousPrefab != prefab)
        {
            guid = GUID.Generate().ToString();
            previousPrefab = prefab;
            EditorUtility.SetDirty(this);
        }

        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(doorwayList), doorwayList);
        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(spawnPositionArray), spawnPositionArray);
    }

#endif
    #endregion
}
