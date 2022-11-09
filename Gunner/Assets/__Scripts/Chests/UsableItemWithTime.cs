using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDetails_", menuName = "Scriptable Objects/Items/Usable Item On Time")]
public class UsableItemWithTime : UsableItem
{
    public float time;
    public ItemEffect[] debuffEffects;

    public override void Use()
    {
        base.Use();

        GameManager.Instance.usableItemCoolDownTime = time;
        GameManager.Instance.usableItemCoolDownActive = true;
        GameManager.Instance.SetCoolDownTimer(time);
        GameManager.Instance.usableItemThatWasUsed = this;
        GameManager.Instance.canUseUsableItem = false;
    }

    public override void SetAfterUse()
    {
        StatsDisplayUI.Instance.UpdateStatsUI();

        GameManager.Instance.GetPlayer().SetCurrentChargingPointsAfterUse();
    }

    public override void ActiveAfterCoolDownTimerEndCount()
    {
        foreach (ItemEffect effect in debuffEffects)
        {
            effect.ActiveEffect();
        }

        StatsDisplayUI.Instance.UpdateStatsUI();
    }
}
