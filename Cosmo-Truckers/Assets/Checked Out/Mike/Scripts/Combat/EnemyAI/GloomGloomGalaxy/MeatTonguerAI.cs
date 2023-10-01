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
            ChosenAttack = attacks[3];
        }

        base.StartTurn();
    }

    protected override void SpecialTarget(int attackIndex)
    {
        PlayerCharacter[] players = FindObjectsOfType<PlayerCharacter>();
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        List<Enemy> debuffedEnemies = new List<Enemy>();
        List<Enemy> halfHealthEnemies = new List<Enemy>();

        List<PlayerCharacter> alivePlayers = new List<PlayerCharacter>();
        foreach (PlayerCharacter player in players)
        {
            if (!player.Dead)
            {
                alivePlayers.Add(player);
            }
        }

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
                int random = Random.Range(0, enemies.Length);
                CombatManager.Instance.CharactersSelected.Add(enemies[random]);
            }

            //Player target
            foreach (PlayerCharacter player in players)
            {
                if (player.IsSupport && !player.Dead)
                {
                    CombatManager.Instance.CharactersSelected.Add(player);
                    supportFound = true;
                    break;
                }
            }

            if (!supportFound)
            {
                int randomPlayer = Random.Range(0, alivePlayers.Count);
                CombatManager.Instance.CharactersSelected.Add(alivePlayers[randomPlayer]);
            }

            //Target
            if (TauntedBy)
            {
                if (CombatManager.Instance.CheckPlayerSummonLayer(TauntedBy.CombatSpot[0]))
                {
                    if (!CombatManager.Instance.CharactersSelected.Contains(TauntedBy))
                        CombatManager.Instance.CharactersSelected.Add(EnemyManager.Instance.PlayerSummons[TauntedBy.CombatSpot[0]]);
                    return;
                }
                else if (!EnemyManager.Instance.Players[TauntedBy.CombatSpot[0]].Dead)
                {
                    if (!CombatManager.Instance.CharactersSelected.Contains(TauntedBy))
                        CombatManager.Instance.CharactersSelected.Add(EnemyManager.Instance.Players[TauntedBy.CombatSpot[0]]);
                    return;
                }
            }
            else
            {
                int randomPlayer = Random.Range(0, alivePlayers.Count);
                if (!CombatManager.Instance.CharactersSelected.Contains(alivePlayers[randomPlayer]))
                    CombatManager.Instance.CharactersSelected.Add(debuffedEnemies[randomPlayer]);
            }
        }

        //Spit Ball
        else if (attackIndex == 1)
        {
            CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }

        //Cumulo-Lickus
        else
        {
            CombatManager.Instance.AllTargetEnemy(ChosenAttack);
        }
    }
}
