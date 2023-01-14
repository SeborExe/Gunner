using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDetails_", menuName = "Scriptable Objects/Items/Item Details")]
public class Item : ScriptableObject
{
    [Header("Item Base Details")]
    public string itemName;
    public Sprite itemSprite;
    [Multiline(4)] public string itemText;
    public bool isUsable = false;

    [Header("Item configuration")]
    public ItemEffect[] effects;

    [Header("World Guide"), Space(15)]
    public string itemID;
    [Multiline(9)] public string itemGuideDescription;

    public void AddImage()
    {
        StatsDisplayUI.Instance.AddItemIcon(itemSprite);
    }
}
