using UnityEngine;

public class GameLoader : MonoBehaviour
{
    [SerializeField]
    private Difficulty defaultDifficulty;

    private Difficulty currentDifficulty;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        currentDifficulty = FetchDifficulty() ?? defaultDifficulty;
    }

    private Difficulty FetchDifficulty()
    {
        // Fetches the difficulty from the game manager

        if (gameManager)
        {
            return gameManager.GetDifficulty();
        }

        Debug.LogWarning("Game Manager not detected. Did the singleton start correctly ?");

        return null;
    }

    private void LoadDifficulty(Difficulty difficulty)
    {
        // Load the difficulty to the various items of the game.
    }
}
