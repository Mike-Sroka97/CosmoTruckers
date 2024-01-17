using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TralaxyAI : Enemy
{
    [SerializeField] int phaseTwoHealth;
    [SerializeField] int phaseThreeHealth;

    [SerializeField] int tallyYourSinWeight = 1;
    [SerializeField] int absolutionWeight = 3;

    [SerializeField] int maxHealthPerTurn;

    bool phaseTwo = false;
    bool justEnteredPhaseTwo = false;
    bool phaseThree = false;

    public override void StartTurn()
    {
        PhaseCheck();

        //phase one
        if(!phaseTwo && !phaseThree)
        {
            //bubble babies
            if(EnemyManager.Instance.GetAliveEnemySummons().Count >= 2)
            {
                ChosenAttack = attacks[0];
            }
            //astor incubation
            else
            {
                ChosenAttack = attacks[1];
            }
        }

        //phase two
        else if(phaseTwo)
        {
            if(justEnteredPhaseTwo)
            {
                justEnteredPhaseTwo = false;
                ChosenAttack = attacks[2];
            }

            int random = Random.Range(0, tallyYourSinWeight + absolutionWeight);
            random++;

            //tally your sin
            if (random == tallyYourSinWeight)
                ChosenAttack = attacks[2];

            //absolution
            else
                ChosenAttack = attacks[3];
        }

        //phase three
        else
        {
            //golden fury
            //TODO NEEDS WRATH AUG

            //devastation
            //TODO NEEDS WRATH AUG
        }

        base.StartTurn();
    }

    protected override void SpecialTarget(int attackIndex)
    {
        //Bubble Babies
        if (attackIndex == 0)
        {
            CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }
        //Astor Incubation
        else if (attackIndex == 1)
        {
            foreach(PlayerCharacter player in EnemyManager.Instance.GetAlivePlayerCharacters())
            {
                if (player.IsUtility)
                {
                    CombatManager.Instance.CharactersSelected.Add(player);
                    return;
                }
            }

            CombatManager.Instance.AddRandomActivePlayer();
        }
        //Tall Your Sin
        else if (attackIndex == 2)
        {
            CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }
        //Absolution
        else if (attackIndex == 3)
        {
            CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }
        //Golden Fury
        else if (attackIndex == 4)
        {
            CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }
        //Devastation
        else
        {
            CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }
    }

    private void PhaseCheck()
    {
        if(CurrentHealth <= phaseTwoHealth && !phaseThree)
        {
            phaseTwo = true;
            justEnteredPhaseTwo = true;
        }
        else if(CurrentHealth <= phaseThreeHealth && phaseTwo)
        {
            phaseTwo = false;
            phaseThree = true;
        }
    }

    public void IncreaseHealth()
    {
        Health += maxHealthPerTurn;
        phaseTwoHealth += maxHealthPerTurn;
        phaseThreeHealth += maxHealthPerTurn;
    }
}
