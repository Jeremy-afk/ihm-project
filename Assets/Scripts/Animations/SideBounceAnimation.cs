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
    private float frequency = 1f;
    [SerializeField]
    private float baseXPosition = 0f;
    [SerializeField]
    private bool activateOnStart = true;

    private float timeOffset;

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
        float value = Mathf.Sin(Time.time * frequency + timeOffset) * amplitude;
        float motion = Mathf.Abs(value) * (invertMotion ? -1 : 1);

        if (icon != null)
        {
            Vector2 newPosition = icon.anchoredPosition;
            newPosition.x = baseXPosition + motion;
            icon.anchoredPosition = newPosition;
        }
    }
}
