using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ScoreGestionner.Instance.Score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
