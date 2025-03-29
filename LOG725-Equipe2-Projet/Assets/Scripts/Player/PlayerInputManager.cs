using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMovementController))]
[RequireComponent(typeof(PlayerPowerController))]
public class PlayerInputManager : MonoBehaviour
{
    private PlayerMovementController _playerMovementController;
    private PlayerPowerController _playerPowerController;

    [SerializeField]
    private InputActionAsset PlayerControlsAsset;

    // Player ActionMap
    private InputActionMap m_Player;
    private InputAction m_Player_Move;
    private InputAction m_Player_Jump;
    private InputAction m_Player_Air;
    private InputAction m_Player_Water;
    private InputAction m_Player_Earth;
    private InputAction m_Player_Fire;

    private void Awake()
    {
        _playerMovementController = GetComponent<PlayerMovementController>();
        _playerPowerController = GetComponent<PlayerPowerController>();

        InitializeActions();
    }

    private void InitializeActions()
    {
        // Player
        m_Player = PlayerControlsAsset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
        m_Player_Jump = m_Player.FindAction("Jump", throwIfNotFound: true);
        m_Player_Air = m_Player.FindAction("Air", throwIfNotFound: true);
        m_Player_Water = m_Player.FindAction("Water", throwIfNotFound: true);
        m_Player_Earth = m_Player.FindAction("Earth", throwIfNotFound: true);
        m_Player_Fire = m_Player.FindAction("Fire", throwIfNotFound: true);
        m_Player.Enable();
    }

    private void OnEnable()
    {
        m_Player_Move.performed += OnMoveAction;
        m_Player_Move.canceled += OnMoveAction;

        m_Player_Jump.performed += OnJumpAction;
        m_Player_Jump.canceled += OnJumpAction;

        m_Player_Air.performed += OnAirAction;
        m_Player_Water.performed += OnWaterAction;
        m_Player_Earth.performed += OnEarthAction;
        m_Player_Fire.performed += OnFireAction;
    }

    private void OnDisable()
    {
        m_Player_Move.performed -= OnMoveAction;
        m_Player_Move.canceled -= OnMoveAction;

        m_Player_Jump.performed -= OnJumpAction;
        m_Player_Jump.canceled -= OnJumpAction;

        m_Player_Air.performed -= OnAirAction;
        m_Player_Water.performed -= OnWaterAction;
        m_Player_Earth.performed -= OnEarthAction;
        m_Player_Fire.performed -= OnFireAction;
    }


    public void OnMoveAction(InputAction.CallbackContext context)
    {
        if(context.performed || context.canceled)
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
            _playerPowerController.UsePower(PowerType.Air, m_Player_Move.ReadValue<Vector2>());
        }
        
    }

    public void OnWaterAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _playerPowerController.UsePower(PowerType.Water, m_Player_Move.ReadValue<Vector2>());
        }
        
    }

    public void OnEarthAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _playerPowerController.UsePower(PowerType.Earth, m_Player_Move.ReadValue<Vector2>());
        }
        
    }

    public void OnFireAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _playerPowerController.UsePower(PowerType.Fire, m_Player_Move.ReadValue<Vector2>());
        }
        
    }
}
