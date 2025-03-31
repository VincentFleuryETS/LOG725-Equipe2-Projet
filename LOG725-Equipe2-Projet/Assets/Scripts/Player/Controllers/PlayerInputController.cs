using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMovementController))]
[RequireComponent(typeof(PlayerPowerController))]
public class PlayerInputController : MonoBehaviour
{
    private PlayerMovementController _playerMovementController;
    private PlayerPowerController _playerPowerController;

    private void Awake()
    {
        _playerMovementController = GetComponent<PlayerMovementController>();
        _playerPowerController = GetComponent<PlayerPowerController>();
    }

    private void OnEnable()
    {
        SubscribeInputCallbacks();
    }

    private void OnDisable()
    {
        UnsubscribeInputCallbacks();
    }

    private void SubscribeInputCallbacks()
    {
        InputManager.Move.performed += OnMoveAction;
        InputManager.Move.canceled += OnMoveAction;

        InputManager.Jump.performed += OnJumpAction;
        InputManager.Jump.canceled += OnJumpAction;

        InputManager.Air.performed += OnAirAction;
        InputManager.Water.performed += OnWaterAction;
        InputManager.Earth.performed += OnEarthAction;
        InputManager.Fire.performed += OnFireAction;

        InputManager.ResetLevel.performed += OnResetLevelAction;
    }

    private void UnsubscribeInputCallbacks()
    {
        InputManager.Move.performed -= OnMoveAction;
        InputManager.Move.canceled -= OnMoveAction;

        InputManager.Jump.performed -= OnJumpAction;
        InputManager.Jump.canceled -= OnJumpAction;

        InputManager.Air.performed -= OnAirAction;
        InputManager.Water.performed -= OnWaterAction;
        InputManager.Earth.performed -= OnEarthAction;
        InputManager.Fire.performed -= OnFireAction;

        InputManager.ResetLevel.performed -= OnResetLevelAction;
    }


    public void OnMoveAction(InputAction.CallbackContext context)
    {
        if (context.performed || context.canceled)
        {
            _playerMovementController.SetMoveInput(context.ReadValue<Vector2>());
        }
    }

    public void OnJumpAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _playerMovementController.StartJump();
        }
        else if (context.canceled) {
            _playerMovementController.EndJump();
        }
    }

    public void OnAirAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _playerPowerController.UsePower(PowerType.Air, InputManager.Move.ReadValue<Vector2>());
        }
    }

    public void OnWaterAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _playerPowerController.UsePower(PowerType.Water, InputManager.Move.ReadValue<Vector2>());
        }
    }

    public void OnEarthAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _playerPowerController.UsePower(PowerType.Earth, InputManager.Move.ReadValue<Vector2>());
        }
    }

    public void OnFireAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _playerPowerController.UsePower(PowerType.Fire, InputManager.Move.ReadValue<Vector2>());
        }
    }

    public void OnResetLevelAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameManager.RestartCurrentLevel();
        }
    }
}
