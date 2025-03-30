using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButtonController : MonoBehaviour
{
    public int levelNumber; // Numéro du niveau pour ce bouton (à définir dans l’inspecteur)
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        int maxLevelReached = PlayerPrefs.GetInt("MaxLevelReached", 1);

        // Désactive le bouton si le niveau est supérieur au maximum atteint
        button.interactable = (levelNumber <= maxLevelReached);
    }

    public void OnClick()
    {
        SceneManager.LoadScene("Level" + levelNumber);
    }
}