using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int currentLevel; // À définir dans l’inspecteur pour chaque scène

    public void CompleteLevel()
    {
        int nextLevel = currentLevel + 1;
        if (nextLevel > PlayerPrefs.GetInt("MaxLevelReached", 1))
        {
            PlayerPrefs.SetInt("MaxLevelReached", nextLevel);
            PlayerPrefs.Save();
        }

        if (SceneExists("Level" + nextLevel))
        {
            SceneManager.LoadScene("Level" + nextLevel);
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    private bool SceneExists(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            if (path.Contains(sceneName)) return true;
        }
        return false;
    }
}