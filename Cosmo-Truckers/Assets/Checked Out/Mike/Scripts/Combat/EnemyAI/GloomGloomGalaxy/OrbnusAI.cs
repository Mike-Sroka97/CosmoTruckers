using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbnusAI : Enemy
{
    [SerializeField] int baseDefense = 25;

    [HideInInspector] public bool EyeOneKilled = false;
    [HideInInspector] public bool EyeTwoKilled = false;
    [HideInInspector] public bool EyeThreeKilled = false;

    bool turnOne = false;

    public int Phase = 1;
    public override void StartTurn()
    {
        //TODO funny turn one haha

        ChosenAttack = attacks[Phase - 1];
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

    public void ShredArmor()
    {
        TakeDamage(1000, true);
        Phase = 2;
        Stats.Defense = baseDefense;
    }

    public override void Die()
    {
        Phase = 3;
    }
}
