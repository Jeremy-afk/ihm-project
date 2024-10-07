using UnityEngine;
using UnityEngine.InputSystem;

public class Casting : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float forceSpeed;
    [SerializeField] private float distanceMultiplier;
    [SerializeField] private GameObject target;

    private float angle;
    private float strenght;

    private bool isAngleChosen;
    private bool isStrenghtChosen;
    private bool hasSpawned;

    private SpriteRenderer spriteRenderer;

    private Vector2 castPosition;
    public Vector2 CastPosition { 
        get { return castPosition; }
        private set { castPosition = value; }
    }

    private InputActionsAsset controls;

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
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.size = new Vector2(1, 1);
    }

    private void Update()
    {
        target.transform.position = castPosition;
        if(!isAngleChosen)
        {
            ChoseAngle();
        }

        if (isAngleChosen && !isStrenghtChosen)
        {
            ChoseStrength();
        }

        if(isAngleChosen && isStrenghtChosen && !hasSpawned) 
        { 
            CreateTarget();
            spriteRenderer.enabled = false;
        }
    }

    private void ChoseAngle()
    {
        angle = 90 * Mathf.Cos(Time.time * rotationSpeed);
        transform.rotation = Quaternion.Euler(0,0,angle);
        print(angle);
    }

    private void ChoseStrength()
    {
        strenght = 0.5f + 0.5f * Mathf.Cos(Time.time * forceSpeed);
        spriteRenderer.size = new Vector2(1,strenght);
    }

    private void OnCasting(InputAction.CallbackContext context)
    {
        if (!isAngleChosen)
        {
            isAngleChosen = true;
            Debug.Log("Angle chosen: " + angle);
        }
        else if(!isStrenghtChosen)
        {
            isStrenghtChosen = true;
            Debug.Log("Strength chosen: " + strenght);
        }
        else
        {
            Debug.Log("Nothing");
        }
    }

    private void CreateTarget()
    {
        float x = -distanceMultiplier * strenght * Mathf.Sin((Mathf.PI / 180) * angle);
        float y = distanceMultiplier * strenght * Mathf.Cos((Mathf.PI / 180) * angle);
        CastPosition = new Vector3(transform.position.x + x, transform.position.y + y, 0);
        print(castPosition);
        Instantiate(target, castPosition, Quaternion.identity);
        hasSpawned = true;
    }
}
