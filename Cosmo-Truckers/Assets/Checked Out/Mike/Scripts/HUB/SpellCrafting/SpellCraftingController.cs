using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using static SpellCraftingCharacterData;

public class SpellCraftingController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI yapAura;
    [SerializeField] CharacterSpellData[] spellData;
    [SerializeField] TextMeshProUGUI commonTokens;
    [SerializeField] TextMeshProUGUI rareTokens;
    [SerializeField] TextMeshProUGUI legendaryTokens;

    public Sprite[] CharacterSprites;

    [HideInInspector] public int CurrentCharacterId;
    [HideInInspector] public PlayerData PlayerData;
    List<SpellCraftingCharacterSelectButton> characterSelectButtons = new List<SpellCraftingCharacterSelectButton>();
    List<SpellCraftingButton> spellAndSpecButtons = new List<SpellCraftingButton>();

    private void OnEnable()
    {
        CurrentCharacterId = PlayerManager.Instance.ActivePlayerIDs[0];

        //set player select buttons
        if (characterSelectButtons.Count <= 0)
            characterSelectButtons = GetComponentsInChildren<SpellCraftingCharacterSelectButton>().ToList();

        characterSelectButtons[0].SelectMe();

        for (int i = 0; i < characterSelectButtons.Count; i++)
            characterSelectButtons[i].SetCharacterImage(CharacterSprites[PlayerManager.Instance.ActivePlayerIDs[i]]);

        //set token text
        PlayerData = SaveManager.LoadPlayerData();

        SetTokenText();

        //set spell and spec buttons
        if (spellAndSpecButtons.Count <= 0)
            spellAndSpecButtons = GetComponentsInChildren<SpellCraftingButton>().ToList();

        ResetSpellUnlockedStatus();
    }

    public void SetYapAura(int id, bool spec)
    {
        if(spec)
            yapAura.text = spellData[CurrentCharacterId].specs[id];
        else
            yapAura.text = spellData[CurrentCharacterId].spells[id];
    }

    public void ResetYapAura()
    {
        yapAura.text = "";
    }

    public void ResetSelectedCharacter()
    {
        foreach (SpellCraftingCharacterSelectButton button in characterSelectButtons)
            button.ResetButton();

        ResetSpellUnlockedStatus();
    }

    public void ResetLockShake()
    {
        foreach (SpellCraftingButton button in spellAndSpecButtons)
            if(!button.Spec)
                button.LockShake(false);
    }

    private void ResetSpellUnlockedStatus()
    {
        foreach (SpellCraftingButton button in spellAndSpecButtons)
            if (!button.Spec)
                button.CheckUnlockedStatus();
    }

    public void SetTokenText()
    {
        PlayerData = SaveManager.LoadPlayerData();

        commonTokens.text = $"x{PlayerData.CommonSpellTokens} C";
        rareTokens.text = $"x{PlayerData.RareSpellTokens} R";
        legendaryTokens.text = $"x{PlayerData.LegendarySpellTokens} L";
    }
}
