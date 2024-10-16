using UnityEngine;

public class FishAnimation : MonoBehaviour
{
    [SerializeField]
    private RectTransform icon;
    [SerializeField]
    private float amplitude = 50f;
    [SerializeField]
    private bool invertMotion = false;
    [SerializeField]
    private Anchor anchorDirection = Anchor.Right;
    [SerializeField]
    private float frequency = 1f;

    private float baseXPosition = 0f;
    private float timeOffset;

    private enum Anchor
    {
        Left,
        Right,
        Middle
    }

    void Start()
    {
        // Store the initial X position of the fish (in case it's not at 0)
        if (icon != null)
        {
            baseXPosition = icon.anchoredPosition.x;
        }

        // Randomize the time offset to make the movement more dynamic
        timeOffset = Random.Range(0f, Mathf.PI * 2f);
    }

    void Update()
    {
        float value = Mathf.Sin(Time.time * frequency + timeOffset);
        float motion = Mathf.Abs(value);
        if (invertMotion) motion = 1 - motion;

        float anchorOffset = 0;

        switch (anchorDirection)
        {
            case Anchor.Left:
                anchorOffset = icon.rect.width / 2;
                break;
            case Anchor.Right:
                anchorOffset = - icon.rect.width / 2;
                motion *= -1;
                break;
            case Anchor.Middle:
                motion -= .5f;
                break;
        }

        motion *= amplitude;

        if (icon != null)
        {
            Vector2 newPosition = icon.anchoredPosition;
            newPosition.x = baseXPosition + anchorOffset + motion;
            icon.anchoredPosition = newPosition;
        }
    }
}
