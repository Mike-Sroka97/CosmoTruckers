using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatTonguerAI : Enemy
{
    [SerializeField] int moveOneWeight;
    [SerializeField] int moveTwoWeight;
    [SerializeField] int moveThreeWeight;
    [SerializeField] int moveFourWeight;

    public override void StartTurn()
    {
        int maxWeight = moveOneWeight + moveTwoWeight + moveThreeWeight + moveFourWeight;
        int random = Random.Range(0, maxWeight);

        if (random <= moveOneWeight)
        {
            //To Tongue or Not to Tongue
            ChosenAttack = attacks[0];
        }
        else if (random > moveOneWeight && random <= moveOneWeight + moveTwoWeight)
        {
            //Solid Shlurping
            ChosenAttack = attacks[1];
        }
        else if (random > moveOneWeight + moveTwoWeight && random <= moveOneWeight + moveTwoWeight + moveThreeWeight)
        {
            //Spit Ball
            ChosenAttack = attacks[2];
        }
        else
        {
            //Cumulo-Lickus
            if (EnemyManager.Instance.GetAliveEnemies().Count <= 1)
                ChosenAttack = attacks[0];
            else
                ChosenAttack = attacks[3];
        }

        base.StartTurn();
    }

    protected override void SpecialTarget(int attackIndex)
    {
        List<PlayerCharacter> players = EnemyManager.Instance.GetAlivePlayerCharacters();
        List<Enemy> enemies = EnemyManager.Instance.GetAliveEnemies();
        List<Enemy> debuffedEnemies = new List<Enemy>();
        List<Enemy> halfHealthEnemies = new List<Enemy>();

        //To Tongue or Not to Tongue
        if (attackIndex == 0)
        {
            CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }

        //Solid Shlurping
        else if(attackIndex == 1)
        {
            foreach(Enemy enemy in enemies)
            {
                foreach(DebuffStackSO augment in enemy.GetAUGS)
                {
                    if(augment.IsDebuff)
                    {
                        debuffedEnemies.Add(enemy);
                        break;
                    }
                }
                if(enemy.CurrentHealth <= enemy.Health / 2)
                {
                    halfHealthEnemies.Add(enemy);
                }
            }

            //Target Con 1
            bool supportFound = false;
            bool enemyFound = false;

            if(debuffedEnemies.Count > 0)
            {
                int random = Random.Range(0, debuffedEnemies.Count);
                CombatManager.Instance.CharactersSelected.Add(debuffedEnemies[random]);
                enemyFound = true;
            }

            //Target Con 2
           if(halfHealthEnemies.Count > 0 && !enemyFound)
            {
                int random = Random.Range(0, halfHealthEnemies.Count);
                CombatManager.Instance.CharactersSelected.Add(halfHealthEnemies[random]);
                enemyFound = true;
            }

            //Target Con 3
            if(!enemyFound)
            {
                int random = Random.Range(0, enemies.Count);
                CombatManager.Instance.CharactersSelected.Add(enemies[random]);
            }

            //Player target
            //Add Support or Random
            if (CombatManager.Instance.FindSupportCharacter())
                CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this, CombatManager.Instance.FindSupportCharacter());
            else
                CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }

        //Spit Ball
        else if (attackIndex == 2)
        {
            CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
            CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }

        //Cumulo-Lickus
        else
        {
            CombatManager.Instance.AllTargetEnemy(this, ChosenAttack);
        }
    }
}
