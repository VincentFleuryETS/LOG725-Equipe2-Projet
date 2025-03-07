using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AirPower : Power
{
    protected override void UsePower(InputAction.CallbackContext callbackContext)
    {
        Debug.Log("Air Power used!");
    }
}
