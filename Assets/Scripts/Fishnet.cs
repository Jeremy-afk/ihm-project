using System;
using System.Collections;
using UnityEngine;
using System.IO.Ports;
using System.Globalization;

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
    [SerializeField] private Color hookingColor = Color.green;
    [SerializeField] private Color offHookColor = Color.red;

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
    [SerializeField] private NotificationAlert contextualHelpMoveBottom;
    [SerializeField] private NotificationAlert contextualHelpMoveUp;

    private Vector2 direction;
    private CapsuleCollider2D fishCollider;
    private BoxCollider2D boxFishNetCollider;
    private SpriteRenderer spriteRenderer;

    private Vector2 inputDirection;
    private float timeColliding = 0f;
    private float hookLevel = 0.5f;
    private float hookTimeAllowedBelowZero;
    private float ardunioCooldown;
    private bool colliding;
    private bool started = false;
    private bool ended = false;
    private bool showContextualHelp = false;

    private GameManager.GameDifficulty difficulty;

    private bool fixedColorSquare;

    //Arduino stuff
    [Header("Arduino stuff")]
    [SerializeField] private string portName = "COM4"; // Nom du port série
    [SerializeField] private int baudRate = 9600; // Baud rate
    [SerializeField] private int dataSendRate = 20; // Arduino send rate
    [SerializeField] private int maxInstructionsPerFrame = 1;
    [SerializeField] private bool overrideInputWithArduino;
    private bool isArduinoButtonPressed;
    private SerialPort serialPort;

    private void Awake()
    {
        controls = new InputActionsAsset();
    }

    private void OnDisable()
    {
        controls.Disable();
        if (bar != null)
            bar.gameObject.SetActive(false);
    }

    private void Start()
    {
        serialPort = new SerialPort(portName, baudRate);
        serialPort.ReadTimeout = 300;
        serialPort.WriteTimeout = 300;
        serialPort.Open();

        fixedColorSquare = PlayerPrefs.GetInt("FixedColorSquare") == 1;

        hookTimeAllowedBelowZero = maxTimeBelowZeroTolerance;
        bar.SetValue(0.5f);
        boxFishNetCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.enabled = false;
        ChangeSquareColor(true);
    }

    private void Update()
    {
        if (!started || ended) return;

        if (overrideInputWithArduino)
        {
            GetArduinoInput();
        }
        else inputDirection = controls.Fishing.Movecursor.ReadValue<Vector2>();

        transform.Translate(netMovingSpeed * Time.deltaTime * inputDirection);

        ManageHookLevel();
        UpdateHookBarVisual();

        if (ardunioCooldown <= 0)
        {
            SendDataToArduino();
            ardunioCooldown = 1f / dataSendRate; // Cooldown
        }

        ardunioCooldown -= Time.deltaTime;
        timeColliding += Time.deltaTime;
    }

    private void GetArduinoInput()
    {
        int instructions = 0;

        while (serialPort != null && serialPort.IsOpen && serialPort.BytesToRead > 0 && instructions < maxInstructionsPerFrame)
        {
            string inputData = serialPort.ReadLine();
            if (inputData.Length < 4)
            {
                Debug.LogWarning("Received data too short: " + inputData);
                continue;
            }

            string inputType = inputData[..4];
            inputData = inputData[4..];

            switch(inputType)
            {
                case "JOY:":
                    string[] values = inputData.Split(',');

                    if (values.Length == 2)
                    {
                        float x = float.Parse(values[0], CultureInfo.InvariantCulture);
                        float y = -float.Parse(values[1], CultureInfo.InvariantCulture);

                        inputDirection = new Vector2(x, y);
                    }
                    else
                    {
                        Debug.LogWarning("Received unexpected JOY format: " + inputData);
                    }
                    break;

                case "BTN:":
                    isArduinoButtonPressed = inputData == "1";
                    print("button state changed: " + isArduinoButtonPressed);
                    break;

                default:
                    Debug.LogWarning($"Received unexpected input type: {inputType}({inputData})");
                    break;
            }

            instructions++;
        }
    }

    public void FixColorSquare(bool fix)
    {
        PlayerPrefs.SetInt("FixedColorSquare", fix ? 1 : 0);
        fixedColorSquare = fix;

        if (fix)
        {
            spriteRenderer.color = hookingColor;
        }
    }

    public void ActivateFishNet()
    {
        fishCollider = GameObject.FindGameObjectWithTag("Fish").GetComponent<CapsuleCollider2D>();
        controls.Enable();
        bar.gameObject.SetActive(true);
        spriteRenderer.enabled = true;
        started = true;

        if (showContextualHelp)
        {
            // Show the text that explains how to move !
            StartCoroutine(ContextualHelpTimer());
        }
    }

    public void SetFishNetProperties(FishNetProperties properties, bool showContextualHelp = false)
    {
        maxTimeBelowZeroTolerance = properties.maxTimeBelowZeroTolerance;
        minTimeBelowZeroTolerance = properties.minTimeBelowZeroTolerance;
        netMovingSpeed = properties.netMovingSpeed;
        rateOfLoss = properties.rateLoss;
        rateOfLossClampingFactor = properties.rateOfLossClampingFactor;
        rateOfGain = properties.rateOfGain;
        rateOfGainClampingFactor = properties.rateOfGainClampingFactor;

        this.showContextualHelp = showContextualHelp;
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

    private void SendDataToArduino()
    {
        // Envoi de la valeur de `hookLevel` au port série pour Arduino
        if (serialPort != null && serialPort.IsOpen)
        {
            string data = "LED:" + hookLevel.ToString("0.00", CultureInfo.InvariantCulture); // Formate `hookLevel` avec deux décimales
            serialPort.WriteLine(data); // Envoie de la donnée
        }
    }

    private void HookSuccessful()
    {
        ended = true;
        castingSystem.HookSuccessful();

        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.WriteLine("0.5");
            serialPort.Close();
        }
    }

    private void Escapes()
    {
        castingSystem.FishEscaped();
        ended = true;

        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.WriteLine("0.5");
            serialPort.Close();
        }
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
            ChangeSquareColor(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Fish"))
        {
            timeColliding = 0;
            colliding = false;
            ChangeSquareColor(false);
        }
    }

    private void ChangeSquareColor(bool on)
    {
        if (fixedColorSquare) return;

        spriteRenderer.color = on ? hookingColor : offHookColor;
    }

    private IEnumerator ContextualHelpTimer()
    {
        contextualHelpMoveBottom.NewNotification("Move the Fish Net with the stick !", ButtonReference.LStick, 5, 1);

        yield return new WaitForSeconds(6);

        contextualHelpMoveUp.NewNotification("Fill the bar to the top to catch the fish !", ButtonReference.None, 5, 1);
    }
}
