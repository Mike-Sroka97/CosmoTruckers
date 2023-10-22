using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPummel : CombatMove
{
    const int twoScoreValue = 2;
    const int oneScoreValue = 1;
    const int baseNumberOfAttacks = 2;

    private void Start()
    {
        GenerateLayout();
    }

    public override void StartMove()
    {
        FindObjectOfType<PPhittable>().DetermineStartingMovement();
        PPspikeBall[] spikeBalls = FindObjectsOfType<PPspikeBall>();

        foreach (PPspikeBall spikeball in spikeBalls)
            spikeball.DetermineStartingMovement();
    }

    public override void EndMove()
    {
        MoveEnded = true;

        if (CombatManager.Instance != null) //In the combat screen
        {
            foreach (Character character in CombatManager.Instance.GetCharactersSelected)
            {
                int numberOfHits;

                //Calculate Damage 
                if (Score >= twoScoreValue)
                {
                    Score = 2;
                    numberOfHits = twoScoreValue + baseNumberOfAttacks;
                }
                else if (Score >= oneScoreValue)
                {
                    Score = 1;
                    numberOfHits = oneScoreValue + baseNumberOfAttacks;
                }
                else
                {
                    Score = 0;
                    numberOfHits = 2;
                }

                //adjust number of hits and damage so that damage is static no matter the number of hits
                character.GetComponent<Character>().TakeMultiHitDamage(baseDamage, numberOfHits);
            }
        }
    }
}
