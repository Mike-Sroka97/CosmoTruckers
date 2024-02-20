using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostDulaxy : NCNodePopUpOptions
{
    [SerializeField] int hpToCheck = 50;
    [SerializeField] GameObject miniDulaxy;

    public override void SetUp(DebuffStackSO[] augs)
    {
        if (EnemyManager.Instance.PlayerSummons.Count >= EnemyManager.Instance.Players.Count)
        {
            NoSummonForYou();
        }
        else
        {

            PlayerCharacter youGetAGift = null;

            //Go through all players
            for (int i = 0; i < EnemyManager.Instance.Players.Count; i++)
            {
                //If they have a summon they can not get this buff
                if (!CheckForSummon(EnemyManager.Instance.Players[i]))
                {
                    //Enure current HP is below the check threshold or lower than the last player to pass the check
                    if (youGetAGift == null && EnemyManager.Instance.Players[i].CurrentHealth < hpToCheck)
                    {
                        youGetAGift = EnemyManager.Instance.Players[i];
                    }
                    else if (youGetAGift != null && EnemyManager.Instance.Players[i].CurrentHealth < youGetAGift.CurrentHealth)
                    {
                        youGetAGift = EnemyManager.Instance.Players[i];
                    }
                }
            }

            if (youGetAGift == null)
                NoSummonForYou();
            else
                YouGetANewSummon(youGetAGift);
        }

        StartCoroutine(ShowTextWait());
    }

    void NoSummonForYou()
    {
        currentPlayer.text = "Sorry no Dulaxy for you";
    }
    void YouGetANewSummon(PlayerCharacter player)
    {
        currentPlayer.text = $"{player.CharacterName} has a new Dulaxy pet";
        //TODO set this up
        EnemyManager.Instance.UpdatePlayerSummons(miniDulaxy, player);
    }

    IEnumerator ShowTextWait()
    {
        yield return new WaitForSeconds(2.5f);

        Destroy(this.gameObject);
    }

    bool CheckForSummon(PlayerCharacter player)
    {
        foreach (PlayerCharacterSummon sum in EnemyManager.Instance.PlayerSummons)
        {
            if (sum.CombatSpot + 4 == player.CombatSpot)
                return true;
        }

        return false;
    }
}
