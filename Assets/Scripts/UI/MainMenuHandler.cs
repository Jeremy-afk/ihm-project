using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] private GameObject mainButtons;
    [SerializeField] private GameObject difficultyButtons;
    [SerializeField] private GameObject settingsButtons;

    [Header("Game Launch")]
    [SerializeField] private string mainGameSceneName;

    [Space]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TMP_Dropdown dropdown;

    private void Start()
    {
        if (mainGameSceneName == "")
        {
            Debug.LogError("Main Game Scene Name is not set in the MainMenuHandler script.");
        }

        if (dropdown != null)
        {
            dropdown.onValueChanged.AddListener(OnDifficultyChange); // Not useful for the moment
        }
    }

    public void NewGame()
    {
        SceneManager.LoadScene(mainGameSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void DifficultyButton()
    {
        mainButtons.SetActive(false);
        difficultyButtons.SetActive(true);
    }

    public void SettingsButton()
    {
        mainButtons.SetActive(false);
        settingsButtons.SetActive(true);
    }

    public void BackMainButtons()
    {
        mainButtons.SetActive(true);
        difficultyButtons.SetActive(false);
        settingsButtons.SetActive(false);
    }

    // Unused for now -- see GameManager.SetDifficulty(Int32)
    private void OnDifficultyChange(int index)
    {
        if (gameManager != null)
        {
            var field = gameManager.GetType().GetField("difficulty");
            if (field != null)
            {
                switch (index)
                {
                    case 0:
                        field.SetValue(gameManager, GameManager.GameDifficulty.Easy);
                        Debug.Log("Easy");
                        break;
                    case 1:
                        field.SetValue(gameManager, GameManager.GameDifficulty.Medium);
                        Debug.Log("Medium");
                        break;
                    case 2:
                        field.SetValue(gameManager, GameManager.GameDifficulty.Hard);
                        Debug.Log("Hard");
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
