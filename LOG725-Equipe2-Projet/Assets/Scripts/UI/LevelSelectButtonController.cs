using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectButtonController : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene("LevelSelect");
    }
}