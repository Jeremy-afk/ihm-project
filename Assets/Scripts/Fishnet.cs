using UnityEngine;

public class FishNet : MonoBehaviour
{
    private InputActionsAsset controls;

    private Vector2 direction;
    [SerializeField] private float speed;

    [SerializeField] private float rateOfLoss;
    [SerializeField] private float rateOfGain;

    [SerializeField] private GameObject barObject;
    private BarVisual bar;

    private CapsuleCollider2D fishCollider;
    private BoxCollider2D collider;

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
        bar = barObject.GetComponent<BarVisual>();
        bar.SetValue(0.5f);
        fishCollider = GameObject.FindGameObjectWithTag("Fish").GetComponent<CapsuleCollider2D>();
        collider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        direction = controls.Fishing.Movecursor.ReadValue<Vector2>();
        transform.Translate(direction * Time.deltaTime * speed);

        ManageHookLevel();
        UpdateHookBarVisual();
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

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Fish"))
        {
            hookLevel += Time.deltaTime * rateOfGain;
            //Debug.Log("Fish is still inside the net.");
        }
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
