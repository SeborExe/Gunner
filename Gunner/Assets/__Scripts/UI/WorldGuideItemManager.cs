using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class WorldGuideItemManager : MonoBehaviour
{
    [SerializeField] WorldGuideRecord worldGuidePrefab;

    [SerializeField] List<Item> itemList = new List<Item>();
    [SerializeField] List<Item> usableItemList = new List<Item>();
    [SerializeField] List<Item> holdingItemList = new List<Item>();

    [SerializeField] Transform itemListTransformPivot;
    [SerializeField] Transform usableItemListTransformPivot;
    [SerializeField] Transform holdingItemListTransformPivot;

    private async void OnEnable()
    {
        await CreateAllRecords();
    }

    private async Task CreateAllRecords()
    {
        foreach (Item item in itemList)
        {
            WorldGuideRecord record = Instantiate(worldGuidePrefab, itemListTransformPivot);
            record.SetUp(item.itemID, item.itemGuideDescription, item.itemSprite);
        }

        foreach (Item item in usableItemList)
        {
            WorldGuideRecord record = Instantiate(worldGuidePrefab, usableItemListTransformPivot);
            record.SetUp(item.itemID, item.itemGuideDescription, item.itemSprite);
        }

        foreach (Item item in holdingItemList)
        {
            WorldGuideRecord record = Instantiate(worldGuidePrefab, holdingItemListTransformPivot);
            record.SetUp(item.itemID, item.itemGuideDescription, item.itemSprite);
        }

        await Task.Yield();
    }
}
