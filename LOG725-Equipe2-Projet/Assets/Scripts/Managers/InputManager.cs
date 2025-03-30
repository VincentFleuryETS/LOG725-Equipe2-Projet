using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private static InputManager instance;
    
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
    private InputAction m_Player_ResetLevel;
    private InputAction m_Player_Pause;

    public static InputActionMap @Player => instance.m_Player;
    public static InputAction @Move => instance.m_Player_Move;
    public static InputAction @Jump => instance.m_Player_Jump;
    public static InputAction @Air => instance.m_Player_Air;
    public static InputAction @Water => instance.m_Player_Water;
    public static InputAction @Earth => instance.m_Player_Earth;
    public static InputAction @Fire => instance.m_Player_Fire;
    public static InputAction @ResetLevel => instance.m_Player_ResetLevel;
    public static InputAction @Pause => instance.m_Player_Pause;

    // UI ActionMap
    private InputActionMap m_UI;
    private InputAction m_UI_Exit;

    public static InputAction @Exit => instance.m_UI_Exit;
    public static InputActionMap @UI => instance.m_UI;

    // Current settings
    private InputActionMap _currentActionMap;

    private void Awake()
    {
        //Keep single instance of AudioManager between scenes.
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            InitializeActions();
            // Currently, default is Player. Should find a better way to set default ActionMap, but we have no time.
            SetActionMap(m_Player);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeActions()
    {
        // Player
        m_Player = PlayerControlsAsset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Move = m_Player.FindAction("Mouvement", throwIfNotFound: true);
        m_Player_Jump = m_Player.FindAction("Saut", throwIfNotFound: true);
        m_Player_Air = m_Player.FindAction("Pouvoir Air", throwIfNotFound: true);
        m_Player_Water = m_Player.FindAction("Pouvoir Eau", throwIfNotFound: true);
        m_Player_Earth = m_Player.FindAction("Pouvoir Terre", throwIfNotFound: true);
        m_Player_Fire = m_Player.FindAction("Pouvoir Feu", throwIfNotFound: true);
        m_Player_ResetLevel = m_Player.FindAction("Recommencer", throwIfNotFound: true);
        m_Player_Pause = m_Player.FindAction("Pause", throwIfNotFound: true);

        // Player
        m_UI = PlayerControlsAsset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_Exit = m_UI.FindAction("Sortir", throwIfNotFound: true);

    }

    public static void SetActionMap(InputActionMap newActionMap)
    {
        instance._currentActionMap?.Disable();
        instance._currentActionMap = newActionMap;
        instance._currentActionMap.Enable();
    }

}
