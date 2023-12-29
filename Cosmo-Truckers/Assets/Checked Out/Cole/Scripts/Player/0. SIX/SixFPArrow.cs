using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SixFPArrow : MonoBehaviour
{
    public void ArrowIncreaseScore(FunkyPersuasion miniGame, int score)
    {
        miniGame.Score += score;
        Debug.Log(miniGame.Score);
        Destroy(gameObject);
    }

    public void ArrowDecreaseScore(FunkyPersuasion miniGame)
    {
        miniGame.Score--;
        Debug.Log(miniGame.Score);
        Destroy(gameObject); 
    }
}
