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
    [Header("Random placement")]
    [SerializeField, Tooltip("Relative to the screen")]
    private Vector2 min;
    [SerializeField, Tooltip("Relative to the screen")]
    private Vector2 max;
    [SerializeField, Range(0, 180)]
    private float randomAngle;
    [SerializeField]
    private float gizmoSize = 2f;

    [Space]

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
    private float defaultFadeDuration = 0.4f;

    [Space]

    [SerializeField]
    private bool alwaysShowOnEnable = false;
    [SerializeField]
    private bool showOnStart = false;

    private RectTransform rectTransform;

    private void OnDrawGizmosSelected()
    {
        // Set Gizmos color for the rectangles
        Gizmos.color = Color.black;

        // Calculate the possible values for x and y positions based on screen width and height
        float minXPos = min.x * Screen.width / 2;
        float maxXPos = max.x * Screen.width / 2;
        float minYPos = min.y * Screen.height / 2;
        float maxYPos = max.y * Screen.height / 2;

        // Array to handle positive and negative positions for each axis
        int[] directions = { -1, 1 };

        // Loop through each combination of positive and negative x and y directions
        foreach (int xDir in directions)
        {
            foreach (int yDir in directions)
            {
                // Define the four corner points of the rectangle
                Vector3 bottomLeft = new Vector3(xDir * minXPos, yDir * minYPos, transform.position.z);
                Vector3 bottomRight = new Vector3(xDir * maxXPos, yDir * minYPos, transform.position.z);
                Vector3 topLeft = new Vector3(xDir * minXPos, yDir * maxYPos, transform.position.z);
                Vector3 topRight = new Vector3(xDir * maxXPos, yDir * maxYPos, transform.position.z);

                // Draw the rectangle by connecting the corner points
                Gizmos.DrawLine(bottomLeft, bottomRight);
                Gizmos.DrawLine(bottomRight, topRight);
                Gizmos.DrawLine(topRight, topLeft);
                Gizmos.DrawLine(topLeft, bottomLeft);
            }
        }
    }

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
            ToogleNotification(true);
        else
            ToogleNotification(false);

        rectTransform = GetComponent<RectTransform>();
    }

    public void NewNotification(string text, ButtonReference buttonToDisplay, float duration, float fadeDuration)
    {
        textNotification.text = text;
        HandleButtonImage(buttonToDisplay);
        ToogleNotification(true);

        if (duration > 0f)
            StartCoroutine(FadeAfter(duration, fadeDuration));
    }

    public void NewNotification(string text, Sprite sprite, float duration, float fadeDuration)
    {
        textNotification.text = text;

        if (sprite)
            imageButton.sprite = sprite;
        else
        {
            imageButton.sprite = null;
            imageButton.gameObject.SetActive(false);
        }

        ToogleNotification(true);

        if (duration > 0f)
            StartCoroutine(FadeAfter(duration, fadeDuration));
    }

    public void PlaceRandomly()
    {
        // Step 1: Get the canvas size to define the screen quarters
        Vector2 canvasSize = new(Screen.width, Screen.height);

        // Step 2: Define the quarter width and height
        float quarterWidth = canvasSize.x / 2;
        float quarterHeight = canvasSize.y / 2;

        // Step 3: Calculate a random position within the defined "base" quarter
        float baseX = Random.Range(min.x * quarterWidth, max.x * quarterWidth);
        float baseY = Random.Range(min.y * quarterHeight, max.y * quarterHeight);

        // Step 4: Randomly choose a quadrant by adding an offset to the base position
        float xOffset = (Random.Range(0, 2) == 0) ? 0 : quarterWidth;
        float yOffset = (Random.Range(0, 2) == 0) ? 0 : quarterHeight;

        // Set the final position within the screen space
        float finalX = baseX + xOffset;
        float finalY = baseY + yOffset;

        print(finalX + " x " + finalY + " y");

        // Step 5: Set the position and apply rotation
        rectTransform.anchoredPosition = new Vector2(finalX, finalY);
        rectTransform.localRotation = Quaternion.Euler(0, 0, Random.Range(-randomAngle, randomAngle));
    }

    public void SetTextSize(float size)
    {
        textNotification.fontSize = size;
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

    private IEnumerator FadeAfter(float duration, float fadeDuration)
    {
        float time = duration;

        while (time > 0)
        {
            time -= Time.deltaTime;

            if (time < fadeDuration)
            {
                notificationCanvasGroup.alpha = time / defaultFadeDuration;
            }

            yield return null;
        }
    }

    private void ResetFade()
    {
        notificationCanvasGroup.alpha = 1;
    }
}
