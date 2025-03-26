using System;
using UnityEngine;

[RequireComponent(typeof(PlayerMovementController))]
public abstract class Power : MonoBehaviour
{

    [SerializeField, Min(0)]
    private int charges;

    [SerializeField]
    protected AudioClip CastSound;

    protected PlayerMovementController playerMovementController;

    void Awake()
    {
        playerMovementController = GetComponent<PlayerMovementController>();
    }

    public int Charges
    {
        get { return charges; }
        //We clamp the value to 0.
        set { charges = Math.Max(value, 0); }
    }

    public abstract void Cast(Vector2 direction);
}
