using UnityEngine;
using UnityEngine.EventSystems;

public class MenuContext : MonoBehaviour
{
    [SerializeField] private GameObject firstSelected;
    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }
}
