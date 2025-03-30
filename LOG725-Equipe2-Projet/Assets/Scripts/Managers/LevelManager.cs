using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public int currentLevel; // À définir dans l’inspecteur pour chaque scène

    private List<SacredTree> treeList = new List<SacredTree>();
    private List<Ghost> ghostList = new List<Ghost>();

    void Update()
    {
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
        CompleteLevel();
    }

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
            SceneManager.LoadScene("MainMenuScene");
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