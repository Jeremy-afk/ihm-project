using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private TextMeshProUGUI timerText;

    private bool isTimerRunning = false;
    private float timer = 0;

    private void Awake()
    {
        timerText = GetComponent<TextMeshProUGUI>();
        timerText.alpha = 0;
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            timerText.text = $"{timer:0.0}";
            timer += Time.deltaTime;
        }

    }

    public void StartTimer()
    {
        timerText.alpha = 1;
        isTimerRunning = true;
    }

    public void StopTimer()
    {
        isTimerRunning = false;
    }

    public void ResetTimer()
    {
        timer = 0;
        timerText.alpha = 0;
    }

    public void ToogleVisibility(bool visible)
    {
        if (visible)
        {
            timerText.alpha = 1;
        }
        else
        {
            timerText.alpha = 0;
        }
    }
}
