using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TralaxyAI : Enemy
{
    [SerializeField] int phaseTwoHealth;
    [SerializeField] int phaseThreeHealth;

    [SerializeField] int tallyYourSinWeight = 1;
    [SerializeField] int absolutionWeight = 3;

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
            if(EnemyManager.Instance.Enemies.Count >= 4)
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
                ChosenAttack = attacks[34];
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
}
