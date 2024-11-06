using System;
using UnityEngine;

[Serializable]
public struct CheerSentence
{
    public string Sentence;
    public Sprite Sprite;
    public float Probability;
    public float ShakeFactor;
}

[CreateAssetMenu(menuName = "New CheerPackage")]
public class CheerPackage : ScriptableObject
{

    [field: SerializeField]
    public string Name { get; private set; }

    [Space]

    [SerializeField]
    private CheerSentence[] LowCheerSentences;
    [SerializeField, Range(0f, 1f)]
    private float lowMidThreshold = 0.3f;
    [SerializeField]
    private CheerSentence[] MidCheerSentences;
    [SerializeField, Range(0f, 1f)]
    private float midHighThreshold = 0.7f;
    [SerializeField]
    private CheerSentence[] HighCheerSentences;

    public CheerSentence GetRandomCheerSentence(float level)
    {
        if (level < lowMidThreshold)
        {
            return LowCheerSentences[UnityEngine.Random.Range(0, LowCheerSentences.Length)];
        }
        else if (level < midHighThreshold)
        {
            return MidCheerSentences[UnityEngine.Random.Range(0, MidCheerSentences.Length)];
        }
        else
        {
            return HighCheerSentences[UnityEngine.Random.Range(0, HighCheerSentences.Length)];
        }
    }
}
