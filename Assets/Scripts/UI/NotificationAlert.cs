using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ButtonReference
{
    None,
    LStick,
    RStick,
    A,
    B,
    X,
    Y,
    Z,
    Q,
    S,
    D,
    Space
}

public class NotificationAlert : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup notificationCanvasGroup;
    [SerializeField]
    private TextMeshProUGUI textNotification;
    [SerializeField]
    private Image imageButton;

    [Space]

    [SerializeField, Tooltip("Disappear after this much time. 0 means this will not disappear automatically.")]
    private float defaultDuration = 3f;
    [SerializeField, Tooltip("Fading duration ? (0 = no fading ~ instant)")]
    private float fadeDuration = 0.4f;

    [Space]

    [SerializeField]
    private bool alwaysShowOnEnable = false;
    [SerializeField]
    private bool showOnStart = false;

    private void OnEnable()
    {
        if (alwaysShowOnEnable)
            ToogleNotification(true);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        ToogleNotification(false);
    }

    private void Awake()
    {
        if (showOnStart)
            NewNotification("Welcome to the game!", ButtonReference.None, defaultDuration);
        else
            ToogleNotification(false);
    }

    public void NewNotification(string text, ButtonReference buttonToDisplay, float duration)
    {
        textNotification.text = text;
        HandleButtonImage(buttonToDisplay);
        ToogleNotification(true);

        if (duration > 0f)
            StartCoroutine(FadeAfter(duration));
    }

    public void ToogleNotification(bool show)
    {
        ResetFade();
        notificationCanvasGroup.gameObject.SetActive(show);
    }

    private void HandleButtonImage(ButtonReference btnRef)
    {
        switch(btnRef)
        {
            case ButtonReference.None:
                imageButton.gameObject.SetActive(false);
                break;

            case ButtonReference.LStick:
                imageButton.sprite = Resources.Load<Sprite>("Buttons/LStick");
                imageButton.gameObject.SetActive(true);
                break;

            case ButtonReference.RStick:
                imageButton.sprite = Resources.Load<Sprite>("Buttons/RStick");
                imageButton.gameObject.SetActive(true);
                break;

            case ButtonReference.A:
                imageButton.sprite = Resources.Load<Sprite>("Buttons/A");
                imageButton.gameObject.SetActive(true);
                break;

            case ButtonReference.B:
                imageButton.sprite = Resources.Load<Sprite>("Buttons/B");
                imageButton.gameObject.SetActive(true);
                break;

            case ButtonReference.X:
                imageButton.sprite = Resources.Load<Sprite>("Buttons/X");
                imageButton.gameObject.SetActive(true);
                break;

            case ButtonReference.Y:
                imageButton.sprite = Resources.Load<Sprite>("Buttons/Y");
                imageButton.gameObject.SetActive(true);
                break;

            case ButtonReference.Z:
                imageButton.sprite = Resources.Load<Sprite>("Buttons/Z");
                imageButton.gameObject.SetActive(true);
                break;

            case ButtonReference.Q:
                imageButton.sprite = Resources.Load<Sprite>("Buttons/Q");
                imageButton.gameObject.SetActive(true);
                break;

            case ButtonReference.S:
                imageButton.sprite = Resources.Load<Sprite>("Buttons/S");
                imageButton.gameObject.SetActive(true);
                break;

            case ButtonReference.D:
                imageButton.sprite = Resources.Load<Sprite>("Buttons/D");
                imageButton.gameObject.SetActive(true);
                break;

            case ButtonReference.Space:
                imageButton.sprite = Resources.Load<Sprite>("Buttons/Space");
                imageButton.gameObject.SetActive(true);
                break;
        }

        // Else do the logic to get the right button image and set it to the imageButton
        // It should depend if the button is A, B, X or Y and also if the player is using a controller or a keyboard
    }

    private IEnumerator FadeAfter(float duration)
    {
        float time = duration;

        while (time > 0)
        {
            time -= Time.deltaTime;

            if (time < fadeDuration)
            {
                notificationCanvasGroup.alpha = time / fadeDuration;
            }

            yield return null;
        }
    }

    private void ResetFade()
    {
        notificationCanvasGroup.alpha = 1;
    }
}
