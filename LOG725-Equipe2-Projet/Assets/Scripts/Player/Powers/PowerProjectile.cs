using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PowerProjectile : MonoBehaviour
{
    public PowerType powerType;
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IPowerTriggerable triggerable))
        {
            triggerable.OnPowerTriggered(gameObject);
        }
        Destroy(gameObject);
    }

    public void ActivateGravity()
    {
        GetComponent<Rigidbody2D>().gravityScale = 3.0f;
    }
}
