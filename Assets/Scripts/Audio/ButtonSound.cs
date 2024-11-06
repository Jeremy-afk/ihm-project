using UnityEngine.EventSystems;
using UnityEngine;

public class ButtonSound : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{
    [Tooltip("Use the sounds from the AudioManager singleton instead of overriding with those ones.")]
    [SerializeField] private bool useDefaultSounds = true;

    [Space]

    [SerializeField] private bool playOnHover = true;
    [SerializeField] private bool playOnSelect = true;
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private bool playClick = true;
    [SerializeField] private AudioClip clickSound;

    private AudioManager audioManager;
    private bool ready = false;

    private void Start()
    {
        audioManager = AudioManager.Instance;

        if (!audioManager)
        {
            Debug.LogError("There are no audio manager on the scene ! Disabling audio button module.");
            enabled = false;
            return;
        }

        ready = true;
    }

    // This is called when the button is selected (using a controller or keyboard navigation)
    public void OnSelect(BaseEventData eventData)
    {
        if (!playOnSelect || !ready) return;

        if (useDefaultSounds)
        {
            //audioManager.PlaySoundEffect(audioManager.menuSelect1);
        }
        else
        {
            audioManager.PlaySoundEffect(hoverSound);
        }
    }

    // This is called when the button is hovered (using a mouse)
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!playOnHover || !ready) return;

        if (useDefaultSounds)
        {
            //audioManager.PlaySoundEffect(audioManager.menuSelect1);
        }
        else
        {
            audioManager.PlaySoundEffect(hoverSound);
        }
    }

    // Call this when the button is clicked/validated
    public void PlayClickSound()
    {
        if (!playClick || !ready) return;

        if (useDefaultSounds)
        {
            audioManager.PlaySoundEffect(audioManager.menuClick);
        }
        else
        {
            audioManager.PlaySoundEffect(clickSound);
        }
    }
}