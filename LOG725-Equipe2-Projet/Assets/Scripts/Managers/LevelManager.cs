using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public int currentLevel; // À définir dans l’inspecteur pour chaque scène
    [SerializeField] private GameObject levelCompletePanel; // Référence au panneau UI

    private List<SacredTree> treeList = new List<SacredTree>();
    private List<Ghost> ghostList = new List<Ghost>();
    private bool levelCompleted = false; // Pour éviter plusieurs déclenchements

    void Start()
    {
        // Assurer que le panneau est caché au démarrage
        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(false);
        }
    }

    void Update()
    {
        if (levelCompleted) return; // Ne pas vérifier si le niveau est déjà terminé

        // Réinitialiser les listes
        treeList.Clear();
        ghostList.Clear();

        // Remplir la liste des arbres sacrés
        SacredTree[] trees = FindObjectsOfType<SacredTree>();
        foreach (SacredTree tree in trees)
        {
            treeList.Add(tree);
        }

        // Remplir la liste des fantômes
        Ghost[] ghosts = FindObjectsOfType<Ghost>();
        foreach (Ghost ghost in ghosts)
        {
            ghostList.Add(ghost);
        }

        // Vérifier les conditions de victoire
        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        // Vérifier si la liste des fantômes est vide
        if (ghostList.Count > 0)
        {
            return; // Il reste des fantômes, pas de victoire
        }

        // Vérifier si tous les arbres sont Purified ou AbsorbedSpirit
        foreach (SacredTree tree in treeList)
        {
            if (tree.CurrentState != TreeState.Purified &&
                tree.CurrentState != TreeState.AbsorbedSpirit)
            {
                return; // Un arbre est encore Corrupted, pas de victoire
            }
        }

        // Si on arrive ici, toutes les conditions sont remplies
        ShowLevelCompleteScreen();
    }

    private void ShowLevelCompleteScreen()
    {
        levelCompleted = true; // Marquer le niveau comme terminé
        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(true); // Afficher le panneau
            Time.timeScale = 0f; // Optionnel : mettre le jeu en pause
        }
    }

    // Méthode appelée par le bouton "Niveau Suivant"
    public void GoToNextLevel()
    {
        int nextLevel = currentLevel + 1;
        if (nextLevel > PlayerPrefs.GetInt("MaxLevelReached", 1))
        {
            PlayerPrefs.SetInt("MaxLevelReached", nextLevel);
            PlayerPrefs.Save();
        }

        Time.timeScale = 1f; // Reprendre le temps si mis en pause
        if (SceneExists("Level" + nextLevel))
        {
            SceneManager.LoadScene("Level" + nextLevel);
        }
        else
        {
            SceneManager.LoadScene("MainMenuScene");
        }
    }

    // Méthode appelée par le bouton "Menu Principal"
    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // Reprendre le temps si mis en pause
        SceneManager.LoadScene("MainMenuScene");
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