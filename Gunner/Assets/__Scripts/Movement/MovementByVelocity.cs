using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] 
[RequireComponent(typeof(MovementByVelocityEvent))] 
[DisallowMultipleComponent]
public class MovementByVelocity : MonoBehaviour
{
    private Rigidbody2D rigidBody2D;
    private MovementByVelocityEvent movementByVelocityEvent;

    private void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        movementByVelocityEvent = GetComponent<MovementByVelocityEvent>();
    }

    private void OnEnable()
    {
        movementByVelocityEvent.OnMovementByVelocity += MovementByVelocityEvent_OnMoveByVelocity;
    }

    private void OnDisable()
    {
        movementByVelocityEvent.OnMovementByVelocity -= MovementByVelocityEvent_OnMoveByVelocity;
    }

    private void MovementByVelocityEvent_OnMoveByVelocity(MovementByVelocityEvent movementByVelocityEvent, MovementByVelocityArgs movementByVelocityArgs)
    {
        MoveRigidBody(movementByVelocityArgs.moveDirection, movementByVelocityArgs.moveSpeed);
    }

    private void MoveRigidBody(Vector2 moveDirection, float moveSpeed)
    {
        rigidBody2D.velocity = moveDirection * moveSpeed;
    }
}
