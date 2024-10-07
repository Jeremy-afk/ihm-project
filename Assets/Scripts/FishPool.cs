using UnityEngine;

public class FishPool : MonoBehaviour
{
    [SerializeField]
    private Fish fishPrefab;
    [SerializeField, Tooltip("Shoud the fishes all be spawned as active-gameObjects ? Otherwise, the fishes will remain not active.")]
    private bool spawnActiveFishes;

    [Space]

    [SerializeField]
    private int maxFishes = 10;
    [SerializeField, Tooltip("Guarenteed distance between all the fishes in the pool upon spawning")]
    private float minimumSpawnDistance = 1.0f;
    [SerializeField]
    private Collider2D spawnArea;

    [Space]

    [SerializeField]
    private int maxSpawnAttempts = 10;
    [SerializeField, Tooltip("Will the pool try to spawn a fish for the whole capacity (False) ?  Or will it stops at the first fish that fails to spawn (True) ?")]
    private bool stopOnFirstFail = false;

    [Space]

    [SerializeField]
    private FishNet fishNet;
    [SerializeField]
    private Casting castingSystem;

    private float m_minimumSpawnDistanceSqr;
    private Fish[] m_fishes;


    // Returns the closest fish to the given position within the given distance (otherwise returns null)
    public Fish PutBaitInPosition(Vector2 position, float maxDistance)
    {
        Fish selectedFish = null;

        float minDistanceSqr = maxDistance * maxDistance;
        foreach (var fish in m_fishes)
        {
            if (fish)
            {
                float distanceSqr = (fish.transform.position - (Vector3)position).sqrMagnitude;
                if (distanceSqr < minDistanceSqr)
                {
                    minDistanceSqr = distanceSqr;
                    selectedFish = fish;
                }
            }
        }

        if (selectedFish != null)
        {
            selectedFish.SetBait(castingSystem, fishNet, position);
        }

        return selectedFish;
    }

    private void Awake()
    {
        if (fishPrefab == null)
        {
            Debug.LogError("Fish prefab is not set in FishPool component.");
            enabled = false;
            return;
        }
        if (spawnArea == null)
        {
            Debug.LogError("Spawn area is not set in FishPool component.");
            enabled = false;
            return;
        }

        m_minimumSpawnDistanceSqr = minimumSpawnDistance * minimumSpawnDistance;

        SpawnFishes();
    }

    // Spawns all the fishes in the pool (using the fish prefab and serialized parameters)
    private void SpawnFishes()
    {
        m_fishes = new Fish[maxFishes];

        for (int i = 0; i < maxFishes; i++)
        {
            m_fishes[i] = SpawnOneFish();
            if (m_fishes[i] == null && stopOnFirstFail)
            {
                Debug.LogWarning("Stopped spawning fishes (stopOnFirstFail = true)");
                break;
            }
            else if (m_fishes[i])
            {
                m_fishes[i].gameObject.SetActive(spawnActiveFishes);
            }
        }
    }

    // Spawns a fish in the Returns the spawned fish if successful, otherwise null
    private Fish SpawnOneFish()
    {
        // Spawn fish at random position within the spawn area
        // Make also sure that the fish is not too close to any other fish
        Fish spawnedFish = null;
        Vector3 spawnPosition;
        bool validPosition;
        int spawnAttemptsRemaining = maxSpawnAttempts;

        // Try to find a valid position to spawn a fish
        do
        {
            spawnPosition = new Vector3(
                Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x),
                Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y),
                0
            );

            validPosition = true;
            foreach (var fish in m_fishes)
            {
                if (fish == null) continue;

                if ((fish.transform.position - spawnPosition).sqrMagnitude < m_minimumSpawnDistanceSqr)
                {
                    validPosition = false;
                    break;
                }
            }

            spawnAttemptsRemaining--;

        } while (!validPosition && spawnAttemptsRemaining > 0);

        // If we found a valid position, spawn the fish
        if (validPosition)
        {
            spawnedFish = Instantiate(fishPrefab, spawnPosition, Quaternion.identity);
        }
        else
            Debug.LogWarning("Failed to find a valid position to spawn a fish.");

        return spawnedFish;
    }
}
