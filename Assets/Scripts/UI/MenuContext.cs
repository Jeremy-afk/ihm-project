using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MenuContext : MonoBehaviour
{
    [SerializeField] private GameObject firstSelected;

    [Tooltip("Button that gets selected when the back button is hit.")]
    [SerializeField] private GameObject backButtonSelection;

    [SerializeField] private InputActionAsset inputAction;
    [SerializeField] private InputAction backAction;

    private bool inputActionLoaded = false;

    private void Awake()
    {
        if (!inputAction)
        {
            Debug.LogError("Input Action Asset is not assigned for this context menu.");
            return;
        }

        InputActionMap inputMap = inputAction.FindActionMap("Fishing");
        print(inputMap);
        backAction = inputMap.FindAction("Cancel");
        print(backAction);

        inputActionLoaded = true;
    }

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(firstSelected);
        
        if (inputActionLoaded)
        {
            print(backAction);
            backAction.Enable();
            backAction.performed += OnBackButtonPressed;
        }
    }

    private void OnDisable()
    {
        if (inputActionLoaded)
        {
            print(backAction);
            backAction.performed -= OnBackButtonPressed;
            backAction.Disable();
        }
    }

    private void OnBackButtonPressed(InputAction.CallbackContext context)
    {
        //print($"TRIGGER: {context.control}");
        if (!backButtonSelection)
        {
            Debug.LogWarning("No back button selection were made for this menu context.", gameObject);
            return;
        }

        if (EventSystem.current.currentSelectedGameObject != backButtonSelection)
        {
            EventSystem.current.SetSelectedGameObject(backButtonSelection);
        }
    }
}
