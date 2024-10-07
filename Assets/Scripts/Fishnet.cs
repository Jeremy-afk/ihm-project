using UnityEngine;

public class FishNet : MonoBehaviour
{
    private InputActionsAsset controls;

    [SerializeField] private float speed;

    [SerializeField] private float rateOfLoss;
    [SerializeField] private float rateOfGain;

    [SerializeField] private BarVisual bar;

    private Vector2 direction;
    private CapsuleCollider2D fishCollider;
    private BoxCollider2D boxFishNetCollider;

    private float hookLevel = .5f;

    private bool colliding;


    private void Awake()
    {
        controls = new InputActionsAsset();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Start()
    {
        bar.SetValue(0.5f);
        fishCollider = GameObject.FindGameObjectWithTag("Fish").GetComponent<CapsuleCollider2D>();
        boxFishNetCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        direction = controls.Fishing.Movecursor.ReadValue<Vector2>();
        transform.Translate(direction * Time.deltaTime * speed);

        ManageHookLevel();
        UpdateHookBarVisual();
    }

    public void FishBitesBait()
    {

    }

    private void ManageHookLevel()
    {
        if(colliding)
            hookLevel += Time.deltaTime * rateOfGain;
        else
            hookLevel -= Time.deltaTime * rateOfLoss;
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
