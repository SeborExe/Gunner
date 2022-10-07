using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UsableItemUI : SingletonMonobehaviour<UsableItemUI>
{
    [SerializeField] Image usableItemImage;
    [SerializeField] Image fill;

    private void Start()
    {
        if (GameManager.Instance.GetPlayer().GetCurrentUsableItem() == null)
        {
            usableItemImage.enabled = false;
        }
        else
        {
            OnItemCollected();
        }
    }

    public void OnItemCollected()
    {
        if (usableItemImage.enabled == false) usableItemImage.enabled = true;

        UsableItem item = GameManager.Instance.GetPlayer().GetCurrentUsableItem();
        usableItemImage.sprite = item.itemSprite;
    }

    public void SetFill(int maxValue, int currentValue)
    {
        float fillScale = (float)currentValue / (float)maxValue;
        fill.transform.localScale = new Vector3(1, fillScale, 1);
    }
}
