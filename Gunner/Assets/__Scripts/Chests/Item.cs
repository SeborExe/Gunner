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
    public bool isUsable = false;

    [Header("Item configuration")]
    public ItemEffect[] effects;

    public void AddImage()
    {
        StatsDisplayUI.Instance.AddItemIcon(itemSprite);
    }
}
