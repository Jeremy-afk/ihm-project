using System;
using UnityEngine;

[Serializable]
public struct FishNetProperties
{
    public float maxTimeBelowZeroTolerance;
    public float minTimeBelowZeroTolerance;
    public float netMovingSpeed;
    public float rateLoss;
    public float rateOfLossClampingFactor;
    public float rateOfGain;
    public float rateOfGainClampingFactor;
}

public class FishNet : MonoBehaviour
{
    private InputActionsAsset controls;

    [Space]

    [SerializeField] private float maxTimeBelowZeroTolerance = 1f;
    [SerializeField] private float minTimeBelowZeroTolerance = 0.5f;
    [SerializeField] private float netMovingSpeed;

    [SerializeField] private float rateOfLoss = 0.15f;
    [SerializeField] private float rateOfLossClampingFactor = 0f;
    [SerializeField] private float rateOfGain = 0.15f;
    [SerializeField] private float rateOfGainClampingFactor = 0f;

    [SerializeField] private BarVisual bar;
    [SerializeField] private Casting castingSystem;

    private Vector2 direction;
    private CapsuleCollider2D fishCollider;
    private BoxCollider2D boxFishNetCollider;
    
    private float timeColliding = 0f;
    private float hookLevel = 0.5f;
    private float hookTimeAllowedBelowZero;
    private bool colliding;
    private bool ended = false;

    private GameManager.GameDifficulty difficulty;

    private void Awake()
    {
        controls = new InputActionsAsset();
    }

    private void OnEnable()
    {
        controls.Enable();
        bar.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        controls.Disable();
        if (bar != null)
            bar.gameObject.SetActive(false);
    }

    private void Start()
    {
        hookTimeAllowedBelowZero = maxTimeBelowZeroTolerance;
        bar.SetValue(0.5f);
        fishCollider = GameObject.FindGameObjectWithTag("Fish").GetComponent<CapsuleCollider2D>();
        boxFishNetCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (ended) return;

        direction = controls.Fishing.Movecursor.ReadValue<Vector2>();
        transform.Translate(direction * Time.deltaTime * netMovingSpeed);

        ManageHookLevel();
        UpdateHookBarVisual();

        timeColliding += Time.deltaTime;
    }

    public void SetFishNetProperties(FishNetProperties properties)
    {
        maxTimeBelowZeroTolerance = properties.maxTimeBelowZeroTolerance;
        minTimeBelowZeroTolerance = properties.minTimeBelowZeroTolerance;
        netMovingSpeed = properties.netMovingSpeed;
        rateOfLoss = properties.rateLoss;
        rateOfLossClampingFactor = properties.rateOfLossClampingFactor;
        rateOfGain = properties.rateOfGain;
        rateOfGainClampingFactor = properties.rateOfGainClampingFactor;
    }

    private void ManageHookLevel()
    {
        if (colliding)
        {
            hookLevel += Time.deltaTime * rateOfGain / (1 + timeColliding * rateOfGainClampingFactor);

            hookTimeAllowedBelowZero = Mathf.Max(hookTimeAllowedBelowZero, minTimeBelowZeroTolerance);
        }
        else
        {
            hookLevel -= Time.deltaTime * rateOfLoss / (1 + timeColliding * rateOfLossClampingFactor);

            if (hookLevel <= 0)
            {
                hookTimeAllowedBelowZero -= Time.deltaTime;
            }
        }

        if (hookLevel >= 1)
        {
            HookSuccessful();
        }
        else if (hookTimeAllowedBelowZero <= 0)
        {
            Escapes();
        }

        hookLevel = Mathf.Clamp01(hookLevel);
    }

    private void HookSuccessful()
    {
        ended = true;
        castingSystem.HookSuccessful();
    }

    private void Escapes()
    {
        castingSystem.FishEscaped();
        ended = true;
    }

    private void UpdateHookBarVisual()
    {
        bar.SetValue(hookLevel);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fish"))
        {
            timeColliding = 0;
            colliding = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Fish"))
        {
            timeColliding = 0;
            colliding = false;
        }
    }
}
