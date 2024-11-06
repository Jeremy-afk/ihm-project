using UnityEngine;

public class ObjectShake : MonoBehaviour
{
    [SerializeField]
    private float shakeSpeed = 3.0f;
    [SerializeField]
    private float shakeDuration = 10.0f;
    [SerializeField]
    private float baseShakeFactor = 2.0f;

    private Vector3 originalPosition;
    private float shakeTimeRemaining = 0.0f;
    private float shakeFactor = 1;

    private void Update()
    {
        if (shakeTimeRemaining > 0)
        {
            Vector2 randomOffset = Random.insideUnitSphere * baseShakeFactor * shakeFactor;
            transform.position = originalPosition + (Vector3)randomOffset;

            shakeTimeRemaining -= Time.deltaTime;

            if (shakeTimeRemaining <= 0)
            {
                StopShake();
            }
        }
    }

    public void StartShake(float shakeFactor)
    {
        originalPosition = transform.position;

        if (shakeFactor <= 0)
        {
            StopShake();
            return;
        }

        this.shakeFactor = shakeFactor;
        shakeTimeRemaining = shakeDuration;
    }

    // Public method to stop the shake and reset position
    public void StopShake()
    {
        transform.position = originalPosition;
        shakeTimeRemaining = 0;
        baseShakeFactor = 0;
    }
}
