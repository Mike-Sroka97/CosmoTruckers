using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperSlam : CombatMove
{
    private void Start()
    {
        GenerateLayout();
        StartMoveTest(); 
    }

    public override void StartMove()
    {
        FindObjectOfType<SSGozorMovement>().trackTime = true;

        SSGun[] ssGuns = FindObjectsOfType<SSGun>();
        foreach (SSGun ssGun in ssGuns)
            ssGun.trackTime = true;
    }
}
