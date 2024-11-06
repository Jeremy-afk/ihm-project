using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameSettingsManager : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private float volumeExponent = 2.0f;
    [Space]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [Space]
    [SerializeField] private AudioSource[] musicSources;
    [SerializeField] private AudioSource[] sfxSources;

    [Header("Graphics Settings")]
    public Toggle fullscreenToggle;

    private bool preventSave = true;

    private void OnEnable()
    {
        preventSave = true;
        StartCoroutine(EnableSaveAfter());
    }

    private void Awake()
    {
        LoadSettings();
    }

    private void LoadSettings()
    {
        masterVolumeSlider.value = Mathf.Pow(PlayerPrefs.GetFloat("MasterVolume", 1f), 1 / volumeExponent);
        musicVolumeSlider.value = Mathf.Pow(PlayerPrefs.GetFloat("MusicVolume", 1f), 1 / volumeExponent);
        sfxVolumeSlider.value = Mathf.Pow(PlayerPrefs.GetFloat("SFXVolume", 1f), 1 / volumeExponent);

        fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen", 1) == 1;

        Debug.Log("Loaded music volume at " + musicVolumeSlider.value, gameObject);

        ApplyAudioSettings();
        ApplyGraphicsSettings();
    }

    public void ApplyAudioSettings()
    {
        // Apply volume levels to game audio
        AudioListener.volume = Mathf.Pow(masterVolumeSlider.value, volumeExponent);
        AudioManager.Instance.SetMusicVolume(Mathf.Pow(musicVolumeSlider.value, volumeExponent));
        AudioManager.Instance.SetSFXVolume(Mathf.Pow(sfxVolumeSlider.value, volumeExponent));

        // Save settings
        PlayerPrefs.SetFloat("MasterVolume", masterVolumeSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolumeSlider.value);
        //Debug.Log("Saved music volume at " + musicVolumeSlider.value, gameObject);
    }

    public void ApplyGraphicsSettings()
    {
        // Apply fullscreen setting
        Screen.fullScreen = fullscreenToggle.isOn;

        // Save settings
        PlayerPrefs.SetInt("Fullscreen", fullscreenToggle.isOn ? 1 : 0);
    }

    public void OnVolumeChanged()
    {
        if (preventSave) return;
        ApplyAudioSettings();
    }

    public void OnGraphicsChanged()
    {
        if (preventSave) return;
        ApplyGraphicsSettings();
    }

    private IEnumerator EnableSaveAfter()
    {
        yield return new WaitForEndOfFrame();

        preventSave = false;
    }
}
