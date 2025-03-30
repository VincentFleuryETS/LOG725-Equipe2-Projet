using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PowerPickup : MonoBehaviour, IPickupable
{
    [Header("----- Properties -----")]
    [field: SerializeField]
    private PowerType powerType;
    [field: SerializeField]
    private ushort amount = 1;

    [Header("----- Audio -----")]
    [SerializeField]
    private AudioClip PickupSound;

    public void OnPickup(GameObject triggerObject)
    {
        if (triggerObject.TryGetComponent(out PlayerPowerController powerController))
        {
            AudioManager.GetSingleton().PlaySFX(PickupSound);
            powerController.ChangePowerCharge(powerType, amount);
            Destroy(gameObject);
        }
    }
}
