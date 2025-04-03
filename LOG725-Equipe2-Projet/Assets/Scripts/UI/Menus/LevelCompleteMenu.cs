using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteMenu : MonoBehaviour
{
    [Header("----- Level Manager -----")]
    [SerializeField]
    private LevelManager _levelManager;

    [Header("----- UI Elements -----")]
    [SerializeField]
    private GameObject _levelCompleteCanvas;
    [SerializeField]
    private GameObject _nextLevelButton;

    private int _nextLevel;


    private void Start()
    {
        _levelCompleteCanvas.SetActive(false);
        _levelManager.LevelCompleteEvent.AddListener(OpenLevelCompleteMenu);

        int nextLevel = _levelManager.CurrentLevel + 1;
        if (GameManager.CheckIfLevelExistsByName("Level" + nextLevel))
        {
            _nextLevelButton.SetActive(true);
        }
        else
        {
            _nextLevelButton.SetActive(false);
        }
    }

    void OnDestroy()
    {
        // Se désabonne de l'événement pour éviter les fuites de mémoire
        if (_levelManager != null)
        {
            _levelManager.LevelCompleteEvent.RemoveListener(OpenLevelCompleteMenu);
        }
    }
    public void OpenLevelCompleteMenu()
    {
        GameManager.PauseGame();
        _levelCompleteCanvas.SetActive(true);
    }

    public void CloseLevelCompleteMenu()
    {
        _levelCompleteCanvas.SetActive(false);
        GameManager.UnpauseGame();
    }

    public void NextLevelButtonPressed()
    {
        _levelManager.GoToNextLevel();
    }

    public void ReturnToMainMenuButtonPressed()
    {
        GameManager.OpenMainMenuScene();
    }
}
