using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDetails_", menuName = "Scriptable Objects/Items/Holding Item")]
public class HoldingItem : Item
{
    [Header("ItemType")]
    public ItemRank effectRank;
    [Multiline(4)] public string itemTextAfterUse;

    public virtual void Use()
    {
        foreach (ItemEffect effect in effects)
        {
            effect.ActiveEffect();
        }

        if (effectRank == ItemRank.Time)
        {
            GameManager.Instance.GetPlayer().lastHoldingItem = this;
        }

        if (!String.IsNullOrEmpty(itemTextAfterUse))
        {
            GameManager.Instance.GetPlayer().itemTextSpawner.Spawn(itemTextAfterUse);
        }

        PlaySound();
        GameManager.Instance.GetPlayer().SetCurrentHoldingItem(null);
        HoldingItemUI.Instance.DisableHoldingItemImageState();

        StatsDisplayUI.Instance.UpdateStatsUI();
    }

    private void PlaySound()
    {
        switch(effectRank)
        {
            case ItemRank.Positive:
                SoundsEffectManager.Instance.PlaySoundEffect(GameResources.Instance.healthPickup);
                break;

            case ItemRank.Neutral:
                SoundsEffectManager.Instance.PlaySoundEffect(GameResources.Instance.weaponPickup);
                break;

            case ItemRank.Negative:
                SoundsEffectManager.Instance.PlaySoundEffect(GameResources.Instance.tableFlip);
                break;

            case ItemRank.Time:
                SoundsEffectManager.Instance.PlaySoundEffect(GameResources.Instance.healthPickup);
                break;
        }
    }

    public virtual void ActiveAfterCoolDownTimerEndCount() { }
}
