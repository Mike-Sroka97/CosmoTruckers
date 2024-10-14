using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathKill : CombatMove
{
    private void Start()
    {
        GenerateLayout();
    }

    public override void EndMove()
    {
        MoveEnded = true;

        //player go die
        int augStacks = CalculateAugmentScore();

        CombatManager.Instance.CharactersSelected[0].TakeDamage(999, true);
        CombatManager.Instance.CharactersSelected[0].AddAugmentStack(DebuffToAdd, augStacks);

        //Heal Wying Dreem
        int healing = CalculateScore();
        CombatManager.Instance.CharactersSelected[1].TakeHealing(healing);
    }
}
