using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject startMenu;
    [SerializeField]
    private string mainGameSceneName;

    [SerializeField] private GameManager gameManager;

    [SerializeField] private GameObject difficultyMenu;
    [SerializeField] private GameObject mainMenu;

    [SerializeField] private TMP_Dropdown dropdown;

    private void Start()
    {
        if (mainGameSceneName == "")
        {
            Debug.LogError("Main Game Scene Name is not set in the MainMenuHandler script.");
        }

        if (dropdown != null)
        {
            dropdown.onValueChanged.AddListener(OnDifficultyChange);
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
        mainMenu.SetActive(false);
        difficultyMenu.SetActive(true);
    }

    public void BackDifficultyButton()
    {
        mainMenu.SetActive(true);
        difficultyMenu.SetActive(false);
    }

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
