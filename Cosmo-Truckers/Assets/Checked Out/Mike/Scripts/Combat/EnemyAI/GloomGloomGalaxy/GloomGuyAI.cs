using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloomGuyAI : Enemy
{
    [HideInInspector] public Enemy ProtectedEnemy;

    public override void StartTurn()
    {
        //if(!FindObjectOfType<AUG_Bullseye>())
        //{

        //}
        //else if(FindObjectOfType<AUG_Bullseye>() && FindObjectOfType<AUG_Bullseye>().attachedPlayer != TauntedBy)
        //{

        //}
        //else
        //{
        //    ChosenAttack = 
        //}

        base.StartTurn();
    }

    protected override void SpecialTarget(int attackIndex)
    {
        //Get Players
        PlayerCharacter[] players = FindObjectsOfType<PlayerCharacter>();

        if (attackIndex == 0)
        {
            CombatManager.Instance.CharactersSelected.Add(ProtectedEnemy);

            //Find Supports
            List<PlayerCharacter> supports = new List<PlayerCharacter>();

            foreach (PlayerCharacter player in players)
                if (player.IsSupport)
                    supports.Add(player);

            int random = Random.Range(0, supports.Count);
            CombatManager.Instance.CharactersSelected.Add(supports[random]);
        }

        //Cometkaze always targets randomly
        else
        {
            int random = Random.Range(0, players.Length);
            while (players[random].Dead)
            {
                random = Random.Range(0, players.Length);
            }
            CombatManager.Instance.CharactersSelected.Add(players[random]);
        }
    }
}
