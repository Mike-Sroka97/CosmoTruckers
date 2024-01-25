using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PainfulPresents : CombatMove
{
    public override void StartMove()
    {
        SetupMultiplayer();
        base.StartMove();
    }

    public override void EndMove()
    {
        MoveEnded = true;

        for (int i = 0; i < CombatManager.Instance.GetCharactersSelected.Count; i++) //minus one to ignore demofongo (not a player character)
        {
            int damage = CalculateMultiplayerScore(PlayerScores[CombatManager.Instance.GetCharactersSelected[i].GetComponent<PlayerCharacter>()]);
            DealDamageOrHealing(CombatManager.Instance.GetCharactersSelected[i], damage, 1); //1 == set damaging
            
            int healing = CalculateMultiplayerAugmentScore(PlayerAugmentScores[CombatManager.Instance.GetCharactersSelected[i].GetComponent<PlayerCharacter>()]);
            DealDamageOrHealing(CombatManager.Instance.GetCharactersSelected[i], healing, 2); //2 == set healing
        }
    }
}
