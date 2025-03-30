using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _mainPauseMenuCanvas;
    [SerializeField]
    private GameObject _settingsMenuCanvas;
    
    // Start is called before the first frame update
    void Start()
    {
        _mainPauseMenuCanvas.SetActive(false);
        _settingsMenuCanvas.SetActive(false);

        SubscribeInputCallbacks();
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
        InputManager.Pause.performed += OnPauseAction;
        InputManager.Exit.performed += OnExitAction;
    }

    private void UnsubscribeInputCallbacks()
    {
        InputManager.Pause.performed -= OnPauseAction;
        InputManager.Exit.performed -= OnExitAction;
    }

    #region Input Action Functions

    public void OnPauseAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Pause();
        }
    }

    public void OnExitAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Unpause();
        }
    }

    #endregion

    #region Pause/Unpause Functions

    private void Pause()
    {
        Debug.Log("Pausing");
        GameManager.PauseGame();
        InputManager.SetActionMap(InputManager.UI);

        _mainPauseMenuCanvas.SetActive(true);
    }

    private void Unpause()
    {
        Debug.Log("Unpausing");
        CloseAllMenus();

        InputManager.SetActionMap(InputManager.Player);
        GameManager.ResumeGame();
    }

    #endregion

    #region Menu Functions

    private void CloseAllMenus()
    {
        _mainPauseMenuCanvas.SetActive(false);
        _settingsMenuCanvas.SetActive(false);
    }

    private void OpenMainMenu()
    {
        _mainPauseMenuCanvas.SetActive(true);
        _settingsMenuCanvas.SetActive(false);
    }

    private void OpenSettingsMenu()
    {
        _settingsMenuCanvas.SetActive(true);
        _mainPauseMenuCanvas.SetActive(false);
    }

    #endregion

    #region Main Menu Button Functions

    public void OnResumeButtonPressed()
    {
        Unpause();
    }

    public void OnSettingsButtonPressed()
    {
        OpenSettingsMenu();
    }

    public void OnExitButtonPressed()
    {
        GameManager.OpenLevelByName("MainMenuScene");
    }

    #endregion

    #region Settings Menu Button Functions

    public void OnSettingsBackButtonPressed()
    {
        OpenMainMenu();
    }

    #endregion
}
