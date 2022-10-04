using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffect : ScriptableObject, IItemEffect
{
    public virtual void ActiveEffect()
    {
        StatsDisplayUI.Instance.UpdateStatsUI();
    }
}
