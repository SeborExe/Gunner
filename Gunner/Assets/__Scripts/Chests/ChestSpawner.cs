using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestSpawner : MonoBehaviour
{
    [System.Serializable]
    private struct RangeByLevel
    {
        public DungeonLevelSO dungeonLevel;
        [Range(0, 100)] public int min;
        [Range(0, 100)] public int max;
    }

    [SerializeField] private GameObject chestPrefab;

    [Header("Chest Spawn Chance")]
    [SerializeField, Range(0, 100)] private int chestSpawnChanceMin;
    [SerializeField, Range(0, 100)] private int chestSpawnChanceMax;
    [SerializeField] private List<RangeByLevel> chestSpawnChanceByLevelList;

    [Header("Chest Spawn Details")]
    [SerializeField] private ChestSpawnEvent chestSpawnEvent;
    [SerializeField] private ChestSpawnPosition chestSpawnPosition;
    [SerializeField, Range(0, 4)] private int numberOfItemsToSpawnMin;
    [SerializeField, Range(0, 4)] private int numberOfItemsToSpawnMax;

    [Header("Chest Content Details")]
    [SerializeField] private List<SpawnableObjectByLevel<WeaponDetailsSO>> weaponSpawnByLevelList;
    [SerializeField] private List<RangeByLevel> healthSpawnByLevelList;
    [SerializeField] private List<RangeByLevel> ammoSpawnByLevelList;
    [SerializeField] private List<SpawnableObjectByLevel<Item>> itemSpawnByLevelList;
    [SerializeField] private List<SpawnableObjectByLevel<HoldingItem>> holdingItemsList;

    private bool chestSpawned = false;
    private Room chestRoom;

    private void OnEnable()
    {
        StaticEventHandler.OnRoomChanged += StaticEventHandler_OnRoomChanged;

        StaticEventHandler.OnRoomEnemiesDefeated += StaticEventHandler_OnRoomEnemiesDefeated;
    }

    private void OnDisable()
    {
        StaticEventHandler.OnRoomChanged -= StaticEventHandler_OnRoomChanged;

        StaticEventHandler.OnRoomEnemiesDefeated -= StaticEventHandler_OnRoomEnemiesDefeated;
    }

    private void StaticEventHandler_OnRoomChanged(RoomChangedEventArgs roomChangedEventArgs)
    {
        if (chestRoom == null)
        {
            chestRoom = GetComponentInParent<InstantiatedRoom>().room;
        }

        if (!chestSpawned && chestSpawnEvent == ChestSpawnEvent.onRoomEntry && chestRoom == roomChangedEventArgs.room)
        {
            SpawnChest();
        }
    }

    private void StaticEventHandler_OnRoomEnemiesDefeated(RoomEnemiesDefeatedArgs roomEnemiesDefeatedArgs)
    {
        if (chestRoom == null)
        {
            chestRoom = GetComponentInParent<InstantiatedRoom>().room;
        }

        if (!chestSpawned && chestSpawnEvent == ChestSpawnEvent.onEnemiesDefeated && chestRoom == roomEnemiesDefeatedArgs.room)
        {
            SpawnChest();
        }
    }

    private void SpawnChest()
    {
        chestSpawned = true;

        if (!RandomSpawnedChest()) return;

        GetItemToSpawn(out int ammoNum, out int healthNum, out int weaponNum, out int itemNum, out int holdingItemNum);

        GameObject chestGameObject = Instantiate(chestPrefab, this.transform);

        if (chestSpawnPosition == ChestSpawnPosition.atSpawnerPosition)
        {
            chestGameObject.transform.position = this.transform.position;
        }

        else if (chestSpawnPosition == ChestSpawnPosition.atPlayerPosition)
        {
            Vector3 spawnPosition = HelperUtilities.GetSpawnPositionNearestToPlayer(GameManager.Instance.GetPlayer().transform.position);
            Vector3 variation = new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.5f, 0.5f), 0);

            chestGameObject.transform.position = spawnPosition + variation;
        }

        Chest chest = chestGameObject.GetComponent<Chest>();

        if (chestSpawnEvent == ChestSpawnEvent.onRoomEntry)
        {
            chest.Initialize(false, GetHealthPercentToSpawn(healthNum), GetWeaponDetailsToSpawn(weaponNum),
                GetAmmoPercentToSpawn(ammoNum), GetItemToSpawn(itemNum), GetHoldingItemToSpawn(holdingItemNum));
        }
        else
        {
            chest.Initialize(true, GetHealthPercentToSpawn(healthNum), GetWeaponDetailsToSpawn(weaponNum),
                GetAmmoPercentToSpawn(ammoNum), GetItemToSpawn(itemNum), GetHoldingItemToSpawn(holdingItemNum));
        }
    }

    private bool RandomSpawnedChest()
    {
        int chancePercent = UnityEngine.Random.Range(chestSpawnChanceMin, chestSpawnChanceMax + 1);

        foreach (RangeByLevel rangeByLevel in chestSpawnChanceByLevelList)
        {
            if (rangeByLevel.dungeonLevel == GameManager.Instance.GetCurrentDungeonLevel())
            {
                chancePercent = UnityEngine.Random.Range(rangeByLevel.min, rangeByLevel.max);
                break;
            }
        }

        int randomPercent = UnityEngine.Random.Range(1, 100 + 1);

        if (randomPercent <= chancePercent)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void GetItemToSpawn(out int ammo, out int health, out int weapons, out int items, out int holdingItem)
    {
        ammo = 0;
        health = 0;
        weapons = 0;
        items = 0;
        holdingItem = 0;

        int numberOfItemsToSpawn = UnityEngine.Random.Range(numberOfItemsToSpawnMin, numberOfItemsToSpawnMax);
        int choice;

        if (numberOfItemsToSpawn == 1)
        {
            choice = UnityEngine.Random.Range(0, 4);
            if (choice == 0) { weapons++; return; }
            if (choice == 1) { ammo++; return; }
            if (choice == 2) { health++; return; }
            if (choice == 3) { holdingItem++; return; }
            return;
        }

        else if (numberOfItemsToSpawn == 2)
        {
            choice = UnityEngine.Random.Range(0, 5);
            if (choice == 0) { weapons++; ammo++; return; }
            if (choice == 1) { ammo++; health++; return; }
            if (choice == 2) { health++; weapons++; return; }
            if (choice == 3) { health++; holdingItem++; return; }
            if (choice == 4) { ammo++; holdingItem++; return; }
            return;
        }

        else if (numberOfItemsToSpawn == 3)
        {
            choice = UnityEngine.Random.Range(0, 7);
            if (choice == 0) { health++; ammo++; weapons++; return; }
            if (choice == 1) { health++; ammo++; weapons++; return; }
            if (choice == 2) { health++; ammo++; weapons++; return; }
            if (choice == 3) { ammo++; health++; items++; return; }
            if (choice == 4) { ammo++; health++; items++; return; }
            if (choice == 5) { ammo++; health++; items++; return; }
            if (choice == 6) { ammo++; health++; holdingItem++; return; }
            return;
        }

        else if (numberOfItemsToSpawn >= 4)
        {
            weapons++;
            ammo++;
            health++;
            items++;
            return;
        }
    }

    private int GetHealthPercentToSpawn(int healthNumber)
    {
        if (healthNumber == 0) return 0;

        foreach (RangeByLevel spawnPercentByLevel in healthSpawnByLevelList)
        {
            if (spawnPercentByLevel.dungeonLevel == GameManager.Instance.GetCurrentDungeonLevel())
            {
                return UnityEngine.Random.Range(spawnPercentByLevel.min, spawnPercentByLevel.max);
            }
        }

        return 0;
    }

    private WeaponDetailsSO GetWeaponDetailsToSpawn(int weaponNum)
    {
        if (weaponNum == 0) return null;

        RandomSpawnableObject<WeaponDetailsSO> weaponRandom = new RandomSpawnableObject<WeaponDetailsSO>(weaponSpawnByLevelList);

        WeaponDetailsSO weaponDetails = weaponRandom.GetItem();
        return weaponDetails;
    }

    private Item GetItemToSpawn(int itemNum)
    {
        if (itemNum == 0) return null;

        RandomSpawnableObject<Item> itemRandom = new RandomSpawnableObject<Item>(itemSpawnByLevelList);

        Item itemDetails = itemRandom.GetItem();
        return itemDetails;
    }

    private HoldingItem GetHoldingItemToSpawn(int itemNum)
    {
        if (itemNum == 0) return null;

        RandomSpawnableObject<HoldingItem> itemRandom = new RandomSpawnableObject<HoldingItem>(holdingItemsList);

        HoldingItem holdingItemDetails = itemRandom.GetItem();
        return holdingItemDetails;
    }

    private int GetAmmoPercentToSpawn(int ammoNumber)
    {
        if (ammoNumber == 0) return 0;

        foreach (RangeByLevel spawnPercentByLevel in ammoSpawnByLevelList)
        {
            if (spawnPercentByLevel.dungeonLevel == GameManager.Instance.GetCurrentDungeonLevel())
            {
                return UnityEngine.Random.Range(spawnPercentByLevel.min, spawnPercentByLevel.max);
            }
        }

        return 0;
    }
}
