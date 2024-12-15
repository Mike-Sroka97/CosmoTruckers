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

        base.StartMove();
    }

    public override void EndMove()
    {
        if (FindObjectOfType<InaPractice>())
            return;

        MoveEnded = true;

        if (CombatManager.Instance != null) //In the combat screen
        {
            foreach (Character character in CombatManager.Instance.GetCharactersSelected)
            {
                int numberOfHits = CalculateNumberOfHits();

                //1 being base damage
                float DamageAdj = 1;

                //Damage on players must be divided by 100 to multiply the final
                DamageAdj = CombatManager.Instance.GetCurrentCharacter.Stats.Damage / 100;
                float newDamage = baseDamage * DamageAdj;
                baseDamage = (int)newDamage;
                int totalDamage = baseDamage * numberOfHits + character.FlatDamageAdjustment * numberOfHits;

                character.GetComponent<Character>().TakeMultiHitDamage(totalDamage / numberOfHits, numberOfHits);
                FindObjectOfType<SafeTMana>().SetCurrentAnger(numberOfHits);
            }
        }
    }

    private int CalculateNumberOfHits()
    {
        //Calculate Damage 
        if (Score >= twoScoreValue)
        {
            Score = 2;
            return twoScoreValue + baseNumberOfAttacks;
        }
        else if (Score >= oneScoreValue)
        {
            Score = 1;
            return oneScoreValue + baseNumberOfAttacks;
        }
        else
        {
            Score = 0;
            return baseNumberOfAttacks;
        }
    }

    public override string TrainingDisplayText => $"You scored {Score = (Score > maxScore ? maxScore : Score)}/{maxScore} dealing {(Score * Damage + baseDamage) / CalculateNumberOfHits()} damage {CalculateNumberOfHits()} times.";
}
