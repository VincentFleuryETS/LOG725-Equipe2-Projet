using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class EarthMovableObject : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        //_rigidbody.isKinematic = true;
        _rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if(Math.Abs(_rigidbody.velocity.y) > 0.01)
        {
            _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
        }
    }

    public void StartMoving()
    {
        //_rigidbody.isKinematic = false;
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void StopMoving()
    {
        //_rigidbody.isKinematic = true;
        _rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
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
}
