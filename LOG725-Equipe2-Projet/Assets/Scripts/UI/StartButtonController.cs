using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButtonController : MonoBehaviour
{
    private int maxLevelReached;

    void Start()
    {
        // Charge la progression au démarrage
        maxLevelReached = PlayerPrefs.GetInt("MaxLevelReached", 1);
    }

    public void OnClick()
    {
        // Lance le dernier niveau validé
        SceneManager.LoadScene("Level" + maxLevelReached);
    }
}