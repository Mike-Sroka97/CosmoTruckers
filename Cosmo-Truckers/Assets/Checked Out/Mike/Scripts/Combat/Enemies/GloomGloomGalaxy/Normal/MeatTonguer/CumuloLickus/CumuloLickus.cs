using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CumuloLickus : CombatMove
{
    [SerializeField] AugmentStackSO moistAug;
    [SerializeField] AugmentStackSO shloppedAug;

    private void Start()
    {
        GenerateLayout();
    }

    public override void StartMove()
    {
        base.StartMove();
        GetComponentInChildren<CumuloLickusShlop>().enabled = true;
        SetupMultiplayer();
    }
    public override void EndMove()
    {
        MoveEnded = true;

        for (int i = 0; i < CombatManager.Instance.GetCharactersSelected.Count; i++)
        {
            if (CombatManager.Instance.GetCharactersSelected[i].GetComponent<Enemy>())
            {
                CombatManager.Instance.GetCharactersSelected[i].AddAugmentStack(moistAug);
                DealDamageOrHealing(CombatManager.Instance.GetCharactersSelected[i], baseDamage);
            }
            else if (!CombatManager.Instance.GetCharactersSelected[i].GetComponent<PlayerCharacterSummon>())
            {
                int tempScore = PlayerAugmentScores[CombatManager.Instance.GetCharactersSelected[i].GetComponent<PlayerCharacter>()];
                tempScore = CalculateMultiplayerAugmentScore(tempScore);
                ApplyAugment(CombatManager.Instance.GetCharactersSelected[i], tempScore);
                CombatManager.Instance.GetCharactersSelected[i].AddAugmentStack(shloppedAug, tempScore);
            }
        }
    }
}
