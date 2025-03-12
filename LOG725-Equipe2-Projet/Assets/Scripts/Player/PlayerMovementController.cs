using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class PlayerMovementController : MonoBehaviour
{
    [Header("----- Inputs -----")]
    [SerializeField] private InputActionReference MovementInput;
    [SerializeField] private InputActionReference JumpInput;

    [Header("----- Movement Parameters -----")]
    public float JumpStrength = 10.0f;
    public float WalkSpeed = 100f;
    public float MaxWalkSpeed = 100f;

    [Header("----- Ladder Parameters -----")]
    [SerializeField] private float climbSpeed = 10f;

    private Rigidbody2D _rigidbody;
    private CapsuleCollider2D _collider;
    private bool facingRight = true;
    private Vector2 _movement = Vector2.zero;
    private bool isClimbing;

    public Vector2 GetMoveDirection()
    {
        Vector2 input = MovementInput.action.ReadValue<Vector2>();
        Debug.Log($"Input - X: {input.x}, Y: {input.y}"); // Vérifie les inputs
        return input;
    }

    public Vector2 GetFacingDirection()
    {
        return facingRight ? transform.right : -transform.right;
    }

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CapsuleCollider2D>();
    }

    private void OnEnable()
    {
        JumpInput.action.performed += ctx => OnJumpPressed();
        JumpInput.action.canceled += ctx => OnJumpReleased();
    }

    private void OnDisable()
    {
        JumpInput.action.performed -= ctx => OnJumpPressed();
        JumpInput.action.canceled -= ctx => OnJumpReleased();
    }

    void Update()
    {
        Vector2 movementInput = GetMoveDirection();

        // Mouvement horizontal
        _movement.x = movementInput.x * WalkSpeed;
        Debug.Log($"Movement Calculated - X: {_movement.x}, Y: {_movement.y}"); // Vérifie le calcul

        if (isClimbing)
        {
            _movement.y = movementInput.y * climbSpeed;
            _rigidbody.gravityScale = 0f;
        }
        else
        {
            _movement.y = 0f;
            _rigidbody.gravityScale = 1f;
        }

        // Limiter la vitesse horizontale
        if (Math.Abs(_rigidbody.velocity.x) > MaxWalkSpeed)
        {
            _movement.x = 0f;
        }

        // Flip
        if (!isClimbing && movementInput.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (!isClimbing && movementInput.x < 0 && facingRight)
        {
            Flip();
        }
    }

    private bool IsGrounded()
    {
        bool grounded = Physics2D.CircleCast(transform.position, _collider.size.y / 2, Vector2.down, 1f);
        Debug.Log($"Is Grounded: {grounded}"); // Vérifie l'état au sol
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

    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector2(
            Mathf.Clamp(_movement.x, -MaxWalkSpeed, MaxWalkSpeed),
            isClimbing ? _movement.y : _rigidbody.velocity.y
        );
        Debug.Log($"Velocity Applied - X: {_rigidbody.velocity.x}, Y: {_rigidbody.velocity.y}"); // Vérifie la vélocité
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
}