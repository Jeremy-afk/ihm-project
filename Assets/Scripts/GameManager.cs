using UnityEngine;


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
            Debug.LogWarning("An instance of game manager already exists ! Disabling...");
            gameObject.SetActive(false);
            return;
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

    public void SetDifficulty(int difficultyIndex)
    {
        selectedDifficulty = (GameDifficulty)difficultyIndex;
    }
}
