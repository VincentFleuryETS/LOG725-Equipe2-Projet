using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public enum PowerType
{
    Air,
    Water,
    Earth,
    Fire
}

public class PlayerPowerController : MonoBehaviour
{
    [SerializeField] private InputActionReference AirPowerInput;
    [SerializeField] private InputActionReference WaterPowerInput;
    [SerializeField] private InputActionReference EarthPowerInput;
    [SerializeField] private InputActionReference FirePowerInput;

    private Action<InputAction.CallbackContext> AirPowerCallback;
    private Action<InputAction.CallbackContext> WaterPowerCallback;
    private Action<InputAction.CallbackContext> EarthPowerCallback;
    private Action<InputAction.CallbackContext> FirePowerCallback;

    public AirPower AirPower;
    public WaterPower WaterPower;
    public EarthPower EarthPower;
    public FirePower FirePower;

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
        GetPowerByType(type).Cast();
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
}
