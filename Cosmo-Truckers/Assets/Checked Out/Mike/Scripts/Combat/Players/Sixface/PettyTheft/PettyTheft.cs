using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PettyTheft : CombatMove
{
    PTMoney[] moneys;

    private void Start()
    {
        GenerateLayout();
        moneys = FindObjectsOfType<PTMoney>();
    }

    public override void StartMove()
    {
        PettyTheftEnemy[] enemies = FindObjectsOfType<PettyTheftEnemy>();
        foreach (PettyTheftEnemy enemy in enemies)
            enemy.ActivateLights();
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
        base.EndMove();
        SixFaceAttackSO attack = (SixFaceAttackSO)CombatManager.Instance.CurrentAttack;
        FindObjectOfType<SixFaceVessel>().UpdateFace(attack.faceType);
    }
}
