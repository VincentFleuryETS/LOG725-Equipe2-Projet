using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class PlayerMovementController : MonoBehaviour
{
    [Header("     ----- Inputs -----")]
    [SerializeField] private InputActionReference MovementInput;
    [SerializeField] private InputActionReference JumpInput;

    private Action<InputAction.CallbackContext> JumpPerformedCallback;
    private Action<InputAction.CallbackContext> JumpCanceledCallback;

    [Header("----- Movement Parameters -----")]
    public float JumpStrength = 12.0f;
    public float WalkSpeed = 9;
    public float DefaultGravity = 2.0f;

    [Header("----- Ladder Parameters -----")]
    [SerializeField] private float climbSpeed = 10f;

    private Rigidbody2D _rigidbody;
    private CapsuleCollider2D _collider;
    private bool facingRight = true;
    private Vector2 _movement = Vector2.zero;
    private bool isClimbing = false;
    private bool velocityIsLocked = false;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CapsuleCollider2D>();
        _rigidbody.gravityScale = DefaultGravity;

        JumpPerformedCallback = (callbackContext) => OnJumpPressed();
        JumpCanceledCallback = (callbackContext) => OnJumpReleased();
    }

    private void OnEnable()
    {
        JumpInput.action.performed += JumpPerformedCallback;
        JumpInput.action.canceled += JumpCanceledCallback;
    }

    private void OnDisable()
    {
        JumpInput.action.performed -= JumpPerformedCallback;
        JumpInput.action.canceled -= JumpCanceledCallback;
    }

    private void FixedUpdate()
    {
        if (!velocityIsLocked) // N'applique le mouvement normal que si la vélocité n'est pas locked.
        {
            _rigidbody.velocity = _movement;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isClimbing = true;
            Debug.Log("Entered ladder");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isClimbing = false;
            Debug.Log("Exited ladder");
        }
    }

    void Update()
    {
        Vector2 movementInput = GetMoveDirection();

        _movement.x = movementInput.x * WalkSpeed;


        if (isClimbing)
        {
            _movement.y = movementInput.y * climbSpeed;
            _rigidbody.gravityScale = 0f;
        }
        else
        {
            //If not climbing or jumping, cut the vertical positive velocity.
            if (JumpInput.action.ReadValue<float>() < 0.1f && _rigidbody.velocity.y > 0)
            {
                _movement.y = _rigidbody.velocity.y / 1.2f;
            }
            else
            {
                _movement.y = _rigidbody.velocity.y;
            }
            _rigidbody.gravityScale = DefaultGravity;
        }


        if (!isClimbing && movementInput.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (!isClimbing && movementInput.x < 0 && facingRight)
        {
            Flip();
        }
    }

    public Vector2 GetMoveDirection()
    {
        Vector2 input = MovementInput.action.ReadValue<Vector2>();
        return input;
    }

    public Vector2 GetFacingDirection()
    {
        return facingRight ? transform.right : -transform.right;
    }

    private bool IsGrounded()
    {
        bool grounded = Physics2D.CircleCast(transform.position, _collider.size.y / 2, Vector2.down, 1f);
        //Debug.Log($"Is Grounded: {grounded}");
        return grounded;
    }

    private void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;
        facingRight = !facingRight;
    }

    private void OnJumpPressed()
    {
        if (IsGrounded() && !isClimbing)
        {
            AddForce(Vector2.up * JumpStrength, ForceMode2D.Impulse, false);
        }
        else if (isClimbing)
        {
            isClimbing = false;
            AddForce(Vector2.up * JumpStrength, ForceMode2D.Impulse, false);
        }
    }

    private void OnJumpReleased()
    {
        if (_rigidbody.velocity.y > 0 && !isClimbing)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y / 2);
        }
    }

    public void AddForce(Vector2 force, ForceMode2D forceMode, bool cancelCurrentVelocity)
    {
        if (cancelCurrentVelocity)
        {
            _rigidbody.velocity = Vector2.zero;
        }
        _rigidbody.AddForce(force, forceMode);
    }

    /// <summary>
    /// Lock the velocity of the entity to the value of <paramref name="velocity"/> for <paramref name="timeLocked"/> seconds.
    /// </summary>
    /// <param name="velocity"></param>
    /// <param name="timeLocked"></param>
    public void LockVelocity(Vector2 velocity, float timeLocked)
    {
        velocityIsLocked = true;
        _rigidbody.velocity = velocity;
        _rigidbody.gravityScale = 0f;
        Invoke(nameof(UnlockVelocity), timeLocked);
    }

    /// <summary>
    /// Unlock the velocity of the entity.
    /// </summary>
    public void UnlockVelocity()
    {
        if(_rigidbody != null)
        {
            _rigidbody.gravityScale = DefaultGravity;
            velocityIsLocked = false;
        }
    }

    public void IgnoreCollision(Collider2D otherCollider)
    {
        Physics2D.IgnoreCollision(_collider, otherCollider);
    }

    
}