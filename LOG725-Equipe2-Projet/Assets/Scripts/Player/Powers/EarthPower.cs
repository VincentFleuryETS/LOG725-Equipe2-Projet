using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EarthPower : Power
{
    public override void Cast(Vector2 direction)
    {
        Debug.Log("Earth Power used!");
    }
}
