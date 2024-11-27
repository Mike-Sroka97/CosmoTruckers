using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUBController : MonoBehaviour
{
    [SerializeField] GameObject mainOptionsGO;
    [SerializeField] GameObject dimensionVoteGO;
    [SerializeField] GameObject characterSwapGO;
    [SerializeField] GameObject spellCraftingGO;
    [SerializeField] InaPractice trainingIna;
    [SerializeField] GameObject emotesGO;
    [SerializeField] GameObject dataLogGO;
    [SerializeField] MainHubButton[] mainHubButtons;

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
            CloseButtons();
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
            CloseButtons();
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
            CloseButtons();
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
            CloseButtons();
            mainOptionsGO.SetActive(false);
            trainingIna.Hub = this;
            StartCoroutine(trainingIna.MoveINACombat(true));
        }
        else
        {
            mainOptionsGO.SetActive(true);
        }
    }

    public void Emotes(bool open)
    {
        if (open)
        {
            CloseButtons();
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
            CloseButtons();
            dataLogGO.SetActive(true);
            mainOptionsGO.SetActive(false);
        }
        else
        {
            dataLogGO.SetActive(false);
            mainOptionsGO.SetActive(true);
        }
    }

    private void CloseButtons()
    {
        foreach (MainHubButton button in mainHubButtons)
            StartCoroutine(button.MoveMe(false));
    }
}
