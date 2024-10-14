using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public enum GameDifficulty
    {
        Easy,
        Medium,
        Hard
    }
    public GameDifficulty difficulty = GameDifficulty.Medium;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SetEasy()
    {
        difficulty = GameDifficulty.Easy;
    }

    public void SetMedium()
    {
        difficulty = GameDifficulty.Medium;
    }

    public void SetHard() 
    {
        difficulty = GameDifficulty.Hard;
    }
}
