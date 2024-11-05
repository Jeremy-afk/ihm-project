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
    private Color defaultColor = Color.yellow;
    [SerializeField]
    private Color lowColor = Color.red;
    [SerializeField]
    private Color highColor = Color.green;
    [SerializeField]
    private bool fixSaturation = false;
    [SerializeField, Range(0f, 1f)]
    private float saturation = 0.5f;
    [SerializeField, Range(0f, 1f)]
    private float value = 0.5f;

    [Header("Debug")]
    [SerializeField, Tooltip("[Space] to drain the value\n[Left Click] to refill with steps")]
    private bool debugMode = false;
    [SerializeField]
    private InputActionAsset playerInput;

    private InputAction debugActionDrain;
    private InputAction debugActionAdd;
    private float m_currentValue;

    private bool useColor;
    private float maxHue;
    private float minHue;

    private void OnEnable()
    {
        if (!debugMode) return;

        // Debug shit
        var debugActionAsset = playerInput.FindActionMap("Debug");
        debugActionAdd = debugActionAsset.FindAction("AddHookPower");
        debugActionDrain = debugActionAsset.FindAction("DecreaseHookPower");
        debugActionAdd.Enable();
        debugActionDrain.Enable();
        debugActionAdd.performed += AddValue;
    }

    private void Start()
    {
        useColor = PlayerPrefs.GetInt("FixedBarColor") == 1;

        Color.RGBToHSV(lowColor, out minHue, out _, out _);
        Color.RGBToHSV(highColor, out maxHue, out _, out _);
        SetValue(startValue);
    }

    private void Update()
    {
        if (debugMode && debugActionDrain.IsPressed())
        {
            SetValue(m_currentValue - Time.deltaTime/3);
        }
    }

    public void FixColor(bool fix)
    {
        PlayerPrefs.SetInt("FixedBarColor", fix ? 1 : 0);

        if (fix)
        {
            barFill.color = defaultColor;
        }
    }

    // Should be between 0 and 1
    public void SetValue(float val)
    {
        val = Mathf.Clamp01(val);
        m_currentValue = val;
        barFill.fillAmount = val;

        if (!useColor) return;

        if (fixSaturation)
        {
            barFill.color = Color.HSVToRGB(Mathf.Lerp(minHue, maxHue, m_currentValue*m_currentValue), saturation, value);
        }
        else
        {
            Color color = Color.Lerp(lowColor, highColor, val);
            barFill.color = color;
        }
    }

    private void AddValue(InputAction.CallbackContext context)
    {
        SetValue(m_currentValue + .05f);
    }
}
