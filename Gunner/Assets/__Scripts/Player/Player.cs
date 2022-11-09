using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

#region Require components
[RequireComponent(typeof(HealthEvent))]
[RequireComponent(typeof(DestroyedEvent))]
[RequireComponent(typeof(Destroyed))]
[RequireComponent(typeof(SortingGroup))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(IdleEvent))]
[RequireComponent(typeof(Idle))]
[RequireComponent(typeof(AimWeaponEvent))]
[RequireComponent(typeof(AimWeapon))]
[RequireComponent(typeof(PlayerControl))]
[RequireComponent(typeof(AnimatePlayer))]
[RequireComponent(typeof(MovementByVelocityEvent))]
[RequireComponent(typeof(MovementByVelocity))]
[RequireComponent(typeof(MovementToPositionEvent))]
[RequireComponent(typeof(MovementToPosition))]
[RequireComponent(typeof(SetActiveWeaponEvent))]
[RequireComponent(typeof(ActiveWeapon))]
[RequireComponent(typeof(WeaponFiredEvent))]
[RequireComponent(typeof(FireWeaponEvent))]
[RequireComponent(typeof(ReloadWeaponEvent))]
[RequireComponent(typeof(WeaponReloadedEventArgs))]
[RequireComponent(typeof(ReloadWeapon))]
[RequireComponent(typeof(DealContactDamage))]
[RequireComponent(typeof(ReciveContactDamage))]
[DisallowMultipleComponent]
#endregion
public class Player : MonoBehaviour
{
    [HideInInspector] public PlayerDetailsSO playerDetails;
    [HideInInspector] public Health health;
    [HideInInspector] public IdleEvent idleEvent;
    [HideInInspector] public AimWeaponEvent aimWeaponEvent;
    [HideInInspector] public HealthEvent healthEvent;
    [HideInInspector] public DestroyedEvent destroyedEvent;
    [HideInInspector] public MovementByVelocityEvent movementByVelocityEvent;
    [HideInInspector] public MovementToPositionEvent movementToPositionEvent;
    [HideInInspector] public SetActiveWeaponEvent setActiveWeaponEvent;
    [HideInInspector] public FireWeaponEvent fireWeaponEvent;
    [HideInInspector] public WeaponFiredEvent weaponFiredEvent;
    [HideInInspector] public ReloadWeaponEvent reloadWeaponEvent;
    [HideInInspector] public WeaponReloadedEvent weaponReloadedEvent;
    [HideInInspector] public FireWeapon fireWeapon;
    [HideInInspector] public ActiveWeapon activeWeapon;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public Animator animator;
    [HideInInspector] public PlayerControl playerControl;
    [HideInInspector] public PlayerStats playerStats;
    [HideInInspector] public ItemTextSpawner itemTextSpawner;

    [SerializeField] public UsableItem lastUsableItem = null;
    [SerializeField] UsableItem currentUsableItem = null;
    [SerializeField] int currentChargingPoints;
    [SerializeField] public HoldingItem holdingItem = null;
    [SerializeField] public HoldingItem lastHoldingItem = null;

    public List<Weapon> weaponList = new List<Weapon>();

    private void Awake()
    {
        healthEvent = GetComponent<HealthEvent>();
        health = GetComponent<Health>();
        idleEvent = GetComponent<IdleEvent>();
        destroyedEvent = GetComponent<DestroyedEvent>();
        aimWeaponEvent = GetComponent<AimWeaponEvent>();
        movementByVelocityEvent = GetComponent<MovementByVelocityEvent>();
        movementToPositionEvent = GetComponent<MovementToPositionEvent>();
        setActiveWeaponEvent = GetComponent<SetActiveWeaponEvent>();
        fireWeaponEvent = GetComponent<FireWeaponEvent>();
        weaponFiredEvent = GetComponent<WeaponFiredEvent>();
        activeWeapon = GetComponent<ActiveWeapon>();
        reloadWeaponEvent = GetComponent<ReloadWeaponEvent>();
        weaponReloadedEvent = GetComponent<WeaponReloadedEvent>();
        itemTextSpawner = GetComponentInChildren<ItemTextSpawner>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        playerControl = GetComponent<PlayerControl>();
        playerStats = GetComponent<PlayerStats>();
    }

