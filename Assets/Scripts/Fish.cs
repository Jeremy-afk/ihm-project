using UnityEngine;

public class Fish : MonoBehaviour
{
    //[SerializeField, Tooltip("Max time before the fish can escapes if the player do not (or badly) perform.")]


    private FishNet fishingNet;

    [SerializeField, Tooltip("Base time between direction changes during fleeing mode")] float changeDirectionTime = 2.5f;
    [SerializeField, Tooltip("Amount of variance added to changeDirectionTime")] float variance;
    [SerializeField] private float fleeSpeed;

    private float timeLeft;
    private Vector2 baitPosition;
    private Vector3 direction = Vector3.zero;
    private int[] listOfShame = new int[] { -1, 0, 1};
    private readonly System.Random random = new System.Random();

    private bool isFleeing = false;
    private bool isBaited = false;

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

    // Makes the fish approach the position of the bait
    public void SetBait(FishNet fishingNet, Vector2 baitPosition)
    {
        if (fishingNet != null)
        {
            isBaited = true;
            isFleeing = false;
            this.baitPosition = baitPosition;
            this.fishingNet = fishingNet;
        }
    }

    private void ApproachBait()
    {
        transform.position = Vector3.MoveTowards(transform.position, baitPosition, fleeSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, fishingNet.transform.position) < 0.01f)
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
            timeLeft += changeDirectionTime + Random.Range(- variance, variance);
            direction = new Vector3(listOfShame[random.Next(0, listOfShame.Length)], listOfShame[random.Next(0, listOfShame.Length)], 0);
        }
        transform.Translate(fleeSpeed * Time.deltaTime * direction);
    }

    // The fish bites the bait (notifies the fishing rod)
    private void BiteBait()
    {
        fishingNet.FishBitesBait();
    }
}
