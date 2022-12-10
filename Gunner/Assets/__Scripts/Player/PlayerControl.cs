using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Player))]
[DisallowMultipleComponent]
public class PlayerControl : MonoBehaviour
{
    [SerializeField] private MovementDetailsSO movementDetails;
    [SerializeField] private float autoAimRange = 22f;

    private Player player;
    private bool leftMouseDownPreviousFrame = false;
    private int currentWeaponIndex = 1;
    private float moveSpeed;
    private Coroutine playerRollCoroutine;
    private WaitForFixedUpdate waitForFixedUpdate;
    [HideInInspector] public bool isPlayerRolling = false;
    private float playerRollCooldownTimer = 0f;
    private bool isPlayerMovementDisabled = false;

    private Joystick joystick;
    private Joystick rotationJoystick;
    private ShootButton joystickButton;
    private ActionButton actionButton;
    private RollButton rollButton;
    private UsableItemButton usableItemButton;
    private HoldingItemButton holdingItemButton;
    private WeaponChangeButton weaponChangeButton;
    private Transform point;
    private Rigidbody2D pointRigidbody2D;

    private void Awake()
    {
        player = GetComponent<Player>();
        moveSpeed = movementDetails.GetMoveSpeed();
    }

    private void Start()
    {
        waitForFixedUpdate = new WaitForFixedUpdate();

        SetStartingWeapon();
        SetPlayerAnimationSpeed();

        joystick = GameManager.Instance.joystick;
        rotationJoystick = GameManager.Instance.rotationJoystick;
        joystickButton = rotationJoystick.GetComponentInChildren<ShootButton>();
        weaponChangeButton = GameManager.Instance.weaponChangeButton;
        actionButton = GameManager.Instance.actionButton;
        rollButton = GameManager.Instance.rollButton;
        usableItemButton = GameManager.Instance.usableItemButton;
        holdingItemButton = GameManager.Instance.holdingItemButton;

        point = GameManager.Instance.point;
        pointRigidbody2D = point.GetComponent<Rigidbody2D>();
    }

    private void SetStartingWeapon()
    {
        int index = 1;

        foreach (Weapon weapon in player.weaponList)
        {
            if (weapon.weaponDetails == player.playerDetails.startingWeapon)
            {
                SetWeaponByIndex(index);
                break;
            }

            index++;
        }
    }

    private void SetPlayerAnimationSpeed()
    {
        player.animator.speed = moveSpeed / Settings.baseSpeedForPlayerAnimations;
    }

    public void SetMovementSpeed(float amount)
    {
        moveSpeed += amount;
    }

    public void SetMovementSpeedOnValue(float amount)
    {
        moveSpeed = amount;
    }

    public float GetMovementSpeed()
    {
        return moveSpeed;
    }

    private void Update()
    {
        if (isPlayerMovementDisabled) return;

        if (isPlayerRolling) return;

        MovementInput();
        UpdatePointPosition();
        WeaponInput();
        UseItemInput();
        PlayerRollCooldownTimer();
    }

    private void SetWeaponByIndex(int weaponIndex)
    {
        if (weaponIndex - 1 < player.weaponList.Count)
        {
            currentWeaponIndex = weaponIndex;
            player.setActiveWeaponEvent.CallSetActiveWeaponEvent(player.weaponList[weaponIndex - 1]);
        }
    }

    private void MovementInput()
    {
        float horizontalMovement = Mathf.RoundToInt(joystick.Horizontal);
        float verticalMovement = Mathf.RoundToInt(joystick.Vertical);

        //bool spaceButton = Input.GetKeyDown(KeyCode.Space);
        bool spaceButton = rollButton.rollButtonPressed;

        Vector2 direction = new Vector2(horizontalMovement, verticalMovement);

        if (horizontalMovement != 0 && verticalMovement != 0)
        {
            direction *= 0.7f;
        }

        if (direction != Vector2.zero)
        {
            if (!spaceButton)
            {
                player.movementByVelocityEvent.CallMovementByVelocityEvent(direction, moveSpeed);
            }

            else if (playerRollCooldownTimer <= 0f)
            {
                PlayerRoll((Vector3)direction);
            }
        }
        else
        {
            player.idleEvent.CallIdleEvent();
        }
    }

    private void PlayerRoll(Vector3 direction)
    {
        playerRollCoroutine = StartCoroutine(PlayerRollRoutine(direction));
    }

    private IEnumerator PlayerRollRoutine(Vector3 direction)
    {
        float minDistance = 0.2f;
        isPlayerRolling = true;

        Vector3 targetPosition = player.transform.position + (Vector3)direction * movementDetails.rollDistance;

        while (Vector3.Distance(player.transform.position, targetPosition) > minDistance)
        {
            player.movementToPositionEvent.CallMovementToPositionEvent(targetPosition, player.transform.position, movementDetails.rollSpeed,
                direction, isPlayerRolling);

            yield return waitForFixedUpdate;
        }

        isPlayerRolling = false;
        playerRollCooldownTimer = movementDetails.rollColdownTime;
        player.transform.position = targetPosition;
    }

