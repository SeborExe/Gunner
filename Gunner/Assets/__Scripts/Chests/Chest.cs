using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(MaterializeEffect))]
public class Chest : MonoBehaviour, IUseable
{
    [SerializeField, ColorUsage(false, true)] private Color materializeColor;
    [SerializeField] private float materializeTime = 3f;
    [SerializeField] private Transform itemSpawnPoint;

    private int healthPercent;
    private WeaponDetailsSO weaponDetails;
    private int ammoPercent;
    private Item item;

    private MaterializeEffect materializeEffect;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isEnabled = false;
    private ChestState chestState = ChestState.closed;
    private GameObject chestItemGameObject;
    private ChestItem chestItem;
    private TextMeshPro messageTextTMP;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        materializeEffect = GetComponent<MaterializeEffect>();
        messageTextTMP = GetComponentInChildren<TextMeshPro>();
    }

    public void Initialize(bool shouldMaterialize, int healthPercent, WeaponDetailsSO weaponDetails, int ammoPercent, Item item)
    {
        this.healthPercent = healthPercent;
        this.weaponDetails = weaponDetails;
        this.ammoPercent = ammoPercent;
        this.item = item;

        if (shouldMaterialize)
        {
            StartCoroutine(MaterializeChest());
        }
        else
        {
            EnableChest();
        }
    }

    private IEnumerator MaterializeChest()
    {
        SpriteRenderer[] spriteRendererArray = new SpriteRenderer[] { spriteRenderer };

        yield return StartCoroutine(materializeEffect.MaterializeRoutine(GameResources.Instance.materializeShader, materializeColor,
            materializeTime, spriteRendererArray, GameResources.Instance.litMaterial));

        EnableChest();
    }

    private void EnableChest()
    {
        isEnabled = true;
    }

    public void UseItem()
    {
        if (!isEnabled) return;

        switch (chestState)
        {
            case ChestState.closed:
                OpenChest();
                break;

            case ChestState.healthItem:
                CollectHealthItem();
                break;

            case ChestState.ammoItem:
                CollectAmmoItem();
                break;

            case ChestState.weaponItem:
                CollectWeaponItem();
                break;

            case ChestState.Item:
                CollectItem();
                break;

            case ChestState.empty:
                return;

            default:
                return;
        }
    }

    private void OpenChest()
    {
        animator.SetBool(Settings.use, true);

        SoundsEffectManager.Instance.PlaySoundEffect(GameResources.Instance.chestOpen);

        if (weaponDetails != null)
        {
            if (GameManager.Instance.GetPlayer().IsWeaponHeldByPlayer(weaponDetails))
                weaponDetails = null;
        }

        UpdateChestState();
    }

    private void UpdateChestState()
    {
        if (healthPercent != 0)
        {
            chestState = ChestState.healthItem;
            InstantiateHealthItem();
        }

        else if (ammoPercent != 0)
        {
            chestState = ChestState.ammoItem;
            InstantiateAmmoItem();
        }

        else if (weaponDetails != null)
        {
            chestState = ChestState.weaponItem;
            InstantiateWeaponItem();
        }

        else if (item != null)
        {
            chestState = ChestState.Item;
            InstantiateBonusItem();
        }
        else
        {
            chestState = ChestState.weaponItem;
        }
    }

    private void InstantiateItem()
    {
        chestItemGameObject = Instantiate(GameResources.Instance.chestItemPrefab, this.transform);
        chestItem = chestItemGameObject.GetComponent<ChestItem>();
    }

    private void InstantiateHealthItem()
    {
        InstantiateItem();

        chestItem.Initialize(GameResources.Instance.heartIcon, healthPercent.ToString() + "%", itemSpawnPoint.position, materializeColor);
    }

    private void InstantiateAmmoItem()
    {
        InstantiateItem();

        chestItem.Initialize(GameResources.Instance.bulletIcon, ammoPercent.ToString() + "%", itemSpawnPoint.position, materializeColor);
    }

    private void InstantiateWeaponItem()
    {
        InstantiateItem();

        chestItemGameObject.GetComponent<ChestItem>().Initialize(weaponDetails.weaponSprite, weaponDetails.weaponName, itemSpawnPoint.position,
            materializeColor);
    }

    private void InstantiateBonusItem()
    {
        InstantiateItem();

        chestItemGameObject.GetComponent<ChestItem>().Initialize(item.itemSprite, item.itemName, itemSpawnPoint.position,
            materializeColor);
    }

    private void InstantiateBonusItem(UsableItem item)
    {
        InstantiateItem();

        chestItemGameObject.GetComponent<ChestItem>().Initialize(item.itemSprite, item.itemName, itemSpawnPoint.position,
            materializeColor);
    }

    private void CollectHealthItem()
    {
        if (chestItem == null || !chestItem.isItemMaterialized) return;

        GameManager.Instance.GetPlayer().health.AddHealth(healthPercent);
        SoundsEffectManager.Instance.PlaySoundEffect(GameResources.Instance.healthPickup);

        healthPercent = 0;
        Destroy(chestItemGameObject);
        UpdateChestState();
    }

    private void CollectAmmoItem()
    {
        if (chestItem == null || !chestItem.isItemMaterialized) return;

        Player player = GameManager.Instance.GetPlayer();
        player.reloadWeaponEvent.CallReloadWeaponEvent(player.activeWeapon.GetCurrentWeapon(), ammoPercent);
        SoundsEffectManager.Instance.PlaySoundEffect(GameResources.Instance.ammoPickup);

        ammoPercent = 0;
        Destroy(chestItemGameObject);
        UpdateChestState();
    }

    private void CollectWeaponItem()
    {
        if (chestItem == null || !chestItem.isItemMaterialized) return;

        if (!GameManager.Instance.GetPlayer().IsWeaponHeldByPlayer(weaponDetails))
        {
            GameManager.Instance.GetPlayer().AddWeaponToPlayer(weaponDetails);
            SoundsEffectManager.Instance.PlaySoundEffect(GameResources.Instance.weaponPickup);
        }

        else
        {
            StartCoroutine(DisplayMessage("WEAPON\nALREADY\nEQUIPPED", 5f));
        }

        weaponDetails = null;
        Destroy(chestItemGameObject);
        UpdateChestState();
    }

    private void CollectItem()
    {
        if (chestItem == null || !chestItem.isItemMaterialized) return;

        foreach (ItemEffect effect in item.effects)
        {
            effect.ActiveEffect();
            SoundsEffectManager.Instance.PlaySoundEffect(GameResources.Instance.healthPickup);
        }

        if (!item.isUsable)
        {
            item.AddImage();

            item = null;
            Destroy(chestItemGameObject);
            UpdateChestState();
        }
        else
        {
            if (GameManager.Instance.GetPlayer().GetCurrentUsableItem() != null)
            {
                SoundsEffectManager.Instance.PlaySoundEffect(GameResources.Instance.healthPickup);

                UsableItem playerUsableItem = GameManager.Instance.GetPlayer().GetCurrentUsableItem();
                UsableItem chestUsableItem = (UsableItem)item;

                GameManager.Instance.GetPlayer().SetCurrentUsableItem(chestUsableItem);
                item = null;
                Destroy(chestItemGameObject);

                item = playerUsableItem;
                InstantiateBonusItem(playerUsableItem);

                UsableItemUI.Instance.OnItemCollected();

                if (chestUsableItem != GameManager.Instance.GetPlayer().lastUsableItem)
                {
                    UsableItemUI.Instance.SetFill(chestUsableItem.chargingPoints, chestUsableItem.chargingPoints);
                    GameManager.Instance.GetPlayer().SetCurrentChargingPointsAfterUse(chestUsableItem.chargingPoints);
                }
                else
                {
                    UsableItemUI.Instance.SetFill(chestUsableItem.chargingPoints, Mathf.Min(
                        chestUsableItem.chargingPoints, GameManager.Instance.GetPlayer().GetCurrentChargingPoints()));
                }

                GameManager.Instance.GetPlayer().lastUsableItem = playerUsableItem;
            }
            else
            {
                UsableItem chestUsableItem = (UsableItem)item;
                GameManager.Instance.GetPlayer().lastUsableItem = chestUsableItem;

                UsableItemUI.Instance.SetFill(chestUsableItem.chargingPoints, chestUsableItem.chargingPoints);
                item = null;
                Destroy(chestItemGameObject);
                UpdateChestState();
            }
        }
    }

    private IEnumerator DisplayMessage(string messageText, float messageDisplayTime)
    {
        messageTextTMP.text = messageText;

        yield return new WaitForSeconds(messageDisplayTime);

        messageTextTMP.text = "";
    }
}
