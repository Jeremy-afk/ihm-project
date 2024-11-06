using System.Collections;
using UnityEngine;

public class BlinkAnimation : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [Header("Animation")]
    [SerializeField] private float blinksPerSecond;
    [SerializeField] private bool snapBlink = false;
    [Range(0f, 1f)]
    [SerializeField] private float snapThreshold = 0.5f;

    [Header("Start Behaviour")]
    [Tooltip("Is not affected by time scale")]
    [SerializeField] private bool ignoreTimeScale = false;
    [SerializeField] private bool autoStartOnEnable = false;
    [SerializeField] private bool restartOnEnable = false;

    private float blinksPerSecondRadian;
    private bool started = false;
    private float timer = 0;

#if UNITY_EDITOR
    private void OnValidate()
    {
        blinksPerSecondRadian = blinksPerSecond / Mathf.PI;
    }

#endif

    private void Awake()
    {
        blinksPerSecondRadian = blinksPerSecond / Mathf.PI;
    }

    private void OnEnable()
    {
        if (autoStartOnEnable)
        {
            StartBlink(restartOnEnable);
        }

        if (restartOnEnable)
        {
            timer = 0;
        }
    }

    private void OnDisable()
    {
        StopBlink();
    }

    public void StartBlink(bool restartTime = false)
    {
        
        timer = restartTime ? 0 : timer;
        StartCoroutine(Blink());
    }

    public void StopBlink()
    {
        StopAllCoroutines();
        started = false;
    }

    private IEnumerator Blink()
    {
        if (started) yield break;

        started = true;

        while (started)
        {
            // Make the canvas group blink
            float alpha = Mathf.Abs(Mathf.Cos(timer / blinksPerSecondRadian));
            canvasGroup.alpha = snapBlink ? SnapAlpha(alpha) : alpha;

            timer += (ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime);
            yield return null;
        }
    }

    private float SnapAlpha(float alpha)
    {
        if (alpha > snapThreshold)
        {
            return 1;
        }

        return 0;
    }
}
