using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMovementController))]
[RequireComponent(typeof(PlayerPowerController))]
public class PlayerInputManager : MonoBehaviour
{
    private PlayerMovementController _playerMovementController;
    private PlayerPowerController _playerPowerController;

    private PlayerControls _playerControls;

    private void Awake()
    {
        _playerMovementController = GetComponent<PlayerMovementController>();
        _playerPowerController = GetComponent<PlayerPowerController>();

        _playerControls = new PlayerControls();
        _playerControls.Player.Enable();
    }

    private void OnEnable()
    {
        _playerControls.Player.Move.performed += OnMoveAction;
        _playerControls.Player.Move.canceled += OnMoveAction;

        _playerControls.Player.Jump.performed += OnJumpAction;
        _playerControls.Player.Jump.canceled += OnJumpAction;

        _playerControls.Player.Air.performed += OnAirAction;
        _playerControls.Player.Water.performed += OnWaterAction;
        _playerControls.Player.Earth.performed += OnEarthAction;
        _playerControls.Player.Fire.performed += OnFireAction;
    }

    private void OnDisable()
    {
        _playerControls.Player.Move.performed -= OnMoveAction;
        _playerControls.Player.Move.canceled -= OnMoveAction;

        _playerControls.Player.Jump.performed -= OnJumpAction;
        _playerControls.Player.Jump.canceled -= OnJumpAction;

        _playerControls.Player.Air.performed -= OnAirAction;
        _playerControls.Player.Water.performed -= OnWaterAction;
        _playerControls.Player.Earth.performed -= OnEarthAction;
        _playerControls.Player.Fire.performed -= OnFireAction;
    }


    public void OnMoveAction(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<Vector2>();
        _playerMovementController.SetMoveInput(value);
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
        _playerPowerController.UsePower(PowerType.Air, _playerControls.Player.Move.ReadValue<Vector2>());
    }

    public void OnWaterAction(InputAction.CallbackContext context)
    {
        _playerPowerController.UsePower(PowerType.Water, _playerControls.Player.Move.ReadValue<Vector2>());
    }

    public void OnEarthAction(InputAction.CallbackContext context)
    {
        _playerPowerController.UsePower(PowerType.Earth, _playerControls.Player.Move.ReadValue<Vector2>());
    }

    public void OnFireAction(InputAction.CallbackContext context)
    {
        _playerPowerController.UsePower(PowerType.Fire, _playerControls.Player.Move.ReadValue<Vector2>());
    }
}
