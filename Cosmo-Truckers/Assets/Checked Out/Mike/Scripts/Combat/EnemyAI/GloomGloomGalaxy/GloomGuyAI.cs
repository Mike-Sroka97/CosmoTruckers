using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloomGuyAI : Enemy
{
    public override void StartTurn()
    {
        if (!FindObjectOfType<AUG_BullsEye>())
        {
            ChosenAttack = attacks[0];
        }
        else if (FindObjectOfType<AUG_BullsEye>() && FindObjectOfType<AUG_BullsEye>().AugmentSO.MyCharacter != TauntedBy)
        {
            ChosenAttack = attacks[1];
        }
        else
        {
            ChosenAttack = attacks[2];
        }

        base.StartTurn();
    }

    protected override void SpecialTarget(int attackIndex)
    {
        //Get Players
        PlayerCharacter[] players = FindObjectsOfType<PlayerCharacter>();

        //Large Iron
        if (attackIndex == 0)
        {
            if (TauntedBy)
            {
                CombatManager.Instance.DetermineTauntedTarget(this);
                return;
            }

            List<PlayerCharacter> nonTanks = new List<PlayerCharacter>();
            List<PlayerCharacter> tanks = new List<PlayerCharacter>();

            foreach(PlayerCharacter player in players)
            {
                if (player.IsTank)
                    tanks.Add(player);
                else
                    nonTanks.Add(player);
            }

            //Target Con 1 non-tank
            bool allDead = true;

            foreach (PlayerCharacter nonTank in nonTanks)
            {
                if (!nonTank.Dead)
                {
                    allDead = false;
                }
                else
                {
                    nonTanks.Remove(nonTank);
                }
            }

            if(!allDead)
            {
                int random = Random.Range(0, nonTanks.Count);
                CombatManager.Instance.CharactersSelected.Add(nonTanks[random]);
            }

            //Target Con 2 tank

            if(allDead)
            {
                foreach(PlayerCharacter tank in tanks)
                {
                    if(tank.Dead)
                    {
                        tanks.Remove(tank);
                    }
                }

                int random = Random.Range(0, tanks.Count);
                CombatManager.Instance.CharactersSelected.Add(tanks[random]);
            }

            return;
        }

        //FanTheHammer && HorsingAround (hits taunted character)
        CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
    }
}
