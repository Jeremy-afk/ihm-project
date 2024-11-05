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
    private Toggle toogleFixedBarColor;
    [SerializeField]
    private Toggle toogleFixedSquareColor;
    [SerializeField]
    private Toggle toogleCheeringBoy;

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
        toogleFixedSquareColor.isOn = PlayerPrefs.GetInt("FixColorSquare") == 1;
        toogleFixedBarColor.isOn = PlayerPrefs.GetInt("FixBarColor") == 1;
    }
}
