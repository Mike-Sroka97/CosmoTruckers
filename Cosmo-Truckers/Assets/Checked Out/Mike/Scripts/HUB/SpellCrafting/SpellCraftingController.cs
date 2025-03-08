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

    public Sprite[] CharacterSprites;

    [HideInInspector] public int CurrentCharacterId;
    List<SpellCraftingCharacterSelectButton> characterSelectButtons = new List<SpellCraftingCharacterSelectButton>();

    private void OnEnable()
    {
        CurrentCharacterId = PlayerManager.Instance.ActivePlayerIDs[0];

        if (characterSelectButtons.Count <= 0)
            characterSelectButtons = GetComponentsInChildren<SpellCraftingCharacterSelectButton>().ToList();

        characterSelectButtons[0].SelectMe();

        for (int i = 0; i < 4; i++)
            characterSelectButtons[i].SetCharacterImage(CharacterSprites[PlayerManager.Instance.ActivePlayerIDs[i]]);
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
    }
}
