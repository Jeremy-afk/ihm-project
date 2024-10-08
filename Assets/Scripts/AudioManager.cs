using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Audio manager is a singleton that will be used to play sound effects
    public static AudioManager instance;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource soundEffectSource;
    [SerializeField] private AudioSource soundEffectLoopSource;

    // All audio clips (public so accessible from other scripts)
    [Header("Musics")]
    public AudioClip mainTheme;
    public AudioClip chaseTheme;
    public AudioClip winTheme;

    [Header("Sound Effects")]
    public AudioClip menuSelect1;
    public AudioClip menuClick;
    public AudioClip plouf;
    public AudioClip fishBites;
    public AudioClip fishDirectionChange;
    public AudioClip winSound;
    public AudioClip gameOverSound;

    private void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
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
}
