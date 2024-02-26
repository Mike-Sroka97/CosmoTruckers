using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WyingDreemAI : Enemy
{
    [SerializeField] int nitemareStacksToKill = 5;
    [SerializeField] int moveOneStackCon = 4;
    [SerializeField] int moveTwoStackCon = 2;
    [SerializeField] int moveOneWeight = 3;
    [SerializeField] int moveTwoWeight = 1;
    [SerializeField] int moveFourWeight = 2;
    const string debuffName = "Nitemare";
    PlayerCharacter[] players;
    PlayerCharacter playerToKill;

    protected override int SelectAttack()
    {
        CurrentTargets.Clear();

        PlayerCharacter[] players = FindObjectsOfType<PlayerCharacter>();
        bool attackChosen = false;

        //kill a player if they have too much nitemare
        foreach (PlayerCharacter player in players)
        {
            foreach (DebuffStackSO augment in player.GetAUGS)
            {
                if (augment.DebuffName == debuffName && augment.CurrentStacks >= nitemareStacksToKill && !player.Dead)
                {
                    //Taunt check
                    if (TauntedBy && TauntedBy != player && !TauntedBy.Dead)
                    {
                        break;
                    }
                    //death kill
                    ChosenAttack = attacks[2];
                    attackChosen = true;
                    playerToKill = player;
                }
            }
        }

        //weighted attacks
        if (!attackChosen)
        {
            int maxWeight = moveOneWeight + moveTwoWeight + moveFourWeight;
            int random = Random.Range(0, maxWeight);

            if (random <= moveOneWeight)
            {
                //bad dreem
                ChosenAttack = attacks[0];
            }
            else if (random > moveOneWeight && random <= moveOneWeight + moveTwoWeight)
            {
                //freak out
                ChosenAttack = attacks[1]; //1
            }
            else
            {
                //split misery
                ChosenAttack = attacks[3]; //3
            }
        }

        return GetAttackIndex();
    }
    protected override void SpecialTarget(int attackIndex)
    {
        //Get Players
        players = FindObjectsOfType<PlayerCharacter>();

        //Bad Dreem
        if (attackIndex == 0)
        {
            List<PlayerCharacter> fourNitemareCharacters = new List<PlayerCharacter>();

            //Determine which characters have exactly four stacks of Nitemare
            fourNitemareCharacters = DetermineNitemare(moveOneStackCon);

            //Target
            if (TauntedBy)
            {
                CombatManager.Instance.DetermineTauntedTarget(this);
                return;
            }
            else if (fourNitemareCharacters.Count > 0)
            {
                int random = Random.Range(0, fourNitemareCharacters.Count);
                CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this, fourNitemareCharacters[random]);
            }
            else
            {
                int random = Random.Range(0, players.Length);
                CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this, players[random]);
            }
        }
        //Freak Out
        else if(attackIndex == 1)
        {
            List<PlayerCharacter> threeNitemareCharacters = new List<PlayerCharacter>();

            //Determine which characters have exactly three stacks of Nitemare
            threeNitemareCharacters = DetermineNitemare(moveTwoStackCon);

            //Target
            if (TauntedBy)
            {
                CombatManager.Instance.DetermineTauntedTarget(this);
            }
            else if (threeNitemareCharacters.Count > 0)
            {
                int random = Random.Range(0, threeNitemareCharacters.Count);
                CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this, threeNitemareCharacters[random]);
            }
            else
            {
                CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
            }

            CombatManager.Instance.ConeTargetEnemy(ChosenAttack, this, CurrentTargets[0]);
        }
        //Death Kill
        else if (attackIndex == 2)
        {
            if (playerToKill == null)
                playerToKill = EnemyManager.Instance.Players[0];

            CurrentTargets.Add(playerToKill);
            CurrentTargets.Add(this);
        }
        //Split Misery
        else
        {
            //Target Con 1
            PlayerCharacter playerWithMostNitemare = null;
            int currentMostNitemare = -1;

            foreach(PlayerCharacter player in players)
            {
                foreach (DebuffStackSO augment in player.GetAUGS)
                {
                    if (augment.DebuffName == debuffName && augment.CurrentStacks >= currentMostNitemare)
                    {
                        currentMostNitemare = augment.CurrentStacks;
                        playerWithMostNitemare = player;
                    }
                }
            }

            if(CombatManager.Instance.CharactersSelected.Count == 0)
            {
                if (currentMostNitemare > 0)
                {
                    CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this, playerWithMostNitemare);
                }
                else
                {
                    CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
                }
            }

            foreach (PlayerCharacter player in players)
            {
                if (!CombatManager.Instance.CharactersSelected.Contains(player))
                {
                    CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this, player);
                    break;
                }
            }
        }
    }

    private List<PlayerCharacter> DetermineNitemare(int stacks)
    {
        List<PlayerCharacter> nitemaredCharacters = new List<PlayerCharacter>();

        foreach (PlayerCharacter player in players)
        {
            foreach (DebuffStackSO augment in player.GetAUGS)
            {
                if (augment.DebuffName == "Nitemare" && augment.CurrentStacks == stacks)
                {
                    nitemaredCharacters.Add(player);
                }
            }
        }

        return nitemaredCharacters;
    }
}
