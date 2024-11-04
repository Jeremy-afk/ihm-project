using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[Serializable]
public struct CastingProperties
{
    public float baitMaxDistance;
    public float rotationSpeed;
    public float forceSpeed;

    public CastingProperties(float baitMaxDistance = 2f, float rotationSpeed = 2f, float forceSpeed = 2f)
    {
        this.baitMaxDistance = baitMaxDistance;
        this.rotationSpeed = rotationSpeed;
        this.forceSpeed = forceSpeed;
    }
}

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
    [SerializeField] private PulseAnimation target;
    [SerializeField] private NotificationAlert notification;
    [SerializeField] private FishPool fishPool;
    [SerializeField] private FishNet fishNet;
    [SerializeField] private Timer timerWidget;

    private float timer;
    private float angle;
    private float strenght = .5f;
    private Vector2 castPosition;
    private CastingState castingState = CastingState.Initial;


    private CameraFollow cameraFollow;
    private Fish fish;
    private PulseAnimation instantiatedTarget;
    private SpriteRenderer spriteRenderer;
    private InputActionsAsset controls;

    private PlayerInput playerInput;

    private bool isGamePaused;
    [SerializeField] GameObject pauseMenu;

    //[SerializeField] private GameObject audioManager;

    private enum CastingState
    {
        Initial,
        ChoosingAngle,
        ChoosingStrength,
        Casted,
        FishBitingBait,
        FishEscaping,
        FishCaptured,
        WaitGameOver,
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
        controls.Fishing.Pause.performed += OnPause;
    }

    private void OnDisable()
    {
        controls.Fishing.SelectCasting.performed -= OnCasting;
        controls.Fishing.SelectCasting.performed -= OnPause;
        controls.Disable();
    }

    private void Start()
    {
        Camera.main.TryGetComponent(out cameraFollow);
        cameraFollow.SetPrimaryTarget(transform);

        instantiatedTarget = Instantiate(target, Vector3.zero, Quaternion.identity);
        instantiatedTarget.gameObject.SetActive(false);
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.size = new Vector2(1, .5f);
        spriteRenderer.enabled = false;

        playerInput = GetComponent<PlayerInput>();

        print(playerInput.currentControlScheme);

        if (playerInput.currentControlScheme == "Switch Controller" || playerInput.currentControlScheme == "Xbox Controller")
        {
                notification.NewNotification("Press Button to start!", ButtonReference.A, 0);
        } 
        else if (playerInput.currentControlScheme == "Keyboard & Mouse") 
        {
            notification.NewNotification("Press Button to start!", ButtonReference.Space, 0);
        }
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

    #region Public Methods
    public void SetCastingProperties(CastingProperties properties)
    {
        baitMaxDistance = properties.baitMaxDistance;
        rotationSpeed = properties.rotationSpeed;
        forceSpeed = properties.forceSpeed;
    }

    public Vector2 GetNetPosition()
    {
        return fishNet.transform.position;
    }

    public void FishBitesBait()
    {
        AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.fishBites);
        castingState = CastingState.FishBitingBait;
        CreateHookEvent();
    }

    public void FishFinishedTheBait()
    {
        notification.ToogleNotification(false);
        StartCoroutine(WaitGameOver("Too slow..."));
    }

    public void FishEscaped()
    {
        fish.Release();
        cameraFollow.ToogleFollow(false);
        StartCoroutine(WaitGameOver("The fish escaped."));
    }

    public void HookSuccessful()
    {
        timerWidget.StopTimer();
        castingState = CastingState.FishCaptured;
        AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.winSound);
        AudioManager.Instance.PauseMusic();
        fish.Capture();
        cameraFollow.ToogleDynamicMode(false);
        cameraFollow.ToogleFollow(false);
        StartCoroutine(AnimateWin());
    }

    #endregion

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
        if (playerInput.currentControlScheme == "Switch Controller" || playerInput.currentControlScheme == "Xbox Controller")
        {
            notification.NewNotification("Press Button !", ButtonReference.A, 0);
        }
        else if (playerInput.currentControlScheme == "Keyboard & Mouse")
        {
            notification.NewNotification("Press Button !", ButtonReference.Space, 0);
        }
    }

    private void BeginFishNetMinigame()
    {
        castingState = CastingState.FishEscaping;
        AudioManager.Instance.PlayMusic(AudioManager.Instance.chaseTheme);
        fish.Hook();
        fish.tag = "Fish";
        fishNet.transform.position = fish.transform.position;
        fishNet.ActivateFishNet();
        notification.NewNotification("HIT !", ButtonReference.None, .8f);

        instantiatedTarget.gameObject.SetActive(false);

        cameraFollow.SetPrimaryTarget(fishNet.transform);
        cameraFollow.ToogleDynamicMode(true, fish.transform);
        cameraFollow.ToogleFollow(true);

        timerWidget.StartTimer();
        fishPool.FleeAllFishes(fish);
    }

    private void OnCasting(InputAction.CallbackContext context)
    {
        if (isGamePaused) return;

        switch (castingState)
        {
            case CastingState.Initial:
                notification.ToogleNotification(false);
                spriteRenderer.enabled = true;
                instantiatedTarget.gameObject.SetActive(true);
                timer = 0;
                castingState = CastingState.ChoosingAngle;
                break;

            case CastingState.ChoosingAngle:
                timer = 0;
                castingState = CastingState.ChoosingStrength;
                break;

            case CastingState.ChoosingStrength:
                spriteRenderer.enabled = false;
                instantiatedTarget.Pulse();
                castingState = CastingState.Casted;
                PostBaitInPool();
                break;

            case CastingState.Casted:
            case CastingState.FishCaptured:
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
            StartCoroutine(WaitGameOver("Too far from a fish."));
        }
    }

    private IEnumerator WaitGameOver(string msg)
    {
        timerWidget.StopTimer();

        if (castingState == CastingState.GameOver || castingState == CastingState.WaitGameOver)
            yield break;

        castingState = CastingState.WaitGameOver;
        yield return new WaitForSeconds(timeDelayBeforeGameOver);

        notification.NewNotification("Game Over !\n" + msg, ButtonReference.None, 0);
        AudioManager.Instance.PauseMusic();
        AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.gameOverSound);
        castingState = CastingState.GameOver;
    }

    private IEnumerator AnimateWin()
    {
        yield return new WaitForSeconds(timeDelayBeforeGameOver);

        notification.NewNotification("You won !\nYou caught the fish !", ButtonReference.None, 0);

        castingState = CastingState.GameOver;
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        if (isGamePaused)
            Unpause();
        else
            Pause();
    }

    public void Pause()
    {
        if(!isGamePaused)
        {
            pauseMenu.SetActive(true);
            isGamePaused = !isGamePaused;
            Time.timeScale = 0;
        }
    }

    public void Unpause()
    {
        if (isGamePaused)
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
            isGamePaused = !isGamePaused;
        }
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
}
