using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TralaxyAI : Enemy
{
    [SerializeField] int phaseTwoHealth;
    [SerializeField] int phaseThreeHealth;
    [SerializeField] DebuffStackSO wrath;

    [SerializeField] int tallyYourSinWeight = 1;
    [SerializeField] int absolutionWeight = 3;

    [SerializeField] int maxHealthPerTurn;

    bool phaseTwo = false;
    bool justEnteredPhaseTwo = false;
    bool phaseThree = false;
    DebuffStackSO wrathReference;

    public override void StartTurn()
    {
        PhaseCheck();

        //phase one
        if (!phaseTwo && !phaseThree)
        {
            //bubble babies
            if(EnemyManager.Instance.GetAliveEnemySummons().Count >= 2)
            {
                ChosenAttack = attacks[0];
            }
            //astor incubation
            else
            {
                ChosenAttack = attacks[4]; //1
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
            if (wrathReference == null)
            {
               wrathReference = AddDebuffStackAndReturnReference(wrath);
            }

            //golden fury
            if (wrathReference.CurrentStacks < wrathReference.MaxStacks)
                ChosenAttack = attacks[4];

            //devastation
            else
                ChosenAttack = attacks[5];
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
            List<DebuffStackSO> sinfulCharacters = new List<DebuffStackSO>();

            foreach (PlayerCharacter playerCharacter in EnemyManager.Instance.GetAlivePlayerCharacters())
                foreach (DebuffStackSO aug in playerCharacter.GetAUGS)
                    if (aug.DebuffName == "Sin")
                        sinfulCharacters.Add(aug);

            if (sinfulCharacters.Count > 0)
            {
                DebuffStackSO mostSin = sinfulCharacters[0];

                foreach (DebuffStackSO sinMan in sinfulCharacters)
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
            CombatManager.Instance.AOETargetPlayers(ChosenAttack);
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
