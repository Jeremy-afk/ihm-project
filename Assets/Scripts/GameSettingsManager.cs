using System.Collections.Generic;
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
    public Dropdown resolutionDropdown;

    [Header("Other Settings")]
    public Slider sensitivitySlider;

    private AudioSource[] musicSources;
    private AudioSource[] sfxSources;
    private Resolution[] resolutions;

    void Start()
    {
        // Load saved settings or set defaults
        LoadSettings();

        // Populate resolution dropdown with available resolutions
        SetupResolutionDropdown();
    }

    void LoadSettings()
    {
        // Audio Settings
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);

        // Graphics Settings
        fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen", 1) == 1 ? true : false;
        sensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity", 1f);

        // Apply initial settings
        ApplyAudioSettings();
        ApplyGraphicsSettings();
    }

    public void SetupResolutionDropdown()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            // Check if this is the current resolution
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void ApplyAudioSettings()
    {
        // Apply volume levels to game audio
        AudioListener.volume = masterVolumeSlider.value;

        // Optionally, apply to specific music/SFX sources
        foreach (var musicSource in musicSources)
        {
            musicSource.volume = musicVolumeSlider.value;
        }

        foreach (var sfxSource in sfxSources)
        {
            sfxSource.volume = sfxVolumeSlider.value;
        }

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

        // Apply resolution
        int selectedResolutionIndex = resolutionDropdown.value;
        Resolution resolution = resolutions[selectedResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        // Save settings
        PlayerPrefs.SetInt("Fullscreen", fullscreenToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void ApplySensitivity()
    {
        // Adjust game sensitivity (for controller/mouse)
        float sensitivity = sensitivitySlider.value;
        // Apply sensitivity (assuming sensitivity is used in other parts of the game)

        // Save sensitivity setting
        PlayerPrefs.SetFloat("Sensitivity", sensitivity);
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

    public void OnSensitivityChanged()
    {
        ApplySensitivity();
    }
}
