using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TralaxyAI : Enemy
{
    [SerializeField] int phaseTwoHealth;
    [SerializeField] int phaseThreeHealth;
    [SerializeField] AugmentStackSO wrath;

    [SerializeField] int tallyYourSinWeight = 1;
    [SerializeField] int absolutionWeight = 3;

    [SerializeField] int maxHealthPerTurn;

    bool phaseTwo = false;
    bool justEnteredPhaseTwo = false;
    bool phaseThree = false;
    AugmentStackSO wrathReference;

    protected override void Start()
    {
        HealthChangeEvent.AddListener(PhaseCheck);
        base.Start();
    }

    protected override int SelectAttack()
    {
        CurrentTargets.Clear();

        PhaseCheck();

        //phase one
        if (!phaseTwo && !phaseThree)
        {
            //bubble babies
            if (EnemyManager.Instance.GetAliveEnemySummons().Count >= 2)
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
        else if (phaseTwo)
        {
            if (justEnteredPhaseTwo)
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
            if (wrathReference == null)
            {
                wrathReference = AddAugmentStackAndReturnReference(wrath);
            }

            //golden fury
            if (wrathReference.CurrentStacks < wrathReference.MaxStacks)
                ChosenAttack = attacks[4];

            //devastation
            else
                ChosenAttack = attacks[5];
        }

        return GetAttackIndex();
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
            //Find Utility or random
            if (CombatManager.Instance.FindUtilityCharacter())
                CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this, CombatManager.Instance.FindUtilityCharacter());
            else
                CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }
        //Tally Your Sin
        else if (attackIndex == 2)
        {
            List<AugmentStackSO> sinfulCharacters = new List<AugmentStackSO>();

            foreach (PlayerCharacter playerCharacter in EnemyManager.Instance.GetAlivePlayerCharacters())
                foreach (AugmentStackSO aug in playerCharacter.GetAUGS)
                    if (aug.AugmentName == "Sin")
                        sinfulCharacters.Add(aug);

            if (sinfulCharacters.Count > 0)
            {
                AugmentStackSO mostSin = sinfulCharacters[0];

                foreach (AugmentStackSO sinMan in sinfulCharacters)
                    if (sinMan.CurrentStacks > mostSin.CurrentStacks)
                        mostSin = sinMan;

                CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this, mostSin.MyCharacter.GetComponent<PlayerCharacter>());
            }
            else
            {
                CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
            }
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
            CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }
        //Devastation
        else
        {
            CombatManager.Instance.AOETargetPlayers(this, ChosenAttack);
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
        AdjustMaxHealth(maxHealthPerTurn);
        phaseTwoHealth += maxHealthPerTurn;
        phaseThreeHealth += maxHealthPerTurn;
    }
}
