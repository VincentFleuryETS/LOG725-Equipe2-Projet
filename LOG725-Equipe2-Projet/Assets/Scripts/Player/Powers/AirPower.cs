using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Air Power requires the player to have a RigidBody2D to apply the Dash.
[RequireComponent(typeof(Rigidbody2D))]
public class AirPower : Power
{
    private Rigidbody2D PlayerRigidbody;

    void Awake()
    {
        PlayerRigidbody = GetComponent<Rigidbody2D>();
    }

    public override void Cast()
    {
        Debug.Log("Air Power used!");
    }
}
