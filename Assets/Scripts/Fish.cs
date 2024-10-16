using System;
using System.Collections;
using UnityEngine;

[Serializable]
public struct FishModifiers
{
    public bool useModifiers;

    public float speedMultiplier;
    public float speedChangeMultiplier;
    public float varianceMultiplier;
    public float baitMaxDurationMultiplier;
    [Space, Range(0f, 1f), Tooltip("Forces the fish to have a minimum speed.\n0 means no magnitude forcing.\n1 means full speed each time.")]
    public float minMagnitude;

    public FishModifiers(bool useModifiers = false, float speedMultiplier = 1f, float speedChangeMultiplier = 1f, float varianceMultiplier = 0f, float baitMaxDurationMultiplier = 1f, float minMagnitude = 0f)
    {
        this.useModifiers = useModifiers;
        this.speedMultiplier = speedMultiplier;
        this.speedChangeMultiplier = speedChangeMultiplier;
        this.varianceMultiplier = varianceMultiplier;
        this.baitMaxDurationMultiplier = baitMaxDurationMultiplier;
        this.minMagnitude = minMagnitude;
    }
} 

public class Fish : MonoBehaviour
{
    private FishNet fishingNet;

    [SerializeField, Tooltip("Base time between direction changes during fleeing mode")] float changeDirectionTime = 2.5f;
    [SerializeField, Tooltip("Amount of variance added to changeDirectionTime")] float variance;
    [SerializeField] private float approachSpeed = 0.5f;
    [SerializeField] private float fleeSpeed = 1.5f;
    [SerializeField, Tooltip("How much time can the player take before the fishes finishes the bait and goes away")]
    private float baitMaxDuration = 1.2f;
    [SerializeField] private bool forceMinMagnitude = false;
    [SerializeField] private float minMagnitude = 0f;

    [Header("Animation")]
    [SerializeField] private float animationSpeed;
    [SerializeField] private float animationTime;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private float timeLeft;
    private Vector2 baitPosition;
    private Vector3 direction = Vector3.zero;

    private readonly int[] listOfShame = new int[] { -1, 0, 1 };
    private readonly System.Random random = new();

    private bool isFleeing = false;
    private bool isEatingBait = false;
    private bool isBaited = false;
    private bool isCaptured = false;

    private Casting castingSystem;

    private void Start()
    {
        direction = new Vector3(UnityEngine.Random.Range(-1f, 2f), UnityEngine.Random.Range(-1f, 1f), 0);
        timeLeft = changeDirectionTime;
    }

    private void Update()
    {
        if (isBaited)
        {
            ApproachBait();
        }

        if (isFleeing)
        {
            Fleeing();
        }
    }

    public void ReleaseDirectly(Vector2 positionToFleeFrom)
    {
        StartCoroutine(FleeAnimation(positionToFleeFrom));
    }

    // Makes the fish approach the position of the bait
    public void SetBait(Casting castingSystem, FishNet fishingNet, Vector2 baitPosition, FishModifiers modifiers)
    {
        if (fishingNet != null && castingSystem != null)
        {
            isBaited = true;
            isFleeing = false;
            this.baitPosition = baitPosition;
            this.fishingNet = fishingNet;
            this.castingSystem = castingSystem;
        }

        if (modifiers.useModifiers)
        {
            fleeSpeed *= modifiers.speedMultiplier;
            changeDirectionTime /= modifiers.speedChangeMultiplier;
            baitMaxDuration *= modifiers.baitMaxDurationMultiplier;
            variance *= modifiers.varianceMultiplier;
            minMagnitude = modifiers.minMagnitude;
        }
    }

    // The player managed to respond in time and the fish is now hooked
    public void Hook()
    {
        isEatingBait = false;
        isFleeing = true;
    }

    // The player failed to keep the net in the right position and the fish is escaping
    public void Release()
    {
        isFleeing = false;
        isEatingBait = false;
        isBaited = false;
        StartCoroutine(FleeAnimation(castingSystem.GetNetPosition()));
    }

    // The player succeeded in capturing the fish
    public void Capture()
    {
        isFleeing = false;
        isCaptured = true;
        StartCoroutine(CaptureAnimation());
    }

    private void ApproachBait()
    {
        MoveFish(approachSpeed * Time.deltaTime * (baitPosition - (Vector2)transform.position).normalized);

        if (Vector3.Distance(transform.position, baitPosition) < 0.01f)
        {
            BiteBait();
        }
    }

    // The fish has been correctly hooked and is trying to escape
    private void Fleeing()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            // Choose a new direction

            timeLeft += changeDirectionTime + UnityEngine.Random.Range(-variance, variance);

            // Original random system (allows 9 fixed directions)
            //direction = new Vector3(listOfShame[random.Next(0, listOfShame.Length)], listOfShame[random.Next(0, listOfShame.Length)], 0);

            // Alternative random system (allows for all real directions)
            float randomAngle = UnityEngine.Random.Range(-Mathf.PI, Mathf.PI);
            direction = new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle), 0) * UnityEngine.Random.Range(minMagnitude, 1f);

            // Rest of this code is only useful if the original random system is used

            //float magnitude = direction.magnitude;

            //print("before: " + magnitude);

            //if (magnitude < minMagnitude)
            //{
            //    direction *= minMagnitude/magnitude;
            //}

            //print("after correction: " + magnitude);
        }
        MoveFish(fleeSpeed * Time.deltaTime * direction);
    }

    // The fish bites the bait (notifies the fishing rod)
    private void BiteBait()
    {
        isBaited = false;
        isEatingBait = true;
        castingSystem.FishBitesBait();
        StartCoroutine(EatBait());
    }

    // Moves the fish and turns the sprite accordingly
    private void MoveFish(Vector2 movement)
    {
        transform.Translate(movement);
        spriteRenderer.flipX = movement.x > 0;
    }

    private IEnumerator EatBait()
    {
        yield return new WaitForSeconds(baitMaxDuration);

        if (isEatingBait)
            castingSystem.FishFinishedTheBait();
    }

    private IEnumerator FleeAnimation(Vector2 fleeFrom)
    {
        float timer = animationTime;
        Vector2 animDirection = ((Vector2)transform.position - fleeFrom).normalized * animationSpeed;

        while (timer > 0)
        {
            MoveFish(animDirection * Time.deltaTime);
            timer -= Time.deltaTime;
            yield return null;
        }

        gameObject.SetActive(false);
    }

    private IEnumerator CaptureAnimation()
    {
        // Do some animation for the capture
        // Shake the fish a bit around its base position
        float timer = animationTime;
        Vector3 basePosition = transform.position;
        Vector3 shakeDirection = new Vector3(0.02f, 0.02f, 0);

        while (timer > 0) {
            Vector3 shake = new Vector3(
                UnityEngine.Random.Range(-1f, 1f) * shakeDirection.x,
                UnityEngine.Random.Range(-1f, 1f) * shakeDirection.y,
                0
            );
            transform.position = basePosition + shake;
            timer -= Time.deltaTime;
            yield return null;
        }

        yield return null;
    }
}
