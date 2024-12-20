using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Audio manager is a singleton that will be used to play sound effects
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource soundEffectSource;
    [SerializeField] private AudioSource soundEffectLoopSource;

    // All audio clips (public so accessible from other scripts)
    [Header("Musics")]
    public AudioClip menuTheme;
    public AudioClip mainTheme;
    public AudioClip chaseTheme;
    public AudioClip winTheme;

    [Header("Sound Effects")]
    public AudioClip sea;
    public AudioClip menuClick;
    public AudioClip plouf;
    public AudioClip fishBites;
    public AudioClip fishDirectionChange;
    public AudioClip winSound;
    public AudioClip gameOverSound;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadAudioPlayerPrefs();
    }

    // Classic function controls

    public void PlayMusic(AudioClip music = null, bool forceRestart = false)
    {
        if (music != null)
            musicSource.clip = music;

        if (musicSource.clip != null)
        {
            if (forceRestart)
                musicSource.Stop();
            musicSource.Play();
        }
    }
    
    public void PauseMusic()
    {
        musicSource.Pause();
    }

    public void PlaySoundEffect(AudioClip soundEffect)
    {
        soundEffectSource.clip = soundEffect;
        soundEffectSource.Play();
    }

    public void PlaySoundEffectLoop(AudioClip soundEffect)
    {
        soundEffectLoopSource.clip = soundEffect;
        soundEffectLoopSource.Play();
    }

    public void StopSoundEffectLoop()
    {
        soundEffectLoopSource.Stop();
    }

    public void SetMusicVolume(float vol)
    {
        musicSource.volume = vol;
    }

    public void SetSFXVolume(float vol)
    {
        soundEffectSource.volume = vol;
        soundEffectLoopSource.volume = vol;
    }

    private void LoadAudioPlayerPrefs()
    {
        float masterVol = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float musicVol = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfxVol = PlayerPrefs.GetFloat("SFXVolume", 1f);

        AudioListener.volume = masterVol;
        musicSource.volume = musicVol;
        soundEffectSource.volume = sfxVol;
        soundEffectLoopSource.volume = sfxVol;
    }
}
