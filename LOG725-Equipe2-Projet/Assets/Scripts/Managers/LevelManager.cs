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

    private SacredTree[] treeArray;
    private Ghost[] ghostArray;

    private void Start()
    {
        treeArray = FindObjectsOfType<SacredTree>();
        ghostArray = FindObjectsOfType<Ghost>();
    }


    void Update()
    {
        if (LevelCompleted) return; // Ne pas vérifier si le niveau est déjà terminé

        // Vérifier les conditions de victoire
        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        // Vérifier si la liste des fantômes est vide
        foreach (Ghost ghost in ghostArray) { 
            if(ghost != null)
            {
                return; // Il reste des fantômes, pas de victoire
            }
        }

        // Vérifier si tous les arbres sont Purified ou AbsorbedSpirit
        foreach (SacredTree tree in treeArray)
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