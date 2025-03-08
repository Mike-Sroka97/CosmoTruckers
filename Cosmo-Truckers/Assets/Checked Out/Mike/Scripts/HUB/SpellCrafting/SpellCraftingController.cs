using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpellCraftingController : MonoBehaviour
{
    public Sprite[] CharacterSprites;

    [HideInInspector] public int CurrentCharacterId;
    List<SpellCraftingCharacterSelectButton> characterSelectButtons = new List<SpellCraftingCharacterSelectButton>();

    private void OnEnable()
    {
        CurrentCharacterId = PlayerManager.Instance.ActivePlayerIDs[0];

        if (characterSelectButtons.Count <= 0)
            characterSelectButtons = GetComponentsInChildren<SpellCraftingCharacterSelectButton>().ToList();

        for (int i = 0; i < 4; i++)
            characterSelectButtons[i].SetCharacterImage(CharacterSprites[PlayerManager.Instance.ActivePlayerIDs[i]]);
    }
}
