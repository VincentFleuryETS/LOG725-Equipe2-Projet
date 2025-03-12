using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PowerPickup : MonoBehaviour, IPickupable
{

    [field: SerializeField]
    private PowerType powerType;
    [field: SerializeField]
    private ushort amount = 1;

    public void OnPickup(GameObject triggerObject)
    {
        if (triggerObject.TryGetComponent(out PlayerPowerController powerController))
        {
            powerController.ChangePowerCharge(powerType, amount);
            Destroy(gameObject);
        }
    }
}
