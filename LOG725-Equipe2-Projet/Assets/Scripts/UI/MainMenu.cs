using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void OnStartButtonPressed()
    {
        GameManager.OpenLevelByName("Level" + PlayerPrefs.GetInt("MaxLevelReached", 1));
    }

    public void OnLevelSelectButtonPressed()
    {
        GameManager.OpenLevelByName("LevelSelect");
    }

    public void OnExitButtonPressed()
    {
        GameManager.CloseGame();
    }
}
