using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbnusAI : Enemy
{
    [SerializeField] int phaseTwoHealth;
    [SerializeField] int phaseThreeHealth;
    [SerializeField] int baseDefense = 25;

    [HideInInspector] public bool EyeOneKilled = false;
    [HideInInspector] public bool EyeTwoKilled = false;
    [HideInInspector] public bool EyeThreeKilled = false;

    int phase = 1;
    public override void StartTurn()
    {
        PhaseCheck();
        ChosenAttack = attacks[phase - 1];
        base.StartTurn();
    }

    protected override void SpecialTarget(int attackIndex)
    {
        //Behold, Death!
        if (attackIndex == 0)
        {
            CombatManager.Instance.AOETargetPlayers(ChosenAttack);
        }
        //Titan's Terror
        else if (attackIndex == 1)
        {
            CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }
        //Last Resort
        else
        {
            CombatManager.Instance.AOETargetPlayers(ChosenAttack);
        }
    }

    private void PhaseCheck()
    {
        {
            if (Health <= phaseThreeHealth && phase < 3)
                phase = 3;
            else if (Health <= phaseTwoHealth && phase < 2)
                phase = 2;
            else if (phase < 1)
                phase = 1;
        }
    }

    public void ShredArmor()
    {
        Stats.Defense = baseDefense;
    }
}
