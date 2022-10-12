using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoldingItemUI : SingletonMonobehaviour<HoldingItemUI>
{
    [SerializeField] Image holdingItemImage;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        if (GameManager.Instance.GetPlayer().GetCurrentHoldingItem() == null)
        {
            holdingItemImage.enabled = false;
        }
        else
        {
            OnItemCollected();
        }
    }

    public void OnItemCollected()
    {
        if (holdingItemImage.enabled == false) holdingItemImage.enabled = true;

        HoldingItem item = GameManager.Instance.GetPlayer().GetCurrentHoldingItem();
        holdingItemImage.sprite = item.itemSprite;
    }

    public void DisableHoldingItemImageState()
    {
        holdingItemImage.enabled = false;
    }
}
