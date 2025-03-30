using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerPowerController : MonoBehaviour
{
    [Header("----- Powers -----")]
    public AirPower AirPower;
    public WaterPower WaterPower;
    public EarthPower EarthPower;
    public FirePower FirePower;

    [Header("----- Events -----")]
    public UnityEvent PowersUpdated;


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

    public void UsePower(PowerType type, Vector2 castDirection)
    {
        GetPowerByType(type).Cast(castDirection);
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
