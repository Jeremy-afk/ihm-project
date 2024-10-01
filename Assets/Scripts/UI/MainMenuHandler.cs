using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject startMenu;

    public void NewGame()
    {
        print("Loading game!");
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        print("quit game!");
        Application.Quit();
    }
}
