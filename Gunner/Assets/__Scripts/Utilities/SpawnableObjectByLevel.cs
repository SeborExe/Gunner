using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnableObjectByLevel<T>
{
    public DungeonLevelSO dungeonLevel;
    public List<SpawnableObjectRatio<T>> spawnableObjectRatioList;
}
