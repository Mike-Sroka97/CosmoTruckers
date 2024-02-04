using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QmuavAI : Enemy
{
    int lastAttack = -1;
    int random = 0;
    public int NumberOfTimesHitLastRound = 0;

    public override void StartTurn()
    {
        //TODO add boss move check
        //Prevents the same spell from being cast twice in a row
        //if(lastAttack != -1)
        //{
        //    while (lastAttack == random)
        //        random = Random.Range(0, attacks.Length - 1); //minus 1 to avoid casting boss move prematurely 
        //}

        ChosenAttack = attacks[4];
        lastAttack = random;
        base.StartTurn();
    }

    protected override void SpecialTarget(int attackIndex)
    {
        //Harm and Repulse
        if (attackIndex == 0)
        {
            //Random single target
            CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }
        //Hive Healing
        else if (attackIndex == 1)
        {
            //Add Qmuav
            CombatManager.Instance.GetCharactersSelected.Add(this);

            //Add Support or Random
            if(CombatManager.Instance.FindSupportCharacter())
                CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this, CombatManager.Instance.FindSupportCharacter());
            else
                CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }
        //Twisted Targeting
        else if (attackIndex == 2)
        {
            //AOE targeting
            CombatManager.Instance.AOETargetPlayers(ChosenAttack);
        }
        //Atomic Impact
        else if (attackIndex == 3)
        {
            //Find DPS or random
            if (CombatManager.Instance.FindDPSCharacter())
                CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this, CombatManager.Instance.FindDPSCharacter());
            else
                CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }
        //Gravitic Siphon
        else if (attackIndex == 4)
        {
            //Random single target
            CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);

            //Add Qmuav
            CombatManager.Instance.GetCharactersSelected.Add(this);
        }
        //Galaxy Burst
        else if (attackIndex == 5)
        {
            //Find Utility or random
            if (CombatManager.Instance.FindUtilityCharacter())
                CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this, CombatManager.Instance.FindUtilityCharacter());
            else
                CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }
        //Task Master
        else if (attackIndex == 6)
        {
            CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }
        //Gloom Guard
        else if (attackIndex == 7)
        {
            CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }
        //Supermassive Amplifier
        else if (attackIndex == 8)
        {
            CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }
        //Titan's Terror
        else if (attackIndex == 9)
        {
            CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }
        //Titan's Terror
        else if (attackIndex == 10)
        {
            CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }
        //Cum of Frustration (BOSS MOVE)
        else if (attackIndex == 11)
        {
            CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }
    }

    public override void EndTurn()
    {
        //Resets hit counter (used for gravitic siphon)
        NumberOfTimesHitLastRound = 0;
        base.EndTurn();
    }

    public override void TakeDamage(int damage, bool defensePiercing = false)
    {
        //Adds single hit to hit counter (used for gravitic siphon)
        NumberOfTimesHitLastRound++;
        base.TakeDamage(damage, defensePiercing);
    }

    public override void TakeMultiHitDamage(int damage, int numberOfHits, bool defensePiercing = false)
    {
        //Adds each hit to hit counter (used for gravitic siphon)
        NumberOfTimesHitLastRound += numberOfHits;
        base.TakeMultiHitDamage(damage, numberOfHits, defensePiercing);
    }
}
