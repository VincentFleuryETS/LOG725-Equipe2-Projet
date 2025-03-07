using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Power : MonoBehaviour
{
    [SerializeField]
    public InputActionReference PowerInput;

    [SerializeField]
    public ushort Charges { get; set; } = 0;

    private Action<InputAction.CallbackContext> PowerCallback;

    void Awake()
    {
        PowerCallback = (callbackContext) => UsePower(callbackContext);
    }

    private void OnEnable()
    {
        PowerInput.action.performed += PowerCallback;
    }

    private void OnDisable()
    {
        PowerInput.action.performed -= PowerCallback;
    }

    protected abstract void UsePower(InputAction.CallbackContext callbackContext);
}
