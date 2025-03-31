using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    [Header("----- Parameters -----")]
    [SerializeField] public int CurrentLevel; // À définir dans l’inspecteur pour chaque scène

    [Header("----- Events -----")]
    public UnityEvent LevelCompleteEvent;
    public bool LevelCompleted = false; // Pour éviter plusieurs déclenchements

    private List<SacredTree> treeList = new List<SacredTree>();
    private List<Ghost> ghostList = new List<Ghost>();


    void Update()
    {
        if (LevelCompleted) return; // Ne pas vérifier si le niveau est déjà terminé

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
            if (tree.CurrentState == TreeState.Corrupted)
            {
                return; // Un arbre est encore Corrupted, pas de victoire
            }
        }

        // Si on arrive ici, toutes les conditions sont remplies, niveau gagné.
        LevelComplete();
    }

    private void LevelComplete()
    {
        LevelCompleted = true;
        LevelCompleteEvent.Invoke();
        int nextLevel = CurrentLevel + 1;
        if (nextLevel > PlayerPrefs.GetInt("MaxLevelReached", 1) && GameManager.CheckIfLevelExistsByName("Level" + nextLevel))
        {
            PlayerPrefs.SetInt("MaxLevelReached", CurrentLevel + 1);
            PlayerPrefs.Save();
        }
    }

    // Méthode appelée par le bouton "Niveau Suivant"
    public void GoToNextLevel()
    {
        int nextLevel = CurrentLevel + 1;
        if (GameManager.CheckIfLevelExistsByName("Level" + nextLevel))
        {
            GameManager.OpenLevelByName("Level" + nextLevel);
        }
    }

    
}