using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PettyTheft : CombatMove
{
    PTMoney[] moneys;

    private void Start()
    {
        StartMove();
        GenerateLayout();
        moneys = FindObjectsOfType<PTMoney>();
    }

    public void ActivateMoney()
    {
        foreach(PTMoney money in moneys)
        {
            money.Activate();
        }
    }

    public override void EndMove()
    {
        throw new System.NotImplementedException();
    }
}
