using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeggieVengeance : CombatMove
{

    public override void StartMove()
    {
        GenerateLayout();
        FindObjectOfType<VeggieVengeanceCannon>().StartMove();
    }

    public override void EndMove()
    {
        FindObjectOfType<VeggieVengeanceCannon>().CalculateMove = false;
        MoveEnded = true;

        foreach (Character character in CombatManager.Instance.GetCharactersSelected)
        {
            //Calculate Healing
            if (Score < 0)
                Score = 0;
            if (Score >= maxScore)
                Score = maxScore;

            int currentHealing = 0;
            //defending/attacking
            if (!defending)
                currentHealing = Score * Damage;
            else
                currentHealing = maxScore * Damage - Score * Damage;

            currentHealing += baseDamage;

            //Calculate Augment Stacks
            int augmentStacks = 1; //always applies overfed to enemies

            //okay here it comes... Im boutta math
            if(character.CurrentHealth + character.AdjustAttackHealing(currentHealing) > character.Health)
            {                
                int currentDamage = character.CurrentHealth + character.AdjustAttackHealing(currentHealing) - character.Health;
                currentHealing = character.Health - character.CurrentHealth;
                character.GetComponent<Character>().TakeHealing(currentHealing, true); //heal to full first
                character.TakeDamage(currentDamage, true); //pierce defense cause technically healing
            }
            else
            {
                character.GetComponent<Character>().TakeHealing(currentHealing);
            }

            ////Apply augment
            //if (playerEnemyTargetDifference && character.GetComponent<Enemy>())
            //    character.GetComponent<Character>().AddDebuffStack(DebuffToAdd, augmentStacks);
        }
    }
}
