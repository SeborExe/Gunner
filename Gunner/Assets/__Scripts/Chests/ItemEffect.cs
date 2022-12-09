using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffect : ScriptableObject, IItemEffect
{
    [SerializeField] string effectText;

    public virtual void ActiveEffect()
    {
        StatsDisplayUI.Instance.UpdateStatsUI();
    }

    public string GetEffectText()
    {
        return effectText;
    }
}
