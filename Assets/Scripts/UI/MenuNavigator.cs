using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuNavigator : MonoBehaviour
{
    [SerializeField]
    private RectTransform fishIcon;
    [SerializeField]
    private float offsetX = 50f;
    [SerializeField]
    private float transitionSpeed = 10f;

    private RectTransform currentButtonRect; // Currently selected button RectTransform
    private GameObject lastSelectedButton;  // Tracks the last selected button
    private Vector3 targetPosition;

    private bool initialized = false;

    private IEnumerator Start()
    {
        fishIcon.gameObject.SetActive(false);

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
            print(selectedButton.name);

            // This getcomponent call in update is expensive but since it's only called once when the selected button changes, it's fine
            currentButtonRect = selectedButton.GetComponent<RectTransform>();
            
            targetPosition = new Vector3(currentButtonRect.position.x - offsetX, currentButtonRect.position.y, currentButtonRect.position.z);
            lastSelectedButton = selectedButton;
        }

        if (currentButtonRect != null)
        {
            fishIcon.position = Vector3.Lerp(fishIcon.position, targetPosition, Time.deltaTime * transitionSpeed);
        }
    }
}
