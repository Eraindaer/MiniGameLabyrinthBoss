using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreGestionner : MonoBehaviour
{
    public static ScoreGestionner Instance { get; private set; }

    public int Score;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    
}

