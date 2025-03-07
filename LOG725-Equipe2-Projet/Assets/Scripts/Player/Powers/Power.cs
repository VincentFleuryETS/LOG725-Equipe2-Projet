using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public abstract class Power : MonoBehaviour
{

    [SerializeField, Min(0)]
    private int charges;

    
    public int Charges
    {
        get { return charges; }
        //We clamp the value to 0.
        set { charges = Math.Max(value, 0); }
    }

    public abstract void Cast();
}
