using System.Collections;
using System.Threading;
using UnityEngine;

public class Fish : MonoBehaviour
{
    //[SerializeField, Tooltip("Max time before the fish can escapes if the player do not (or badly) perform.")]


    private FishNet fishingNet;

    [SerializeField, Tooltip("Base time between direction changes during fleeing mode")] float changeDirectionTime = 2.5f;
    [SerializeField, Tooltip("Amount of variance added to changeDirectionTime")] float variance;
    [SerializeField] private float approachSpeed = 0.5f;
    [SerializeField] private float fleeSpeed = 1.5f;
    [SerializeField, Tooltip("How much time can the player take before the fishes finishes the bait and goes away")]
    private float baitMaxDuration = 1.2f;

    [Header("Animation")]
    [SerializeField] private float animationSpeed;
    [SerializeField] private float animationTime;

    private float timeLeft;
    private Vector2 baitPosition;
    private Vector3 direction = Vector3.zero;
    private int[] listOfShame = new int[] { -1, 0, 1 };
    private readonly System.Random random = new System.Random();

    private bool isFleeing = false;
    private bool isEatingBait = false;
    private bool isBaited = false;

    private Casting castingSystem;

    private void Start()
    {
        direction = new Vector3(Random.Range(-1f, 2f), Random.Range(-1f, 1f), 0);
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
    public void SetBait(Casting castingSystem, FishNet fishingNet, Vector2 baitPosition)
    {
        if (fishingNet != null && castingSystem != null)
        {
            isBaited = true;
            isFleeing = false;
            this.baitPosition = baitPosition;
            this.fishingNet = fishingNet;
            this.castingSystem = castingSystem;
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

    private void ApproachBait()
    {
        transform.position = Vector3.MoveTowards(transform.position, baitPosition, approachSpeed * Time.deltaTime);

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
            timeLeft += changeDirectionTime + Random.Range(-variance, variance);
            direction = new Vector3(listOfShame[random.Next(0, listOfShame.Length)], listOfShame[random.Next(0, listOfShame.Length)], 0);
        }
        transform.Translate(fleeSpeed * Time.deltaTime * direction);
    }

    // The fish bites the bait (notifies the fishing rod)
    private void BiteBait()
    {
        isBaited = false;
        isEatingBait = true;
        castingSystem.FishBitesBait();
        StartCoroutine(EatBait());
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
            transform.Translate(animDirection * Time.deltaTime);
            timer -= Time.deltaTime;
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
