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

    [SerializeField] Sprite[] silhouetteSprites;
    [HideInInspector] public CharacterSilhouette[] CharacterSilhouettes;
    CharacterSelectController characterSelectController;
    PlayerData playerData;

    [Space(50)]
    [Header("DEBUG")]
    public bool AllCharactersUnlocked;

    private void Start()
    {

        playerData = SaveManager.LoadPlayerData();
        CameraController.Instance.StartCoroutine(CameraController.Instance.FadeVignette(true));
        CameraController.Instance.NormalizePositionRotation();

        CharacterSilhouettes = GetComponentsInChildren<CharacterSilhouette>(true);
        characterSelectController = GetComponentInChildren<CharacterSelectController>(true);
        characterSelectController.PopulateBaseData();
        SetSilhouettes();
    }

    public void SetSilhouettes()
    {
        PlayerData data = SaveManager.LoadPlayerData();
        for (int i = 0; i < data.SelectedCharacters.Count; i++)
            CharacterSilhouettes[i].SetSprite(silhouetteSprites[data.SelectedCharacters[i]]);
    }

    public void SetSilhouette(int i)
    {
        CharacterSilhouettes[i].SetSprite(silhouetteSprites[characterSelectController.PlayerData.SelectedCharacters[i]]);
    }

    public void AnimateSilhouette(int i)
    {
        CharacterSilhouettes[i].GetComponent<Animator>().Play("Jaunt Down");
    }

    public void DisableAll()
    {
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
