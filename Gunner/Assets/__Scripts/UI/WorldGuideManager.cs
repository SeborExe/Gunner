using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldGuideManager : MonoBehaviour
{
    [SerializeField] Button itemButton;
    [SerializeField] Button weaponButton;
    [SerializeField] Button enemyButton;

    [Header("Item Options")]
    [SerializeField] GameObject ItemGuide;
    [SerializeField] GameObject ItemOptionsBar;
    [SerializeField] Button itemButtonOptions;
    [SerializeField] Button usableItemButtonOptions;
    [SerializeField] Button holdingItemButtonOptions;
    [SerializeField] GameObject ItemContent;
    [SerializeField] GameObject UsableItemContent;
    [SerializeField] GameObject HoldingItemContent;

    [Header("Weapons Options")]
    [SerializeField] GameObject WeaponGuide;
    [SerializeField] GameObject WeaponsOptionsBar;
    [SerializeField] Button gunsButtonOptions;
    [SerializeField] Button meleeButtonOptions;
    [SerializeField] GameObject GunsContent;
    [SerializeField] GameObject MeleeContent;

    [Header("Enemies Options")]
    [SerializeField] GameObject EnemiesGuide;
    [SerializeField] GameObject EnemiesOptionsBar;
    [SerializeField] Button HedusaButtonOptions;
    [SerializeField] Button SlimeblockButtonOptions;
    [SerializeField] Button OrcButtonOptions;
    [SerializeField] Button GrimonkButtonOptions;
    [SerializeField] Button MudrockButtonOptions;
    [SerializeField] Button SlizzardButtonOptions;
    [SerializeField] Button BossButtonOptions;
    [SerializeField] GameObject HedusaContent;
    [SerializeField] GameObject SlimeblockContent;
    [SerializeField] GameObject OrcContent;
    [SerializeField] GameObject GrimonkContent;
    [SerializeField] GameObject MudrockContent;
    [SerializeField] GameObject SlizzardContent;
    [SerializeField] GameObject BossContent;

    private GameObject currentGuideContent;
    private GameObject currentOptionsBar;
    private GameObject currentPageOptions;

    private void Start()
    {
        OpenNewMenu(ItemOptionsBar, ItemContent, ItemGuide, itemButtonOptions);
    }

    private void OnEnable()
    {
        itemButton.onClick.AddListener(() => OpenNewMenu(ItemOptionsBar, ItemContent, ItemGuide, itemButtonOptions));
        weaponButton.onClick.AddListener(() => OpenNewMenu(WeaponsOptionsBar, GunsContent, WeaponGuide, gunsButtonOptions));
        enemyButton.onClick.AddListener(() => OpenNewMenu(EnemiesOptionsBar, HedusaContent, EnemiesGuide, HedusaButtonOptions));

        itemButtonOptions.onClick.AddListener(() => OpenNewOptionsMenu(ItemContent, itemButtonOptions));
        usableItemButtonOptions.onClick.AddListener(() => OpenNewOptionsMenu(UsableItemContent, usableItemButtonOptions));
        holdingItemButtonOptions.onClick.AddListener(() => OpenNewOptionsMenu(HoldingItemContent, holdingItemButtonOptions));

        gunsButtonOptions.onClick.AddListener(() => OpenNewOptionsMenu(GunsContent, gunsButtonOptions));
        meleeButtonOptions.onClick.AddListener(() => OpenNewOptionsMenu(MeleeContent, meleeButtonOptions));

        HedusaButtonOptions.onClick.AddListener(() => OpenNewOptionsMenu(HedusaContent, HedusaButtonOptions));
        SlimeblockButtonOptions.onClick.AddListener(() => OpenNewOptionsMenu(SlimeblockContent, SlimeblockButtonOptions));
        OrcButtonOptions.onClick.AddListener(() => OpenNewOptionsMenu(OrcContent, OrcButtonOptions));
        GrimonkButtonOptions.onClick.AddListener(() => OpenNewOptionsMenu(GrimonkContent, GrimonkButtonOptions));
        MudrockButtonOptions.onClick.AddListener(() => OpenNewOptionsMenu(MudrockContent, MudrockButtonOptions));
        SlizzardButtonOptions.onClick.AddListener(() => OpenNewOptionsMenu(SlizzardContent, SlizzardButtonOptions));
        BossButtonOptions.onClick.AddListener(() => OpenNewOptionsMenu(BossContent, BossButtonOptions));
    }

    private void OnDisable()
    {
        itemButton.onClick.RemoveListener(() => OpenNewMenu(ItemOptionsBar, ItemContent, ItemGuide, itemButtonOptions));
        weaponButton.onClick.RemoveListener(() => OpenNewMenu(WeaponsOptionsBar, GunsContent, WeaponGuide, gunsButtonOptions));
        enemyButton.onClick.RemoveListener(() => OpenNewMenu(EnemiesOptionsBar, HedusaContent, EnemiesGuide, HedusaButtonOptions));

        itemButtonOptions.onClick.RemoveListener(() => OpenNewOptionsMenu(ItemContent, itemButtonOptions));
        usableItemButtonOptions.onClick.RemoveListener(() => OpenNewOptionsMenu(UsableItemContent, usableItemButtonOptions));
        holdingItemButtonOptions.onClick.RemoveListener(() => OpenNewOptionsMenu(HoldingItemContent, holdingItemButtonOptions));

        gunsButtonOptions.onClick.RemoveListener(() => OpenNewOptionsMenu(GunsContent, gunsButtonOptions));
        meleeButtonOptions.onClick.RemoveListener(() => OpenNewOptionsMenu(MeleeContent, meleeButtonOptions));

        HedusaButtonOptions.onClick.RemoveListener(() => OpenNewOptionsMenu(HedusaContent, HedusaButtonOptions));
        SlimeblockButtonOptions.onClick.RemoveListener(() => OpenNewOptionsMenu(SlimeblockContent, SlimeblockButtonOptions));
        OrcButtonOptions.onClick.RemoveListener(() => OpenNewOptionsMenu(OrcContent, OrcButtonOptions));
        GrimonkButtonOptions.onClick.RemoveListener(() => OpenNewOptionsMenu(GrimonkContent, GrimonkButtonOptions));
        MudrockButtonOptions.onClick.RemoveListener(() => OpenNewOptionsMenu(MudrockContent, MudrockButtonOptions));
        SlizzardButtonOptions.onClick.RemoveListener(() => OpenNewOptionsMenu(SlizzardContent, SlizzardButtonOptions));
        BossButtonOptions.onClick.RemoveListener(() => OpenNewOptionsMenu(BossContent, BossButtonOptions));
    }

    //Open new Bar with options
    private void OpenNewMenu(GameObject options, GameObject startContent, GameObject mainContent, Button startOptionButton)
    {
        if (currentOptionsBar != null)
            currentOptionsBar.SetActive(false);

        currentOptionsBar = options;
        options.SetActive(true);
        OpenNewOptionsMenu(startContent, startOptionButton);

        if (currentGuideContent != null)
            currentGuideContent.SetActive(false);

        currentGuideContent = mainContent;
        mainContent.SetActive(true);

        startOptionButton.Select();
    }

    //Open new page in guide
    private void OpenNewOptionsMenu(GameObject options, Button startOptionButton)
    {
        if (currentPageOptions != null)
            currentPageOptions.gameObject.SetActive(false);

        currentPageOptions = options;
        options.SetActive(true);

        startOptionButton.Select();
    }
}
