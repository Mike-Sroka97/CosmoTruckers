using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NebularCloudAI : Enemy
{
    bool usingMelancholyPrecipitation = true;

    struct ShockedPlayer
    {
        public ShockedPlayer(PlayerCharacter player, int stacks)
        {
            myCharacter = player;
            numberOfShocked = stacks;
        }

        public PlayerCharacter myCharacter;
        public int numberOfShocked;
    }

    public override void StartTurn()
    {
        if(usingMelancholyPrecipitation)
        {
            usingMelancholyPrecipitation = !usingMelancholyPrecipitation;
            ChosenAttack = attacks[0];
        }
        else
        {
            usingMelancholyPrecipitation = !usingMelancholyPrecipitation;
            ChosenAttack = attacks[1];
        }

        base.StartTurn();
    }

    protected override void SpecialTarget(int attackIndex)
    {
        //lol
        List<ShockedPlayer> playersWithSlow = new List<ShockedPlayer>();
        List<PlayerCharacter> playersWithoutSlow = new List<PlayerCharacter>();

        //populate targeting parameters
        List<PlayerCharacter> players = new List<PlayerCharacter>();
        PlayerCharacter[] tempCharacters = FindObjectsOfType<PlayerCharacter>();
        foreach (PlayerCharacter tempChararacter in tempCharacters)
            players.Add(tempChararacter);

        //find shocked players and number of stacks
        foreach (PlayerCharacter player in players)
        {
            List<DebuffStackSO> playerAUGs = player.GetAUGS;
            foreach (DebuffStackSO aug in playerAUGs)
            {
                if (aug.AugSpawner.GetComponent<AUG_Shocked>() && !player.Dead)
                {
                    playersWithSlow.Add(new ShockedPlayer(player, aug.CurrentStacks));
                    break;
                }
            }
        }

        //find non-shocked players
        for (int i = 0; i < players.Count; i++)
        {
            if (playersWithSlow.Count > i)
            {
                if (!players.Contains(playersWithSlow[i].myCharacter))
                    playersWithoutSlow.Add(playersWithSlow[i].myCharacter);
            }
            else
            {
                playersWithoutSlow.Add(players[i]);
            }
        }

        //Melancholy Precipitation
        if (attackIndex == 0)
        {
            //target con 1 (targets random player without shocked)
            if (playersWithoutSlow.Count > 0)
            {
                int random = Random.Range(0, playersWithoutSlow.Count);
                CombatManager.Instance.CharactersSelected.Add(playersWithoutSlow[random]);
            }

            //target con 2 (player with least shocked)
            else
            {
                Character leastShocked = null;
                int lowestStacks = 0;

                foreach(ShockedPlayer shockedPlayer in playersWithSlow)
                {
                    if (lowestStacks == 0 || shockedPlayer.numberOfShocked < lowestStacks)
                    {
                        lowestStacks = shockedPlayer.numberOfShocked;
                        leastShocked = shockedPlayer.myCharacter;
                    }
                }

                if(leastShocked)
                    CombatManager.Instance.CharactersSelected.Add(leastShocked);
            }
        }

        //Shocking Shock
        else if(attackIndex == 1)
        {
            //target con 1 (shocked players exist)
            if(playersWithSlow.Count > 0)
            {
                int random = Random.Range(0, playersWithSlow.Count);
                CombatManager.Instance.CharactersSelected.Add(playersWithSlow[random].myCharacter);
            }

            //target con 2 (no shocked players)
            else
            {
                int random = Random.Range(0, players.Count);
                while (players[random].Dead)
                {
                    random = Random.Range(0, players.Count);
                }
                CombatManager.Instance.CharactersSelected.Add(players[random]);
            }

            //target con for friendlies

            Enemy[] enemies = FindObjectsOfType<Enemy>();
            List<int> enemiesDebuffCounter = new List<int>();
            for (int i = 0; i < enemies.Length; i++)
                enemiesDebuffCounter.Add(0);
            Enemy mostDebuffedEnemy = null;
            int mostDebuffs = 0;

            //add up all the enemy debuffs
            for(int i = 0; i < enemies.Length; i++)
            {
                if(!enemies[i].Dead)
                {
                    foreach(DebuffStackSO augment in enemies[i].GetAUGS)
                    {
                        if (augment.IsDebuff)
                            enemiesDebuffCounter[i] += augment.CurrentStacks;
                    }
                }
            }
            
            //find the highest number of debuffs
            for (int i = 0; i < enemies.Length; i++)
            {
                if ((mostDebuffedEnemy == null || mostDebuffs < enemiesDebuffCounter[i]) && !enemies[i].Dead)
                {
                    mostDebuffedEnemy = enemies[i];
                }
            }

            //add enemy to the targeting
            CombatManager.Instance.CharactersSelected.Add(mostDebuffedEnemy);
        }
    }
}
