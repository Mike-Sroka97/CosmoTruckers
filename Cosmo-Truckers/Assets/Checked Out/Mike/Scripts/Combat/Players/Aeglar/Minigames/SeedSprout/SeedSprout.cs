using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedSprout : CombatMove
{
    SeedSproutFlower[] flowers;
    int currentFlower = 0;

    private void Start()
    {
        flowers = GetComponentsInChildren<SeedSproutFlower>();
        NextFlower();
    }

    public void NextFlower()
    {
        flowers[currentFlower].TrackTime = true;
        currentFlower++;
    }

    public override void EndMove()
    {

    }
}
