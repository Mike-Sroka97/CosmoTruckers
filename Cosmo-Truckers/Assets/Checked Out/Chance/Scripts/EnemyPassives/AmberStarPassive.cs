using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmberStarPassive : EnemyPassiveBase
{
    [SerializeField] AugmentStackSO debuffToAdd;

    public override void Activate(Enemy enemy)
    {
        List<PlayerCharacter> players = FindObjectOfType<EnemyManager>().Players;

        for (int i = 0; i < players.Count; i++)
        {
            if (!players[i].GetAUGS.Contains(debuffToAdd))
            {
                debuffToAdd.AugSpawner.GetComponent<AUG_Resinated>().enemyToCheck = enemy;
                debuffToAdd.AugSpawner.GetComponent<AUG_Resinated>().playerToCheck = players[i];
                players[i].AddAugmentStack(debuffToAdd);
                print($"{players[i].CharacterName} has been debuffed");
                return;
            }
        }
    }
}
