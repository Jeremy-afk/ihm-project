using UnityEngine;


public class GameManager : MonoBehaviour
{


    public GameDifficulty difficulty = GameDifficulty.Medium;

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
            Debug.LogWarning("An instance of game manager already exists ! Disabling...");
            gameObject.SetActive(false);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Load the difficulty IF the game started (we're not in the MainMenu scene, but MainGame)

    }

    public Difficulty GetDifficulty()
    {
        return null;
    }

    public void SetEasy()
    {
        difficulty = GameDifficulty.Easy;
    }

    public void SetMedium()
    {
        difficulty = GameDifficulty.Medium;
    }

    public void SetHard() 
    {
        difficulty = GameDifficulty.Hard;
    }
}
