using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject startMenu;
    [SerializeField]
    private string mainGameSceneName;

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
}
