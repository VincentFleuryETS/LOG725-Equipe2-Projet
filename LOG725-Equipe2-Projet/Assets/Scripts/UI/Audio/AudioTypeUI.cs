using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioTypeUI : MonoBehaviour
{
    [field: SerializeField] private AudioType audioType;
    [field: SerializeField] private Toggle muteToggle;

    private AudioManager audioManager;


    // Start is called before the first frame update
    void Start()
    {
        audioManager = AudioManager.GetSingleton();
        muteToggle.isOn = audioManager.IsAudioTypeEnabled(audioType);
    }

    private void OnEnable()
    {
        muteToggle.onValueChanged.AddListener(OnMuteButtonToggled);
    }

    private void OnDisable()
    {
        muteToggle.onValueChanged.RemoveListener(OnMuteButtonToggled);
    }

    public void OnMuteButtonToggled(bool newValue)
    {
        audioManager.ToggleAudioType(audioType, newValue);
    }
}
