using UnityEngine;

[CreateAssetMenu(menuName = "NewDifficulty")]
public class Difficulty : ScriptableObject
{
    [field: SerializeField]
    public string Name { get; private set; }

    [field: Header("Fish net properties")]
    [field: SerializeField]
    public float GainRate { get; private set; }
    [field: SerializeField]
    public float GainRateClamping { get; private set; }
    [field: SerializeField]
    public float LossRate { get; private set; }
    [field: SerializeField]
    public float LossRateClamping { get; private set; }

    [field: Header("Global fishes properties")]
    [field: SerializeField]
    public float SpeedMultiplier { get; private set; }
    [field: SerializeField]
    public float ChangingSpeedMultiplier { get; private set; }
}
