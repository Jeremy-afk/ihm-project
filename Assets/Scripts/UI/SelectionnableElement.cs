using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionnableElement : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{
    [Header("Sound"), Tooltip("Use the sounds from the AudioManager singleton instead of overriding with those ones.")]
    [SerializeField] private bool useDefaultSounds = true;

    [Space]

    [SerializeField] private bool playOnHover = true;
    [SerializeField] private bool playOnSelect = true;
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private bool playConfirmSelection = true;
    [SerializeField] private AudioClip clickSound;


    [Header("Icon Overlay")]
    [SerializeField] private bool overrideElementSemiWidth = false;
    [SerializeField] private int elementSemiWidth = 380;

    private RectTransform rectTransform;
    private AudioManager audioManager;
    private bool ready = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        audioManager = AudioManager.Instance;

        if (!audioManager && useDefaultSounds)
        {
            Debug.LogError("There are no audio manager on the scene ! Disabling audio button module.");
            enabled = false;
        }

        ready = true;
    }

    public float GetElementSemiWidth()
    {
        if (overrideElementSemiWidth)
            return elementSemiWidth;
        else
        {
            return rectTransform.rect.width / 2;
        }
    }

    // This is called when the button is selected (using a controller or keyboard navigation)
    public void OnSelect(BaseEventData eventData)
    {
        if (!playOnSelect || !ready) return;

        if (useDefaultSounds)
        {
            audioManager.PlaySoundEffect(audioManager.menuSelect1);
        }
        else
        {
            audioManager.PlaySoundEffect(hoverSound);
        }
    }

    // This is called when the button is hovered (using a mouse)
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (EventSystem.current.currentSelectedGameObject != gameObject)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }

        if (!playOnHover || !ready) return;

        if (useDefaultSounds)
        {
            audioManager.PlaySoundEffect(audioManager.menuSelect1);
        }
        else
        {
            audioManager.PlaySoundEffect(hoverSound);
        }
    }

    // Call this when the button is clicked/validated
    public void PlayClickSound()
    {
        if (!playConfirmSelection || !ready) return;

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
