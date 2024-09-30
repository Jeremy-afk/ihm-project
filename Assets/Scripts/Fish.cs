using UnityEngine;

public class Fish : MonoBehaviour
{
    [SerializeField, Tooltip("Max time before the fish can escapes if the player do not (or badly) perform.")]
    private float fleeSpeed = 3.0f;

    private GameObject fishingRod;


    // Makes the fish approach the position of the bait
    public void SetBait(GameObject fishingRod, Vector2 baitPosition)
    {
        this.fishingRod = fishingRod;
    }

    // The fish has been correctly hooked and is trying to escape
    public void Hook()
    {
        // TODO: Either implement the minigame logic here (with update function), or have the fishing rod handle it ?
    }

    // The fish bites the bait (notifies the fishing rod)
    private void BiteBait()
    {

    }
}