    public void Initialize(PlayerDetailsSO playerDetails)
    {
        this.playerDetails = playerDetails;

        CreatePlayerStartingWeapons();
        SetPlayerHealth();

        StaticEventHandler.OnRoomEnemiesDefeated += StaticEventHandler_OnRoomEnemiesDefeated;

        if (currentUsableItem != null)
        {
            currentChargingPoints = currentUsableItem.GetChargingPoints();
        }
        else
        {
            currentChargingPoints = 0;
        }
    }

    private void StaticEventHandler_OnRoomEnemiesDefeated(RoomEnemiesDefeatedArgs e)
    {
        RefreshCurrentChargingPoints();
    }

    private void OnEnable()
    {
        healthEvent.OnHealthChanged += HealthEvent_OnHealthChanged;
    }

    private void OnDisable()
    {
        healthEvent.OnHealthChanged -= HealthEvent_OnHealthChanged;
    }

    private void HealthEvent_OnHealthChanged(HealthEvent healthEvent, HealthEventArgs healthEventArgs)
    {
        if (healthEventArgs.healthAmount <= 0f)
        {
            destroyedEvent.CallDestroyedEvent(true, 0);
        }
    }

    private void CreatePlayerStartingWeapons()
    {
        weaponList.Clear();

        foreach (WeaponDetailsSO weaponDetails in playerDetails.startingWeaponList)
        {
            AddWeaponToPlayer(weaponDetails);
        }
    }

    public void SetPlayerHealth()
    {
        int healthAmount = Mathf.Max(1, playerDetails.playerHealthAmount + playerStats.GetAdditionalhealth());
        health.SetStartingHealth(healthAmount);
    }

    public Vector3 GetPlayerPosition()
    {
        return transform.position;
    }

    public Weapon AddWeaponToPlayer(WeaponDetailsSO weaponDetails)
    {
        Weapon weapon = new Weapon()
        {
            weaponDetails = weaponDetails,
            weaponReloadTimer = 0f,
            weaponClipRemainingAmmo = weaponDetails.weaponClipAmmoCapacity,
            weaponRemainingAmmo = weaponDetails.weaponAmmoCapacity,
            isWeaponReloading = false
        };

        weaponList.Add(weapon);
        weapon.weaponListPosition = weaponList.Count;
        setActiveWeaponEvent.CallSetActiveWeaponEvent(weapon);

        return weapon;
    }

    public bool IsWeaponHeldByPlayer(WeaponDetailsSO weaponDetails)
    {
        foreach (Weapon weapon in weaponList)
        {
            if (weapon.weaponDetails == weaponDetails) return true;
        }

        return false;
    }

    public UsableItem GetCurrentUsableItem()
    {
        return currentUsableItem;
    }

    public void SetCurrentUsableItem(UsableItem item)
    {
        currentUsableItem = item;
    }

    public HoldingItem GetCurrentHoldingItem()
    {
        return holdingItem;
    }

    public void SetCurrentHoldingItem(HoldingItem item)
    {
        holdingItem = item;
    }

    public int GetCurrentChargingPoints()
    {
        return currentChargingPoints;
    }

    public void SetCurrentChargingPointsAfterUse()
    {
        currentChargingPoints = 0;
    }

    public void SetCurrentChargingPointsAfterUse(int amount)
    {
        currentChargingPoints = amount;
    }

    public void RefreshCurrentChargingPoints(int amount = 1)
    {
        if (currentUsableItem != null)
        {
            if (currentUsableItem.chargingPoints != 0)
            {
                if (currentChargingPoints < currentUsableItem.chargingPoints)
                {
                    currentChargingPoints += amount;
                    UsableItemUI.Instance.SetFill(currentUsableItem.chargingPoints, currentChargingPoints);

                    GameManager.Instance.usableItemsThatPlayerHad[currentUsableItem] = currentChargingPoints;
                }
            }
        }
    }
}
