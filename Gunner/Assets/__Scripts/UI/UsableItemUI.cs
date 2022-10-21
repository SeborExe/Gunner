using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UsableItemUI : SingletonMonobehaviour<UsableItemUI>
{
    [SerializeField] Image usableItemImage;
    [SerializeField] Image fill;

    [Header("Stripes")]
    [SerializeField] Transform rootObject;
    [SerializeField] GameObject stripPrefab;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        if (GameManager.Instance.GetPlayer().GetCurrentUsableItem() == null)
        {
            usableItemImage.enabled = false;
        }
        else
        {
            OnItemCollected();
            AddStripes();
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
        if (maxValue != 0)
        {
            float fillScale = (float)currentValue / (float)maxValue;
            fill.transform.localScale = new Vector3(1, fillScale, 1);
        }
        else
        {
            fill.transform.localScale = Vector3.one;
        }
    }

    public void SetFill(float maxValue, float currentValue)
    {
        float fillScale = currentValue / maxValue;
        fill.transform.localScale = new Vector3(1, fillScale, 1);
    }

    public void AddStripes()
    {
        foreach (Transform child in rootObject)
        {
            Destroy(child.gameObject);
        }

        int stripsCount = GameManager.Instance.GetPlayer().GetCurrentUsableItem().chargingPoints;
        float rootSize = rootObject.GetComponent<RectTransform>().sizeDelta.y;

        for (int i = 1; i < stripsCount; i++)
        {
            GameObject strip = Instantiate(stripPrefab, rootObject);
            strip.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, ((rootSize / stripsCount) * i) - rootSize / 2);
        }
    }
}
