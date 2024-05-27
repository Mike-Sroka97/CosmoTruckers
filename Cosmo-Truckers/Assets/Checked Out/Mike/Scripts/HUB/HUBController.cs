using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUBController : MonoBehaviour
{
    [SerializeField] GameObject mainOptionsGO;
    [SerializeField] GameObject dimensionVoteGO;
    [SerializeField] GameObject characterSwapGO;
    [SerializeField] GameObject spellCraftingGO;
    [SerializeField] GameObject trainingGO;
    [SerializeField] GameObject emotesGO;
    [SerializeField] GameObject dataLogGO;

    int playersLockedIn;

    public void DisableAll()
    {
        playersLockedIn++;

        //start dimension load if every player in the player manager has voted
        //if(playersLockedIn < playersInLobby)


        gameObject.SetActive(false);
    }

    public void DimensionVote(bool open)
    {
        if (open)
        {
            dimensionVoteGO.SetActive(true);
            mainOptionsGO.SetActive(false);
        }
        else
        {
            dimensionVoteGO.SetActive(false);
            mainOptionsGO.SetActive(true);
        }
    }

    public void CharacterSwap(bool open)
    {
        if (open)
        {
            characterSwapGO.SetActive(true);
            mainOptionsGO.SetActive(false);
        }
        else
        {
            characterSwapGO.SetActive(false);
            mainOptionsGO.SetActive(true);
        }
    }

    public void SpellCrafting(bool open)
    {
        if (open)
        {
            spellCraftingGO.SetActive(true);
            mainOptionsGO.SetActive(false);
        }
        else
        {
            spellCraftingGO.SetActive(false);
            mainOptionsGO.SetActive(true);
        }
    }

    public void Training(bool open)
    {
        if (open)
        {
            trainingGO.SetActive(true);
            mainOptionsGO.SetActive(false);
        }
        else
        {
            trainingGO.SetActive(false);
            mainOptionsGO.SetActive(true);
        }
    }

    public void Emotes(bool open)
    {
        if (open)
        {
            emotesGO.SetActive(true);
            mainOptionsGO.SetActive(false);
        }
        else
        {
            emotesGO.SetActive(false);
            mainOptionsGO.SetActive(true);
        }
    }

    public void DataLog(bool open)
    {
        if (open)
        {
            dataLogGO.SetActive(true);
            mainOptionsGO.SetActive(false);
        }
        else
        {
            dataLogGO.SetActive(false);
            mainOptionsGO.SetActive(true);
        }
    }
}
