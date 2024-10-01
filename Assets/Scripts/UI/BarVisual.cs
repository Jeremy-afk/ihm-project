using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BarVisual : MonoBehaviour
{
    [SerializeField]
    private float startValue = .75f;

    [SerializeField]
    private Image barFill;
    [SerializeField]
    private Color lowColor = Color.red;
    [SerializeField]
    private Color highColor = Color.green;

    [Header("Debug")]
    [SerializeField, Tooltip("[Space] to drain the value\n[Left Click] to refill with steps")]
    private bool debugMode = false;
    [SerializeField]
    private InputActionAsset playerInput;

    private InputAction debugActionDrain;
    private InputAction debugAction;
    private float m_currentValue;
    public float currentValue {
        get { return m_currentValue; } 
    }

    private void OnEnable()
    {
        if (!debugMode) return;

        // Debug shit
        var debugActionAsset = playerInput.FindActionMap("Debug");
        debugAction = debugActionAsset.FindAction("AddHookPower");
        debugActionDrain = debugActionAsset.FindAction("DecreaseHookPower");
        debugAction.Enable();
        debugActionDrain.Enable();
        debugAction.performed += AddValue;
    }

    private void Start()
    {
        SetValue(startValue);
    }

    private void Update()
    {
        if (debugMode && debugActionDrain.IsPressed())
        {
            SetValue(m_currentValue - Time.deltaTime/3);
        }
    }

    // Should be between 0 and 1
    public void SetValue(float value)
    {
        value = Mathf.Clamp01(value);
        m_currentValue = value;
        barFill.fillAmount = value;

        barFill.color = Color.Lerp(lowColor, highColor, value);
    }

    private void AddValue(InputAction.CallbackContext context)
    {
        SetValue(m_currentValue + .05f);
    }

    public void AddValue(float delta)
    {
        m_currentValue += delta;
        barFill.fillAmount = m_currentValue;
        barFill.color = Color.Lerp(lowColor, highColor, m_currentValue);
    }
}