    private void WeaponInput()
    {
        Vector3 weaponDirection;
        float weaponAngleDegrees, playerAngleDegrees;
        AimDirection playerAimDirection;

        AimWeaponInput(out weaponDirection, out weaponAngleDegrees, out playerAngleDegrees, out playerAimDirection);

        FireWeaponInput(weaponDirection, weaponAngleDegrees, playerAngleDegrees, playerAimDirection);

        SwitchWeaponInput();

        ReloadWeaponInput();

        HandleUsableItem();

        HandleHoldingItem();
    }

    private void UseItemInput()
    {
        //if (Input.GetKeyDown(KeyCode.E))
        if (actionButton.actionButtonPressed)
        {
            float useItemRadius = 2f;

            Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(player.GetPlayerPosition(), useItemRadius);

            foreach (Collider2D collider2D in collider2DArray)
            {
                IUseable useable = collider2D.GetComponent<IUseable>();

                if (useable != null)
                {
                    useable.UseItem();
                }
            }
        }
    }

    private void FireWeaponInput(Vector3 weaponDirection, float weaponAngleDegrees, float playerAngleDegrees, AimDirection playerAimDirection)
    {
        if (joystickButton.buttonPressed)
        {
            player.fireWeaponEvent.CallFireWeaponEvent(true, leftMouseDownPreviousFrame, playerAimDirection, playerAngleDegrees, weaponAngleDegrees,
                weaponDirection);

            leftMouseDownPreviousFrame = true;
        }
        else
        {
            leftMouseDownPreviousFrame = false;
        }
    }

    private void HandleUsableItem()
    {
        if (player.GetCurrentUsableItem() != null)
        {
            if (player.GetCurrentUsableItem().hasTimeEffect && !GameManager.Instance.canUseUsableItem) return;

            if (usableItemButton.usableButtonPressed)
            {
                player.GetCurrentUsableItem().OnUse();
            }
        }
    }

    private void HandleHoldingItem()
    {
        if (player.GetCurrentHoldingItem() != null)
        {
            if (player.GetCurrentHoldingItem().effectRank == ItemRank.Time && !GameManager.Instance.canUseHoldingItem) return;

            if (holdingItemButton.holdingItemButtonPressed)
            {
                player.GetCurrentHoldingItem().Use(true);
            }
        }
    }

