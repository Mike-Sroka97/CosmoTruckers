using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosmicCrustAI : Enemy
{
    public override void StartTurn()
    {
        //null checks
        AUG_SpikyShield liveShield = FindObjectOfType<AUG_SpikyShield>();
        AUG_NovaSword liveSword = FindObjectOfType<AUG_NovaSword>();

        //Live enemy count
        int liveEnemies = 0;
        foreach(Enemy enemy in EnemyManager.Instance.Enemies)
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

        base.StartTurn();
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

        //Live player count
        List<PlayerCharacter> livePlayers = new List<PlayerCharacter>();
        foreach(PlayerCharacter player in EnemyManager.Instance.Players)
        {
            if (!player.Dead)
                livePlayers.Add(player);
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

                CombatManager.Instance.CharactersSelected.Add(liveEnemies[randomOne]);
                CombatManager.Instance.CharactersSelected.Add(liveEnemies[randomTwo]);
            }
            else
            {
                CombatManager.Instance.CharactersSelected.Add(liveEnemies[0]);
            }

            CombatManager.Instance.CharactersSelected.Add(this);

            foreach (PlayerCharacter player in livePlayers)
                if (player.IsUtility)
                {
                    CombatManager.Instance.ActivePlayers.Add(player); 
                    return;
                }

            CombatManager.Instance.ActivePlayers.Add(livePlayers[Random.Range(0, livePlayers.Count)]);
        }

        //Starlight Fury
        else if(attackIndex == 1)
        {
            if(TauntedBy)
            {
                CombatManager.Instance.DetermineTauntedTarget(this);
                return;
            }

            int random;

            //Find utlitiy
            List<PlayerCharacter> utilities = new List<PlayerCharacter>();
            foreach (PlayerCharacter player in livePlayers)
            {
                if (player.IsUtility)
                    utilities.Add(player);
            }

            if (utilities.Count > 0)
            {
                random = Random.Range(0, utilities.Count);
                CombatManager.Instance.CharactersSelected.Add(utilities[random]);
            }
            else
            {
                random = Random.Range(0, livePlayers.Count);
                CombatManager.Instance.CharactersSelected.Add(livePlayers[random]);
            }
        }

        //Encrustable
        else if(attackIndex == 2)
        {
            //Pick random live enemy from above list
            int random = Random.Range(0, liveEnemies.Count);
            CombatManager.Instance.CharactersSelected.Add(liveEnemies[random]);

            //Find support
            List<PlayerCharacter> supports = new List<PlayerCharacter>();
            foreach(PlayerCharacter player in livePlayers)
            {
                if (player.IsSupport)
                    supports.Add(player);
            }

            if(supports.Count > 0)
            {
                random = Random.Range(0, supports.Count);
                CombatManager.Instance.CharactersSelected.Add(supports[random]);
            }
            else
            {
                random = Random.Range(0, livePlayers.Count);
                CombatManager.Instance.CharactersSelected.Add(livePlayers[random]);
            }
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
            CombatManager.Instance.CharactersSelected.Add(this);
        }
    }
}
