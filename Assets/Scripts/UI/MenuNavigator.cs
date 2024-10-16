using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuNavigator : MonoBehaviour
{
    [SerializeField]
    private RectTransform fishIcon;
    [SerializeField]
    private float globalOffsetX = 50f;
    [SerializeField]
    private float transitionSpeed = 10f;

    private RectTransform currentButtonRect; // Currently selected button RectTransform
    private GameObject lastSelectedButton;  // Tracks the last selected button
    private Vector3 targetPosition;

    private bool initialized = false;

    private void Awake()
    {
        fishIcon.gameObject.SetActive(false);
    }

    private IEnumerator Start()
    {
        // Wait a bit before setting the first selected button to let the UI EventSystem initialize
        yield return new WaitForEndOfFrame();

        fishIcon.gameObject.SetActive(true);
        initialized = true;
    }

    private void Update()
    {
        if (!initialized) return;

        GameObject selectedButton = EventSystem.current.currentSelectedGameObject;

        if (selectedButton != null && selectedButton != lastSelectedButton)
        {
            // This getcomponent call in update is expensive but since it's only called once when the selected button changes, it's fine
            currentButtonRect = selectedButton.GetComponent<RectTransform>();

            targetPosition = new Vector3(currentButtonRect.position.x - globalOffsetX, currentButtonRect.position.y, currentButtonRect.position.z);

            // Also take into account the selected button's semi-width
            if (currentButtonRect.TryGetComponent(out SelectionnableElement element))
            {
                targetPosition -= new Vector3(element.GetElementSemiWidth(), 0, 0);
            }
            else
                Debug.LogWarning("No selectable component on " + currentButtonRect.gameObject.name, currentButtonRect.gameObject);


            lastSelectedButton = selectedButton;
        }

        if (currentButtonRect != null)
        {
            fishIcon.position = Vector3.Lerp(fishIcon.position, targetPosition, Time.deltaTime * transitionSpeed);
        }
    }
}
