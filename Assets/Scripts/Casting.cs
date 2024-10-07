using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
    [SerializeField] private FishNet fishNet;

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
        Casted,
        FishBitingBait,
        FishEscaping,
        GameOver
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

        notification.NewNotification("Appuyer sur [ESPACE] pour commencer !", ButtonReference.None, 0);
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

    public void FishBitesBait()
    {
        castingState = CastingState.FishBitingBait;
        CreateHookEvent();
    }

    public void FishFinishedTheBait()
    {
        castingState = CastingState.GameOver;
        notification.ToogleNotification(false);
        StartCoroutine(WaitGameOver("T'es trop lent bruh"));
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

    private void CreateHookEvent()
    {
        notification.NewNotification("Appuyer sur [ESPACE]", ButtonReference.None, 0);
        // Should generate a random button to press
    }

    private void BeginFishNetMinigame()
    {
        castingState = CastingState.FishEscaping;
        fish.Hook();
        fishNet.transform.position = fish.transform.position;
        fishNet.gameObject.SetActive(true);
        notification.NewNotification("HIT !", ButtonReference.None, .8f);
    }

    private void OnCasting(InputAction.CallbackContext context)
    {
        switch (castingState)
        {
            case CastingState.Initial:
                notification.ToogleNotification(false);
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

            case CastingState.FishBitingBait:
                BeginFishNetMinigame();
                break;

            case CastingState.GameOver:
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
            StartCoroutine(WaitGameOver("Trop loin d'un poisson !"));
        }
    }

    private IEnumerator WaitGameOver(string msg)
    {
        yield return new WaitForSeconds(timeDelayBeforeGameOver);

        notification.NewNotification("Game Over !\n" + msg, ButtonReference.None, 0);
        castingState = CastingState.GameOver;
    }
}
