using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent (typeof(CapsuleCollider2D))]
public class EarthMovableObject : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartMoving()
    {
        _rigidbody.isKinematic = false;
    }

    public void StopMoving()
    {
        _rigidbody.isKinematic = true;
        _rigidbody.velocity = Vector3.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out EarthPower earthPower))
        {
            if (earthPower.IsPowerActive())
            {
                Debug.Log("Collision with EarthPower active!");
                earthPower.AddMovableObject(this);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        /*if (collision.gameObject.TryGetComponent(out EarthPower earthPower))
        {
            earthPower.RemoveMovableObject(this);
        }*/
    }
}
