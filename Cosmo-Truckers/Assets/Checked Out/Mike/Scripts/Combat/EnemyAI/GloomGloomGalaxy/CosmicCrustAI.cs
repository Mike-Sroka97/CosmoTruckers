using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosmicCrustAI : Enemy
{

    protected override int SelectAttack()
    {
        CurrentTargets.Clear();

        //null checks
        AUG_SpikyShield liveShield = FindObjectOfType<AUG_SpikyShield>();
        AUG_NovaSword liveSword = FindObjectOfType<AUG_NovaSword>();

        //Live enemy count
        int liveEnemies = 0;
        foreach (Enemy enemy in EnemyManager.Instance.Enemies)
        {
            if (!enemy.Dead)
                liveEnemies++;
        }

        //Cosmic Caster
        if (!liveShield && !liveSword)
            ChosenAttack = attacks[0]; //0

        //Starlight Fury
        else if (liveShield && !liveSword)
            ChosenAttack = attacks[1];

        //Encrustable
        else if (!liveShield && liveSword)
            ChosenAttack = attacks[2];

        //Orbital Crust
        else if (liveShield && liveSword)
            ChosenAttack = attacks[3];

        //Spike Storm
        if (liveEnemies <= 1)
            ChosenAttack = attacks[4];

        return GetAttackIndex();
    }

    protected override void SpecialTarget(int attackIndex)
    {
        //Live enemy count (don't count self)
        List<Enemy> liveEnemies = new List<Enemy>();
        foreach (Enemy enemy in EnemyManager.Instance.Enemies)
        {
            if (!enemy.Dead && enemy != this)
                liveEnemies.Add(enemy);
        }

        //Cosmic Caster
        if(attackIndex == 0)
        {
            if(liveEnemies.Count >= 2)
            {
                int randomOne = Random.Range(0, liveEnemies.Count);
                int randomTwo = randomOne;
                while(randomTwo == randomOne)
                    randomTwo = Random.Range(0, liveEnemies.Count);

                CurrentTargets.Add(liveEnemies[randomOne]);
                CurrentTargets.Add(liveEnemies[randomTwo]);
            }
            else
            {
                CurrentTargets.Add(liveEnemies[0]);
            }

            CurrentTargets.Add(this);

            //Find Utility or random
            if (CombatManager.Instance.FindUtilityCharacter())
                CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this, CombatManager.Instance.FindUtilityCharacter());
            else
                CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }

        //Starlight Fury
        else if(attackIndex == 1)
        {
            //Find Utility or random
            if (CombatManager.Instance.FindUtilityCharacter())
                CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this, CombatManager.Instance.FindUtilityCharacter());
            else
                CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }

        //Encrustable
        else if(attackIndex == 2)
        {
            //Pick random live enemy from above list
            int random = Random.Range(0, liveEnemies.Count);
            CurrentTargets.Add(liveEnemies[random]);

            //Add Support or Random
            if (CombatManager.Instance.FindSupportCharacter())
                CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this, CombatManager.Instance.FindSupportCharacter());
            else
                CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }

        //Orbital Crust
        else if(attackIndex == 3)
        {
            CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }

        //Spike Storm
        else
        {
            CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
            CurrentTargets.Add(this);
        }
    }
}
