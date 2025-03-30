using UnityEngine;

public class QuitButtonController : MonoBehaviour
{
    public void OnClick()
    {
        Application.Quit();
        Debug.Log("Jeu quitté");
    }
}