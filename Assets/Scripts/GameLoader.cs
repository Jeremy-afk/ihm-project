using UnityEngine;

public class GameLoader : MonoBehaviour
{
    [SerializeField]
    private Difficulty defaultDifficulty;

    [Header("References")]
    [SerializeField]
    private FishNet fishNet;
    [SerializeField]
    private FishPool fishPool;
    [SerializeField]
    private Casting fishRoad;

    private Difficulty currentDifficulty;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        currentDifficulty = FetchDifficulty() ?? defaultDifficulty;

        LoadDifficulty(currentDifficulty);
    }

    private Difficulty FetchDifficulty()
    {
        // Fetches the difficulty from the game manager

        if (gameManager)
        {
            return gameManager.GetDifficulty();
        }

        Debug.LogWarning("Game Manager not detected. Did the singleton start correctly ? (loading default difficulty)");

        return null;
    }

    private void LoadDifficulty(Difficulty difficulty)
    {
        if (!difficulty)
        {
            Debug.LogWarning($"Couldn't load difficulty. Going with the default settings.");
            return;
        }

        Debug.Log("Loading difficulty " + difficulty.Name);

        // Load the fishnet
        fishNet.SetFishNetProperties(difficulty.FishNetProperties);
        fishPool.SetFishPoolProperties(difficulty.FishPoolProperties, difficulty.FishModifiers);
        fishRoad.SetCastingProperties(difficulty.CastingProperties);
    }
}
