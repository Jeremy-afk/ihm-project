using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject startMenu;
    [SerializeField]
    private string mainGameSceneName;

    [SerializeField] private GameManager gameManager;

    [SerializeField] private GameObject difficultyMenu;
    [SerializeField] private GameObject mainMenu;

    private void Start()
    {
        if (mainGameSceneName == "")
        {
            Debug.LogError("Main Game Scene Name is not set in the MainMenuHandler script.");
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

    public void ChangeDifficulty()
    {
        
    }
}
