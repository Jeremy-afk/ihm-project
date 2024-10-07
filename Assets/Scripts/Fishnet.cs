using UnityEngine;

public class FishNet : MonoBehaviour
{
    private InputActionsAsset controls;

    [SerializeField] private float maxTimeBelowZeroTolerance = 1f;
    [SerializeField] private float minTimeBelowZeroTolerance = 0.5f;
    [SerializeField] private float netMovingSpeed;

    [SerializeField] private float rateOfLoss;
    [SerializeField] private float rateOfGain;

    [SerializeField] private BarVisual bar;
    [SerializeField] private Casting castingSystem;

    private Vector2 direction;
    private CapsuleCollider2D fishCollider;
    private BoxCollider2D boxFishNetCollider;
    

    private float hookLevel = 0.5f;
    private float hookTimeAllowedBelowZero;
    private bool colliding;
    private bool ended = false;

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
        print("Direction: " + direction.x + " x ; " + direction.y + " y");
        transform.Translate(direction * Time.deltaTime * netMovingSpeed);

        ManageHookLevel();
        UpdateHookBarVisual();
    }

    private void ManageHookLevel()
    {
        if (colliding)
        {
            hookLevel += Time.deltaTime * rateOfGain;

            hookTimeAllowedBelowZero = Mathf.Max(hookTimeAllowedBelowZero, minTimeBelowZeroTolerance);
        }
        else
        {
            hookLevel -= Time.deltaTime * rateOfLoss;

            if (hookLevel <= 0)
            {
                hookTimeAllowedBelowZero -= Time.deltaTime;
            }
        }

        if (hookTimeAllowedBelowZero <= 0)
        {
            Escapes();
        }

        hookLevel = Mathf.Clamp01(hookLevel);
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
            colliding = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Fish"))
        {
            colliding = false;
        }
    }
}
