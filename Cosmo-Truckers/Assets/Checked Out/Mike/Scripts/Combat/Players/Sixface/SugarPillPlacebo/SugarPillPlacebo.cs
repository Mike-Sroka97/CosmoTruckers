using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SugarPillPlacebo : MonoBehaviour
{
    [HideInInspector] public Vector3 CurrentCheckPointLocation;
    [HideInInspector] public int Score;

    private void Update()
    {
        if (Score == 3) //max score
        {
            //end minigame early
        }
    }
}
