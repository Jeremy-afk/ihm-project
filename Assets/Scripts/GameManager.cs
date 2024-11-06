using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Difficulty[] difficulties;

    public GameDifficulty selectedDifficulty = GameDifficulty.Medium;

    public enum GameDifficulty
    {
        Easy,
        Medium,
        Hard
    }

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance)
        {
            Debug.LogWarning("An instance of game manager already exists ! Overwriting...");
            Destroy(Instance.gameObject);
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public Difficulty GetDifficulty()
    {
        int index = (int)selectedDifficulty;

        if (index >= difficulties.Length)
        {
            Debug.LogWarning("Too few difficulties object were given to the game manager !");

            return null;
        }

        return difficulties[index];
    }

    // Called automatically by the dropdown on value changed.
    public void SetDifficulty(Int32 difficultyIndex)
    {
        selectedDifficulty = (GameDifficulty)difficultyIndex;
    }

    private void LoadVideoPlayerPrefs()
    {
        Screen.fullScreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
    }
}
