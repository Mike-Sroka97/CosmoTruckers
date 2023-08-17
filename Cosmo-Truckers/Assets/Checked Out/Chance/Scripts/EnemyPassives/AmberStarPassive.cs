using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmberStarPassive : EnemyPassiveBase
{
    [SerializeField] DebuffStackSO debuffToAdd;

    public override void Activate()
    {
        List<PlayerCharacter> players = FindObjectOfType<EnemyManager>().Players;

        for (int i = 0; i < players.Count; i++)
        {
            if (!players[i].GetAUGS.Contains(debuffToAdd))
            {
                players[i].AddDebuffStack(debuffToAdd);
                print($"{players[i].GetName()} has been debuffed");
                return;
            }
        }
    }
}
