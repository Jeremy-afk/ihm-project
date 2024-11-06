using UnityEngine;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainPauseMenu;
    [SerializeField]
    private GameObject preferencesMenu;

    [Space]

    [SerializeField]
    private Toggle toogleFixedColorBar;
    [SerializeField]
    private Toggle toogleFixedSquareColor;
    [SerializeField]
    private Toggle toogleCheeringBoy;

    private void OnEnable()
    {
        LoadPreferences();
    }

    public void ShowMainPauseMenu()
    {
        mainPauseMenu.SetActive(true);
        preferencesMenu.SetActive(false);
    }

    public void ShowPreferencesMenu()
    {
        mainPauseMenu.SetActive(false);
        preferencesMenu.SetActive(true);
    }

    public void LoadPreferences()
    {
        toogleCheeringBoy.isOn = PlayerPrefs.GetInt("CheeringBoy") == 1;
        toogleFixedSquareColor.isOn = PlayerPrefs.GetInt("FixedColorSquare") == 1;
        toogleFixedColorBar.isOn = PlayerPrefs.GetInt("FixedColorBar") == 1;
    }
}
