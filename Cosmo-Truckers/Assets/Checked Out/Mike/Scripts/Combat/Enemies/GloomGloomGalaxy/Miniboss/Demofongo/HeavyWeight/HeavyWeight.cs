using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyWeight : CombatMove
{
    public override void StartMove()
    {
        foreach (HeavyWeightSaw saw in GetComponentsInChildren<HeavyWeightSaw>())
            saw.enabled = true;
        foreach (HeavyWeightWeight weight in GetComponentsInChildren<HeavyWeightWeight>())
            weight.enabled = true;

        SetupMultiplayer();
        base.StartMove();
    }

    public override void EndMove()
    {
        MoveEnded = true;

        int ironFurStacks = 0;

        for (int i = 0; i < CombatManager.Instance.GetCharactersSelected.Count - 1; i++) //minus one to ignore demofongo (not a player character)
        {
            int damage = CalculateMultiplayerScore(PlayerScores[CombatManager.Instance.GetCharactersSelected[i].GetComponent<PlayerCharacter>()]);
            DealDamageOrHealing(CombatManager.Instance.GetCharactersSelected[i], damage);

            if (PlayerScores[CombatManager.Instance.GetCharactersSelected[i].GetComponent<PlayerCharacter>()] > 0)
                ironFurStacks++;
        }

        ironFurStacks += CombatManager.Instance.GetCharactersSelected.Count - 5; //in case there is not 4 players added to the minigame

        CombatManager.Instance.GetCharactersSelected[CombatManager.Instance.GetCharactersSelected.Count - 1].AddAugmentStack(DebuffToAdd, ironFurStacks);
    }
}
