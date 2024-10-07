using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Casting : MonoBehaviour
{
    [Header("Fish Rod Effects")]
    [SerializeField] private float timeDelayBeforeGameOver = 1.5f;

    [Header("Fish Rod Effects")]
    [SerializeField] private float baitMaxDistance = 2f;

    [Header("Fish Rod Launch")]
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float forceSpeed;
    [SerializeField] private float distanceMultiplier;

    [Header("References")]
    [SerializeField] private GameObject target;
    [SerializeField] private NotificationAlert notification;
    [SerializeField] private FishPool fishPool;

    private float timer;
    private float angle;
    private float strenght = .5f;
    private Vector2 castPosition;
    private CastingState castingState = CastingState.Initial;

    private Fish fish;
    private GameObject instantiatedTarget;
    private SpriteRenderer spriteRenderer;
    private InputActionsAsset controls;


    private enum CastingState
    {
        Initial,
        ChoosingAngle,
        ChoosingStrength,
        Casted
    }

    private void Awake()
    {
        controls = new InputActionsAsset();
    }

    private void OnEnable()
    {
        controls.Enable();
        controls.Fishing.SelectCasting.performed += OnCasting;
    }

    private void OnDisable()
    {
        controls.Fishing.SelectCasting.performed -= OnCasting;
        controls.Disable();
    }

    private void Start()
    {
        instantiatedTarget = Instantiate(target, Vector3.zero, Quaternion.identity);
        instantiatedTarget.SetActive(false);
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.size = new Vector2(1, .5f);
        spriteRenderer.enabled = false;
    }

    private void Update()
    {
        target.transform.position = castPosition;

        switch (castingState)
        {
            case CastingState.ChoosingAngle:
                HandleAngle();
                break;

            case CastingState.ChoosingStrength:
                HandleStrength();
                break;
        }

        if ((int)castingState > 0 && (int)castingState < 3)
        {
            timer += Time.deltaTime;
            UpdateTarget();
        }
    }

    private void HandleAngle()
    {
        angle = 90 * Mathf.Sin(timer * rotationSpeed);
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void HandleStrength()
    {
        strenght = 0.5f + 0.5f * Mathf.Sin(timer * forceSpeed);
        spriteRenderer.size = new Vector2(1, strenght);
    }

    private void OnCasting(InputAction.CallbackContext context)
    {
        switch (castingState)
        {
            case CastingState.Initial:
                spriteRenderer.enabled = true;
                instantiatedTarget.SetActive(true);
                timer = 0;
                castingState += 1;
                break;

            case CastingState.ChoosingAngle:
                timer = 0;
                castingState += 1;
                break;

            case CastingState.ChoosingStrength:
                spriteRenderer.enabled = false;
                castingState += 1;
                PostBaitInPool();
                break;

            case CastingState.Casted:
                // Do nothing (discard the input)
                break;
        }
    }

    private void UpdateTarget()
    {
        float x = - distanceMultiplier * strenght * Mathf.Sin((Mathf.PI / 180) * angle);
        float y = distanceMultiplier * strenght * Mathf.Cos((Mathf.PI / 180) * angle);
        castPosition = new Vector3(transform.position.x + x, transform.position.y + y, 0);

        instantiatedTarget.transform.position = castPosition;
    }

    // Puts the bait in position in the pool 
    private void PostBaitInPool()
    {
        fish = fishPool.PutBaitInPosition(castPosition, baitMaxDistance);

        if (!fish)
        {
            StartCoroutine(WaitGameOver());
        }
    }

    private IEnumerator WaitGameOver()
    {
        yield return new WaitForSeconds(timeDelayBeforeGameOver);

        notification.NewNotification("Game Over !\nAucun poisson n'est intéressé.", ButtonReference.None, 0);
    }
}
