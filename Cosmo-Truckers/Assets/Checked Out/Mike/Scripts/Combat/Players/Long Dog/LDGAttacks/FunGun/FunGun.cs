using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunGun : CombatMove
{
    [SerializeField] Transform layout;

    FGGun[] guns;
    int currentActiveGun;

    private void Start()
    {
        StartMove();
        GenerateLayout();
        guns = FindObjectsOfType<FGGun>();
        currentActiveGun = UnityEngine.Random.Range(0, guns.Length);
        guns[currentActiveGun].TrackingTime = true;
    }

    public void NextGun()
    {
        if(currentActiveGun + 1 < guns.Length)
        {
            currentActiveGun++;
        }
        else
        {
            currentActiveGun = 0;
        }

        guns[currentActiveGun].TrackingTime = true;
    }

    public override void EndMove()
    {
        throw new System.NotImplementedException();
    }
}
