using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraggyCoating : CombatMove
{
    [SerializeField] float maxTimeSuccess;

    private void Start()
    {
        StartMove();
        GenerateLayout();
    }

    public override void EndMove()
    {
        AugmentScore = CalculateAugmentScore();

        foreach(Character character in CombatManager.Instance.GetCharactersSelected)
            if (character.GetComponent<Enemy>())
                character.AddDebuffStack(DebuffToAdd, AugmentScore);
    }
}
