using TMPro;
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
    [SerializeField]
    private TextMeshProUGUI difficultyText;

    private Difficulty currentDifficulty;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        currentDifficulty = FetchDifficulty();
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
            Debug.LogWarning("Couldn't load difficulty. Loading the default difficulty.");
            if (!defaultDifficulty)
            {
                Debug.LogWarning("There is no default difficulty, going with the default settings.");
                return;
            }
            difficulty = defaultDifficulty;
        }

        bool contextualHelp = difficulty.displayContextualHelp;

        // Load the fishnet
        fishNet.SetFishNetProperties(difficulty.FishNetProperties, contextualHelp);
        fishPool.SetFishPoolProperties(difficulty.FishPoolProperties, difficulty.FishModifiers);
        fishRoad.SetCastingProperties(difficulty.CastingProperties);

        difficultyText.text = difficulty.Name;
    }
}
