using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeIron : CombatMove
{
    public override void StartMove()
    {
        //We have to do this to prevent the colliders from not working when the player does not move
        FindObjectOfType<PlayerBody>().transform.position -= new Vector3(-.01f, 0, 0);
        GetComponentInChildren<LargeIronClock>().enabled = true;
    }

    public override void EndMove()
    {
        MoveEnded = true;

        int damage = CalculateScore();

        DealMultiHitDamageOrHealing(CombatManager.Instance.GetCharactersSelected[0], damage, 2);
        CombatManager.Instance.GetCharactersSelected[0].AddDebuffStack(DebuffToAdd, baseAugmentStacks);
    }
}
