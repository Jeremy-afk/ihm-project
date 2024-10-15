using UnityEngine;

public class FishNet : MonoBehaviour
{
    private InputActionsAsset controls;

    //[SerializeField] private Difficulty[] difficulties; deprecated

    [Space]

    [SerializeField] private float maxTimeBelowZeroTolerance = 1f;
    [SerializeField] private float minTimeBelowZeroTolerance = 0.5f;
    [SerializeField] private float netMovingSpeed;

    [SerializeField] private float rateOfLoss;
    [SerializeField] private float rateOfLossClampingFactor = 1f;
    [SerializeField] private float rateOfGain;
    [SerializeField] private float rateOfGainClampingFactor = 1f;

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

        print("Fetching difficulty...");
        // Currently working to move this to the game loader
        GameObject gameManagerGo = GameObject.Find("Game Manager");
        if (gameManagerGo && gameManagerGo.TryGetComponent(out GameManager gameManager))
        {
            difficulty = gameManager.difficulty;
        }
        else
        {
            Debug.LogError("There is no game manager on the scene, or it doesn't have the game manager script. Also check the spelling: 'Game Manager'");
        }

        int difficultyLevel = 0;

        switch (difficulty)
        {
            case GameManager.GameDifficulty.Hard:
                difficultyLevel = 2;
                break;
            case GameManager.GameDifficulty.Medium:
                difficultyLevel = 1;
                break;
            case GameManager.GameDifficulty.Easy:
                difficultyLevel = 0;
                break;
        }

        print("Requested difficulty " + difficultyLevel);

        //LoadDifficulty(difficulties[difficultyLevel]); deprecated
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

    private void LoadDifficulty(Difficulty difficulty)
    {
        // Deprecated
        print("Loading difficulty " + difficulty.Name + "...");
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
