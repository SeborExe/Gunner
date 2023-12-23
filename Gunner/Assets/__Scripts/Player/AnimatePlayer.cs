using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
[DisallowMultipleComponent]
public class AnimatePlayer : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void OnEnable()
    {
        player.movementByVelocityEvent.OnMovementByVelocity += MovementByVelocityEvent_OnMoveByVelocity;

        player.movementToPositionEvent.OnMovementToPosition += MovementToPositionEvent_OnMovementToPosition;

        player.idleEvent.OnIdle += IdleEvent_OnIdle;
        player.aimWeaponEvent.OnWeaponAim += AimWeaponEvent_OnWeaponAim;    
    }

    private void OnDisable()
    {
        player.movementByVelocityEvent.OnMovementByVelocity -= MovementByVelocityEvent_OnMoveByVelocity;

        player.movementToPositionEvent.OnMovementToPosition -= MovementToPositionEvent_OnMovementToPosition;

        player.idleEvent.OnIdle -= IdleEvent_OnIdle;
        player.aimWeaponEvent.OnWeaponAim -= AimWeaponEvent_OnWeaponAim;
    }

    private void MovementByVelocityEvent_OnMoveByVelocity(MovementByVelocityEvent movementByVelocityEvent, MovementByVelocityArgs movementByVelocityArgs)
    {
        InitializeRollAnimatorParameters();
        SetMovementAnimationParameters();
    }

    private void MovementToPositionEvent_OnMovementToPosition(MovementToPositionEvent movementToPosition, MovementToPositionArgs movementToPositionArgs)
    {
        InitializeAimAnimatorParameters();
        InitializeRollAnimatorParameters();
        SetMovementToPositionAnimationParameters(movementToPositionArgs);
    }

    private void IdleEvent_OnIdle(IdleEvent idleEvent)
    {
        InitializeRollAnimatorParameters();
        SetIdleAnimationsParameters();
    }

    private void AimWeaponEvent_OnWeaponAim(AimWeaponEvent aimWeaponEvent, AimWeaponEventArgs aimWeaponEventArgs)
    {
        InitializeAimAnimatorParameters();
        InitializeRollAnimatorParameters();
        SetAimWeaponAnimationParameters(aimWeaponEventArgs.aimDirection);
    }

    private void SetMovementAnimationParameters()
    {
        player.animator.SetBool(Settings.isMoving, true);
        player.animator.SetBool(Settings.isIdle, false);
    }

    private void SetIdleAnimationsParameters()
    {
        player.animator.SetBool(Settings.isMoving, false);
        player.animator.SetBool(Settings.isIdle, true);
    }

    private void InitializeAimAnimatorParameters()
    {
        player.animator.SetBool(Settings.aimUp, false);
        player.animator.SetBool(Settings.aimUpRight, false);
        player.animator.SetBool(Settings.aimUpLeft, false);
        player.animator.SetBool(Settings.aimRight, false);
        player.animator.SetBool(Settings.aimLeft, false);
        player.animator.SetBool(Settings.aimDown, false);
    }

    private void SetAimWeaponAnimationParameters(AimDirection aimDirection)
    {
        switch (aimDirection)
        {
            case AimDirection.Up:
                player.animator.SetBool(Settings.aimUp, true);
                break;

            case AimDirection.Left:
                player.animator.SetBool(Settings.aimLeft, true);
                break;

            case AimDirection.Right:
                player.animator.SetBool(Settings.aimRight, true);
                break;

            case AimDirection.UpLeft:
                //player.animator.SetBool(Settings.aimUpLeft, true);
                player.animator.SetBool(Settings.aimUp, true);
                break;

            case AimDirection.UpRight:
                //player.animator.SetBool(Settings.aimUpRight, true);
                player.animator.SetBool(Settings.aimUp, true);
                break;

            case AimDirection.Down:
                player.animator.SetBool(Settings.aimDown, true);
                break;
        }
    }

    private void InitializeRollAnimatorParameters()
    {
        player.animator.SetBool(Settings.rollDown, false);
        player.animator.SetBool(Settings.rollLeft, false);
        player.animator.SetBool(Settings.rollRight, false);
        player.animator.SetBool(Settings.rollUp, false);
    }

    private void SetMovementToPositionAnimationParameters(MovementToPositionArgs movementToPositionArgs)
    {
        if (movementToPositionArgs.isRolling)
        {
            if (movementToPositionArgs.moveDirection.x > 0f)
            {
                player.animator.SetBool(Settings.rollRight, true);
            }

            else if (movementToPositionArgs.moveDirection.x < 0f)
            {
                player.animator.SetBool(Settings.rollLeft, true);
            }

            else if (movementToPositionArgs.moveDirection.y > 0f)
            {
                player.animator.SetBool(Settings.rollUp, true);
            }

            else if (movementToPositionArgs.moveDirection.y < 0f)
            {
                player.animator.SetBool(Settings.rollDown, true);
            }
        }
    }
}
