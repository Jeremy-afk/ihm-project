using System;
using UnityEngine;

public class Fish : MonoBehaviour
{
    //[SerializeField, Tooltip("Max time before the fish can escapes if the player do not (or badly) perform.")]


    private GameObject fishingRod;

    [SerializeField, Tooltip("Base time between direction changes during fleeing mode")] float changeDirectionTime = 2.5f;
    [SerializeField, Tooltip("Amount of variance added to changeDirectionTime")] float variance;
    [SerializeField] private float fleeSpeed;

    private float timeLeft;
    private Vector3 direction = Vector3.zero;
    private int[] listOfShame = new int[] { -1,0,1};
    private readonly System.Random random = new System.Random();

    [HideInInspector] public bool isFleeing;

    private void Start()
    {
        direction = new Vector3(UnityEngine.Random.Range(-1f, 2f), UnityEngine.Random.Range(-1f, 1f), 0);
        timeLeft = changeDirectionTime;
    }
    // Makes the fish approach the position of the bait
    public void SetBait(GameObject fishingRod, Vector2 baitPosition)
    {
        this.fishingRod = fishingRod;
    }

    // The fish has been correctly hooked and is trying to escape
    private void Fleeing()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            timeLeft += changeDirectionTime + UnityEngine.Random.Range(-variance, variance);
            direction = new Vector3(listOfShame[random.Next(0, listOfShame.Length)], listOfShame[random.Next(0, listOfShame.Length)], 0);
        }
        transform.Translate(direction * Time.deltaTime * fleeSpeed);
    }

    private void Update()
    {
        if (isFleeing) 
        { 
            Fleeing();
        }

    }

    // The fish bites the bait (notifies the fishing rod)
    private void BiteBait()
    {

    }
}
