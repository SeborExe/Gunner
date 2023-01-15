using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsDisplayUI : SingletonMonobehaviour<StatsDisplayUI>
{
    [SerializeField] TMP_Text damageText;
    [SerializeField] TMP_Text fireRateText;
    [SerializeField] TMP_Text healthText;
    [SerializeField] TMP_Text movementSpeedText;
    [SerializeField] TMP_Text rangeText;
    [SerializeField] TMP_Text ammoSpeedText;
    [SerializeField] TMP_Text weaponReloadSpeedText;
    [SerializeField] Transform itemIconContainer;
    [SerializeField] Image itemIconPrefab;

    [Header("Current Usable Item"), Space(10)]
    [SerializeField] Image usableItemIcon;
    [SerializeField] TMP_Text usableItemDescription;

    Player player;

    public event Action OnRefreshStatsUI;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        OnRefreshStatsUI += InitializeUI;
        InitializeUI();
    }

    private void OnEnable()
    {
        player = GameManager.Instance.GetPlayer();
        UpdateCurrentUsableItemInPauseMenu();
    }

    private void InitializeUI()
    {
        ShowDamage();
        ShowFireRate();
        ShowMaxHealth();
        ShowMovementSpeed();
        ShowRange();
        ShowAmmoSpeed();
        ShowWeaponReloadSpeed();
    }

    public void UpdateCurrentUsableItemInPauseMenu()
    {
        UsableItem usableItem = player.GetCurrentUsableItem();
        if (usableItem != null)
        {
            usableItemIcon.gameObject.SetActive(true);
            usableItemIcon.sprite = usableItem.itemSprite;

            usableItemDescription.text = usableItem.itemGuideDescription;
        }
        else
        {
            usableItemIcon.gameObject.SetActive(false);
            usableItemDescription.text = "";
        }
    }

    public void UpdateStatsUI()
    {
        OnRefreshStatsUI?.Invoke();
    }

    private void ShowDamage()
    {
        float damage = player.playerStats.GetBaseDamage();
        damageText.text = $"{damage}%";
    }

    private void ShowFireRate()
    {
        float fireRate = player.playerStats.GetAdditionalFireRate();
        fireRateText.text = $"{fireRate}%";
    }

    private void ShowMaxHealth()
    {
        float maxHealth = player.health.GetStartingHealth();
        healthText.text = maxHealth.ToString();
    }

    private void ShowMovementSpeed()
    {
        float movementSpeed = player.playerControl.GetMovementSpeed();
        movementSpeedText.text = movementSpeed.ToString();
    }

    private void ShowRange()
    {
        float range = player.playerStats.GetAdditionalAmmoRange();
        rangeText.text = range.ToString();
    }

    private void ShowAmmoSpeed()
    {
        int ammoSpeed = player.playerStats.GetAdditionalAmmoSpeed();
        ammoSpeedText.text = ammoSpeed.ToString();
    }

    private void ShowWeaponReloadSpeed()
    {
        float reloadSpeed = player.playerStats.GetAdditionalWeaponReloadSpeed();
        weaponReloadSpeedText.text = $"{reloadSpeed}%";
    }

    public void AddItemIcon(Sprite itemIcon)
    {
        Image iconPrefab = Instantiate(itemIconPrefab, itemIconContainer);
        iconPrefab.sprite = itemIcon;
    }
}
