using UnityEngine;

[CreateAssetMenu(menuName = "NewDifficulty")]
public class Difficulty : ScriptableObject
{
    [field: SerializeField]
    public string Name { get; private set; }

    [field: Space, SerializeField]
    public FishPoolProperties FishPoolProperties { get; private set; }
    [field: Space, SerializeField]
    public FishNetProperties FishNetProperties { get; private set; }
    [field: Space, SerializeField]
    public CastingProperties CastingProperties { get; private set; }

    [field: Header("Global fishes properties")]
    [field: SerializeField]
    public FishModifiers FishModifiers { get; private set; }

    [field: Header("Context Help")]
    [field: SerializeField]
    public bool displayContextualHelp { get; private set; }
}
