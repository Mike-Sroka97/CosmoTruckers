using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SixFPArrow : MonoBehaviour
{
    public void ArrowIncreaseScore(FunkyPersuasion miniGame, int score)
    {
        miniGame.Score += score;
        Destroy(gameObject);
    }

    public void ArrowDecreaseScore(FunkyPersuasion miniGame)
    {
        miniGame.Score--;
        Destroy(gameObject); 
    }
}
