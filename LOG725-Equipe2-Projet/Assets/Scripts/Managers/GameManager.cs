using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Singleton to manage the game at a high level. (Level changes, pause, quit the game, etc.)

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    private void Awake()
    {
        //Keep single instance of AudioManager between scenes.
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void OpenLevel(int level)
    {
        Debug.Log("GameManager: Load Level " + level);
        SceneManager.LoadScene(level);
        InputManager.SetActionMap(InputManager.Player);
        UnpauseGame();
    }

    public static void OpenLevelByName(string levelName)
    {
        Debug.Log("GameManager: Load Level " + levelName);
        SceneManager.LoadScene(levelName);
        InputManager.SetActionMap(InputManager.Player);
        UnpauseGame();
    }

    public static void OpenMainMenuScene()
    {
        SceneManager.LoadScene("MainMenuScene");
        InputManager.SetActionMap(InputManager.UI);
    }

    public static void OpenLevelSelectScene()
    {
        SceneManager.LoadScene("LevelSelect");
        InputManager.SetActionMap(InputManager.UI);
    }

    public static bool CheckIfLevelExistsByName(string levelName)
    {
        int buildIndex = SceneUtility.GetBuildIndexByScenePath(levelName);
        return buildIndex >= 0;
    }

    public static void RestartCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        UnpauseGame();
    }

    public static void CloseGame()
    {
        Debug.Log("GameManager: Close Game");
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public static void UnpauseGame()
    {
        Time.timeScale = 1;
    }

    public static void PauseGame()
    {
        Time.timeScale = 0;
    }
}