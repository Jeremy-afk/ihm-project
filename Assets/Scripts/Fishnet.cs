using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Windows.Speech;

public class Fishnet : MonoBehaviour
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
    // Start is called before the first frame update
    void Start()
    {
        bar = barObject.GetComponent<BarVisual>();
        bar.SetValue(0.5f);
        fishCollider = GameObject.FindGameObjectWithTag("Fish").GetComponent<CapsuleCollider2D>();
        collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        direction = controls.Fishing.Movecursor.ReadValue<Vector2>();
        transform.Translate(direction * Time.deltaTime * speed);

        if(!colliding)
        {
            bar.AddValue(-Time.deltaTime * rateOfLoss);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Fish"))
        {
            bar.AddValue(+Time.deltaTime * rateOfGain);
            Debug.Log("Fish is still inside the net.");
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
