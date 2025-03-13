using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Air Power requires the player to have a RigidBody2D to apply the Dash.
[RequireComponent(typeof(PlayerMovementController))]
public class AirPower : Power
{
    private PlayerMovementController playerMovementController;
    public float AirDashForce = 20.0f;
    public float AirDashDuration = 0.2f;

    void Awake()
    {
        playerMovementController = GetComponent<PlayerMovementController>();
    }

    public override void Cast(Vector2 direction)
    {
        if(Charges > 0)
        {
            Debug.Log("Air Power used!");
            //If the direction is basically zero, use the direction the Player is facing instead.
            if(direction.x < 0.1f && direction.x > -0.1f && direction.y < 0.1f && direction.y > -0.1f)
            {
                playerMovementController.LockVelocity(playerMovementController.GetFacingDirection() * AirDashForce, AirDashDuration);
            }
            else
            {
                direction.Normalize();
                playerMovementController.LockVelocity(direction * AirDashForce, AirDashDuration);
            }
            Charges--;
        }
        
    }
}
