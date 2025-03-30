using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Animator))] // Ajoute l'Animator comme composant requis
public class PlayerMovementController : MonoBehaviour
{
    [Header("----- Movement Parameters -----")]
    public float JumpStrength = 12.0f;
    public float WalkSpeed = 9;
    public float DefaultGravity = 2.0f;

    [Header("----- Ladder Parameters -----")]
    [SerializeField] private float climbSpeed = 10f;

    private Rigidbody2D _rigidbody;
    private CapsuleCollider2D _collider;
    private Animator _animator; // Référence à l'Animator
    private bool facingRight = true;
    private Vector2 _moveInput = Vector2.zero;
    private Vector2 _movement = Vector2.zero;
    private bool isJumping = false;
    private bool isClimbing = false;
    private bool velocityIsLocked = false;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CapsuleCollider2D>();
        _animator = GetComponent<Animator>(); // Récupère l'Animator
        _rigidbody.gravityScale = DefaultGravity;
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
            _animator.SetBool("IsClimbing", true); // Active l'animation de grimper
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isClimbing = false;
            _animator.SetBool("IsClimbing", false); // Désactive l'animation de grimper
        }
    }

    void Update()
    {
        _movement.x = _moveInput.x * WalkSpeed;

        if (isClimbing)
        {
            _movement.y = _moveInput.y * climbSpeed;
            _rigidbody.gravityScale = 0f;
        }
        else
        {
            // Si pas en train de grimper ou de sauter, coupe la vélocité verticale positive
            if (!isJumping && _rigidbody.velocity.y > 0)
            {
                _movement.y = _rigidbody.velocity.y / 1.2f;
            }
            else
            {
                _movement.y = _rigidbody.velocity.y;
            }
            _rigidbody.gravityScale = DefaultGravity;
        }

        // Met à jour le paramètre Speed pour l'animation
        _animator.SetFloat("Speed", Mathf.Abs(_moveInput.x));

        // Gestion de l'orientation du sprite
        if (!isClimbing && _moveInput.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (!isClimbing && _moveInput.x < 0 && facingRight)
        {
            Flip();
        }
    }

    private bool IsGrounded()
    {
        bool grounded = Physics2D.CircleCast(transform.position, _collider.size.y / 2, Vector2.down, 1f);
        return grounded;
    }

    private void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;
        facingRight = !facingRight;
    }

    public Vector2 GetMoveInput()
    {
        return _moveInput;
    }

    public void SetMoveInput(Vector2 direction)
    {
        _moveInput = direction;
    }

    public Vector2 GetFacingDirection()
    {
        return facingRight ? transform.right : -transform.right;
    }

    public void StartJump()
    {
        if (IsGrounded() && !isClimbing)
        {
            AddForce(Vector2.up * JumpStrength, ForceMode2D.Impulse, false);
            isJumping = true;
        }
        else if (isClimbing)
        {
            AddForce(Vector2.up * JumpStrength, ForceMode2D.Impulse, false);
            isClimbing = false;
            _animator.SetBool("IsClimbing", false); // Désactive l'animation de grimper
            isJumping = true;
        }
    }

    public void EndJump()
    {
        isJumping = false;
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

    public void LockVelocity(Vector2 velocity, float timeLocked)
    {
        velocityIsLocked = true;
        _rigidbody.velocity = velocity;
        _rigidbody.gravityScale = 0f;
        Invoke(nameof(UnlockVelocity), timeLocked);
    }

    public void UnlockVelocity()
    {
        if (_rigidbody != null)
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