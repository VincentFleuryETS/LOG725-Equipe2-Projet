
using System;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent (typeof(CapsuleCollider2D))]
public class PlayerMovementController : MonoBehaviour
{
    [Header("     ----- Inputs -----")]
    [SerializeField] private InputActionReference MovementInput;
    [SerializeField] private InputActionReference JumpInput;

    private Action<InputAction.CallbackContext> JumpPressedCallback;
    private Action<InputAction.CallbackContext> JumpReleasedCallback;

    [Header("----- Parameters -----")]
    public float JumpStrength = 5.0f;
    public float WalkAcceleration = 30.0f;
    public float MaxWalkSpeed = 12f;

    private Rigidbody2D _rigidbody;
    private CapsuleCollider2D _collider;
    private bool facingRight = true;
    private Vector2 _movement = Vector2.zero;

    /// <summary>
    /// Returns the current direction the Player is trying to move towards.
    /// </summary>
    public Vector2 GetMoveDirection()
    {
        return MovementInput.action.ReadValue<Vector2>();
    }

    /// <summary>
    /// Returns the current direction the Player is facing.
    /// </summary>
    public Vector2 GetFacingDirection()
    {
        return facingRight ? transform.right : -transform.right;
    }


    // Start is called before the first frame update
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CapsuleCollider2D>();

        JumpPressedCallback = (callbackContext) => OnJumpPressed(callbackContext);
        JumpReleasedCallback = (callbackContext) => OnJumpReleased(callbackContext);
    }

    private void OnEnable()
    {
        JumpInput.action.performed += JumpPressedCallback;
        JumpInput.action.canceled += JumpReleasedCallback;
    }

    private void OnDisable()
    {
        JumpInput.action.performed -= JumpPressedCallback;
        JumpInput.action.canceled -= JumpReleasedCallback;
    }

    // Update is called once per frame
    void Update()
    {
        //We get the movement input value, and set the horizontal velocity.
        Vector2 movementInput = GetMoveDirection();

        if (Math.Abs(_rigidbody.velocity.x) <= MaxWalkSpeed)
        {
            _movement = new Vector2(movementInput.x * WalkAcceleration, 0);
        }
        else
        {
            _movement = Vector2.zero;
        }
        

        //We flip the character sprite if we change directions.
        if (movementInput.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (movementInput.x < 0 && facingRight) {
            Flip();
        }
    }

    /// <summary>
    /// Returns <c>true</c> if the Player is currently grounded (standing on ground). Returns <c>false</c> otherwise.
    /// </summary>
    private bool IsGrounded()
    {
        return Physics2D.CircleCast(transform.position, _collider.size.y, Vector2.down, 1f);
    }

    /// <summary>
    /// Flips the horizontal transform of the Player in order to flip their sprite.
    /// </summary>
    private void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        facingRight = !facingRight;
    }


    private void OnJumpPressed(InputAction.CallbackContext callbackContext)
    {
        if (IsGrounded())
        {
            AddForce(Vector2.up * JumpStrength, ForceMode2D.Impulse, false);
        }
    }

    /*
     * We attenuate the vertical velocity to allow the player to control jump height.
     */
    private void OnJumpReleased(InputAction.CallbackContext callbackContext)
    {
        if (_rigidbody.velocity.y > 0)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y / 2);
        }
    }

    /// <summary>
    /// Applies a force to the PlayerMovementController's RigidBody.
    /// </summary>
    public void AddForce(Vector2 force, ForceMode2D forceMode, bool cancelCurrentVelocity)
    {
        if (cancelCurrentVelocity) {
            _rigidbody.velocity = Vector2.zero;
        }
        _rigidbody.AddForce(force, ForceMode2D.Impulse);
    }

    private void FixedUpdate()
    {
        _rigidbody.AddForce(_movement);
    }
}
