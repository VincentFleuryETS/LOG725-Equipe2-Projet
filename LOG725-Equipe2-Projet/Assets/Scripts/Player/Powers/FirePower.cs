using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirePower : Power
{
    public override void Cast(Vector2 direction)
    {
        Debug.Log("Fire Power used!");
    }
}
