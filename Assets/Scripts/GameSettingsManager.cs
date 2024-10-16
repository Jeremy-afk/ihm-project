using UnityEngine;
using UnityEngine.UI;

public class GameSettingsManager : MonoBehaviour
{
    [Header("Audio Settings")]
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    [Header("Graphics Settings")]
    public Toggle fullscreenToggle;

    private AudioSource[] musicSources;
    private AudioSource[] sfxSources;

    void Start()
    {
        LoadSettings();
    }

    void LoadSettings()
    {
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);

        fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen", 1) == 1 ? true : false;

        ApplyAudioSettings();
        ApplyGraphicsSettings();
    }

    public void ApplyAudioSettings()
    {
        // Apply volume levels to game audio
        AudioListener.volume = masterVolumeSlider.value;

        // TODO: LINK THE AUDIO SOURCES VOLUMES (create function set volume or idk in the audio manager that will be called here)
        // Optionally, apply to specific music/SFX sources
        //foreach (var musicSource in musicSources)
        //{
        //    musicSource.volume = musicVolumeSlider.value;
        //}

        //foreach (var sfxSource in sfxSources)
        //{
        //    sfxSource.volume = sfxVolumeSlider.value;
        //}

        // Save settings
        PlayerPrefs.SetFloat("MasterVolume", masterVolumeSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolumeSlider.value);
        PlayerPrefs.Save();
    }

    public void ApplyGraphicsSettings()
    {
        // Apply fullscreen setting
        Screen.fullScreen = fullscreenToggle.isOn;

        // Save settings
        PlayerPrefs.SetInt("Fullscreen", fullscreenToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void OnVolumeChanged()
    {
        ApplyAudioSettings();
    }

    public void OnGraphicsChanged()
    {
        ApplyGraphicsSettings();
    }
}
