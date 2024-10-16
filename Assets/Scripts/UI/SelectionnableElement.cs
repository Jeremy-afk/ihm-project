using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionnableElement : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private bool overrideElementSemiWidth = false;
    [SerializeField] private int elementSemiWidth = 380;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public float GetElementSemiWidth()
    {
        if (overrideElementSemiWidth)
            return elementSemiWidth;
        else
        {
            return rectTransform.rect.width / 2;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (EventSystem.current.currentSelectedGameObject != gameObject)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }
}
