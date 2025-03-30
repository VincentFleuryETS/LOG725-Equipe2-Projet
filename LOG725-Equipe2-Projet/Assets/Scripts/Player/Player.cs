using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("----- Audio -----")]
    [SerializeField]
    private AudioClip DeathSound;

    public void KillPlayer()
    {
        AudioManager.GetSingleton().PlaySFX(DeathSound);
        GameManager.RestartCurrentLevel();
    }
}
