using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraggyCoating : CombatMove
{
    [SerializeField] float maxTimeSuccess;

    private void Start()
    {
        GenerateLayout();
    }

    public override void EndMove()
    {
        if (FindObjectOfType<InaPractice>())
            return;

        AugmentScore = CalculateAugmentScore();

        foreach(Character character in CombatManager.Instance.GetCharactersSelected)
            if (character.GetComponent<Enemy>())
                character.AddAugmentStack(DebuffToAdd, AugmentScore);
    }
}