    private void SwitchWeaponInput()
    {
        if (Input.mouseScrollDelta.y < 0f)
        {
            PreviousWeapon();
        }

        if (Input.mouseScrollDelta.y > 0f)
        {
            NextWeapon();
        }

        if (weaponChangeButton.weaponChangeButtonPressed)
        {
            NextWeapon();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetWeaponByIndex(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetWeaponByIndex(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetWeaponByIndex(3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SetWeaponByIndex(4);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SetWeaponByIndex(5);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SetWeaponByIndex(6);
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SetWeaponByIndex(7);
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            SetWeaponByIndex(8);
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            SetWeaponByIndex(9);
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SetWeaponByIndex(10);
        }

        if (Input.GetKeyDown(KeyCode.Minus))
        {
            SetCurrentWeaponToFirstInTheList();
        }
    }

    private void SetCurrentWeaponToFirstInTheList()
    {
        List<Weapon> tempWeaponList = new List<Weapon>();

        Weapon currentWeapon = player.weaponList[currentWeaponIndex - 1];
        currentWeapon.weaponListPosition = 1;
        tempWeaponList.Add(currentWeapon);

        int index = 2;

        foreach (Weapon weapon in player.weaponList)
        {
            if (weapon == currentWeapon) continue;

            tempWeaponList.Add(weapon);
            weapon.weaponListPosition = index;
            index++;
        }

        player.weaponList = tempWeaponList;
        currentWeaponIndex = 1;
        SetWeaponByIndex(currentWeaponIndex);
    }

    private void NextWeapon()
    {
        currentWeaponIndex++;

        if (currentWeaponIndex > player.weaponList.Count)
        {
            currentWeaponIndex = 1;
        }

        SetWeaponByIndex(currentWeaponIndex);
    }

    private void PreviousWeapon()
    {
        currentWeaponIndex--;

        if (currentWeaponIndex < 1)
        {
            currentWeaponIndex = player.weaponList.Count;
        }

        SetWeaponByIndex(currentWeaponIndex);
    }

    private void ReloadWeaponInput()
    {
        Weapon currentWeapon = player.activeWeapon.GetCurrentWeapon();

        if (currentWeapon.isWeaponReloading) return;

        if (currentWeapon.weaponRemainingAmmo < currentWeapon.weaponDetails.weaponClipAmmoCapacity && !currentWeapon.weaponDetails.hasInfiniteAmmo)
            return;

        if (currentWeapon.weaponClipRemainingAmmo == currentWeapon.weaponDetails.weaponClipAmmoCapacity) return;

        if (GameManager.Instance.reloadButton.reloadButtonPressed)
        {
            player.reloadWeaponEvent.CallReloadWeaponEvent(player.activeWeapon.GetCurrentWeapon(), 0);
        }
    }

    private void PlayerRollCooldownTimer()
    {
        if (playerRollCooldownTimer >= 0f)
        {
            playerRollCooldownTimer -= Time.deltaTime;
        }
    }

    public void EnablePlayer()
    {
        isPlayerMovementDisabled = false;
    }

    public void DisablePlayer()
    {
        isPlayerMovementDisabled = true;
        player.idleEvent.CallIdleEvent();
    }

    public void AimWeaponInput(out Vector3 weaponDirection, out float weaponAngleDegrees, out float playerAngleDegrees, out AimDirection playerAimDirection)
    {
        //Vector3 mouseWorldPosition = HelperUtilities.GetMouseWorldPosition();
        Vector3 pointPosition = Vector3.zero;
        if (FindBestTarget() == null)
        {
            pointPosition = HelperUtilities.GetPointWorldPosition();
        }
        else
        {
            pointPosition = FindBestTarget().transform.position + Vector3.up * 0.5f;
        }


        weaponDirection = (pointPosition - player.activeWeapon.GetShootPosition());

        Vector3 playerDirection = (pointPosition - transform.position);

        //Get weapon to cursor angle
        weaponAngleDegrees = HelperUtilities.GetAngleFromVector(weaponDirection);

        //Get player to cursor angle
        playerAngleDegrees = HelperUtilities.GetAngleFromVector(playerDirection);

        playerAimDirection = HelperUtilities.GetAimDirection(playerAngleDegrees);

        player.aimWeaponEvent.CallAimWeaponEvent(playerAimDirection, playerAngleDegrees, weaponAngleDegrees, weaponDirection);
    }

    private void UpdatePointPosition()
    {
        float x = rotationJoystick.Horizontal;
        float y = rotationJoystick.Vertical;

        Vector2 direction = new Vector2(x, y);

        if (x != 0 && y != 0)
        {
            direction *= 0.7f;
        }

        pointRigidbody2D.velocity = direction * 10000f;

        float radius = 100f;
        Vector3 centerPosition = new Vector3(Screen.width / 2, Screen.height / 2, 0f);
        float distance = Vector3.Distance(point.transform.position, centerPosition);
        if (distance > radius)
        {
            Vector3 fromOriginToObject = point.transform.position - centerPosition; //~GreenPosition~ - *BlackCenter*
            fromOriginToObject *= radius / distance; //Multiply by radius //Divide by Distance
            point.transform.position = centerPosition + fromOriginToObject; //*BlackCenter* + all that Math
        }
    }

    private IEnumerable<Enemy> FindAllEnemiesInRange()
    {
        Collider2D[] raycastHits = Physics2D.OverlapCircleAll(transform.position, autoAimRange);
        foreach (Collider2D collider in raycastHits)
        {
            Enemy enemy = collider.transform.GetComponent<Enemy>();
            if (enemy != null)
                yield return enemy;
        }
    }

    public Enemy FindBestTarget()
    {
        
        Enemy best = null;
        float bestDistance = Mathf.Infinity;

        foreach (Enemy canditate in FindAllEnemiesInRange())
        {
            float candidateDinstance = Vector3.Distance(transform.position, canditate.transform.position);
            if (candidateDinstance < bestDistance)
            {
                best = canditate;
                bestDistance = candidateDinstance;
            }
        }
        return best;
    }

    public static bool IsDoubleTap()
    {
        bool result = false;
        float MaxTimeWait = 1;
        float VariancePosition = 1;

        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            float DeltaTime = Input.GetTouch(0).deltaTime;
            float DeltaPositionLenght = Input.GetTouch(0).deltaPosition.magnitude;

            if (DeltaTime > 0 && DeltaTime < MaxTimeWait && DeltaPositionLenght < VariancePosition)
                result = true;
        }

        return result;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StopPlayerRollRoutine();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        StopPlayerRollRoutine();
    }

    private void StopPlayerRollRoutine()
    {
        if (playerRollCoroutine != null)
        {
            StopCoroutine(playerRollCoroutine);
            isPlayerRolling = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, autoAimRange);
    }

    #region Validation
#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(movementDetails), movementDetails);
    }

#endif
    #endregion
}
