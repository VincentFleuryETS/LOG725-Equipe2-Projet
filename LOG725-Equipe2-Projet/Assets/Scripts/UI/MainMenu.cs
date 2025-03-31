using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void OnStartButtonPressed()
    {
        //PlayerPrefs.SetInt("MaxLevelReached", 1);
        //PlayerPrefs.Save();
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
