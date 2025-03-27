using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerPowerController : MonoBehaviour
{
    [Header("     ----- Inputs -----")]
    [SerializeField] private InputActionReference AirPowerInput;
    [SerializeField] private InputActionReference WaterPowerInput;
    [SerializeField] private InputActionReference EarthPowerInput;
    [SerializeField] private InputActionReference FirePowerInput;
    [SerializeField] private InputActionReference MovementInput;

    private Action<InputAction.CallbackContext> AirPowerCallback;
    private Action<InputAction.CallbackContext> WaterPowerCallback;
    private Action<InputAction.CallbackContext> EarthPowerCallback;
    private Action<InputAction.CallbackContext> FirePowerCallback;

    [Header("----- Powers -----")]
    public AirPower AirPower;
    public WaterPower WaterPower;
    public EarthPower EarthPower;
    public FirePower FirePower;

    [Header("----- Events -----")]
    public UnityEvent PowersUpdated;


    void Awake()
    {
        AirPowerCallback = (callbackContext)    => UsePower(PowerType.Air, callbackContext);
        WaterPowerCallback = (callbackContext)  => UsePower(PowerType.Water, callbackContext);
        EarthPowerCallback = (callbackContext)  => UsePower(PowerType.Earth, callbackContext);
        FirePowerCallback = (callbackContext)   => UsePower(PowerType.Fire, callbackContext);
    }

    private void OnEnable()
    {
        AirPowerInput.action.performed += AirPowerCallback;
        WaterPowerInput.action.performed += WaterPowerCallback;
        EarthPowerInput.action.performed += EarthPowerCallback;
        FirePowerInput.action.performed += FirePowerCallback;
    }

    private void OnDisable()
    {
        AirPowerInput.action.performed -= AirPowerCallback;
        WaterPowerInput.action.performed -= WaterPowerCallback;
        EarthPowerInput.action.performed -= EarthPowerCallback;
        FirePowerInput.action.performed -= FirePowerCallback;
    }

    private Power GetPowerByType(PowerType type)
    {
        switch (type)
        {
            case PowerType.Air:
                return AirPower;
            case PowerType.Water:
                return WaterPower;
            case PowerType.Earth:
                return EarthPower;
            case PowerType.Fire:
                return FirePower;
            default:
                return null;
        }
    }

    private void UsePower(PowerType type, InputAction.CallbackContext callbackContext)
    {
        GetPowerByType(type).Cast(MovementInput.action.ReadValue<Vector2>());
        PowersUpdated.Invoke();
    }

    public void ChangePowerCharge(PowerType type, int amount)
    {
        GetPowerByType(type).Charges += amount;
        PowersUpdated.Invoke();
    }

    public void SetPowerCharge(PowerType type, int amount)
    {
        GetPowerByType(type).Charges = amount;
        PowersUpdated.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IPickupable triggerable))
        {
            triggerable.OnPickup(gameObject);
        }
    }
}
