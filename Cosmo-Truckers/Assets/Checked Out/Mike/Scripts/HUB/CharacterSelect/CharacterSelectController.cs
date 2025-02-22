using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharacterSelectController : MonoBehaviour
{
    public Sprite[] CharacterSprites;
    public int CurrentId;
    public List<int> InUseIds = new List<int>();
    public HUBController Hub;

    List<CharacterSlotButton> characterSlotButtons = new List<CharacterSlotButton>();
    List<CharacterSelectButton> characterSelectButtons = new List<CharacterSelectButton>();

    private void Awake()
    {
        Hub = GetComponentInParent<HUBController>();
    }

    /// <summary>
    /// Sets characters to load for a dungeon
    /// </summary>
    private void OnDisable()
    {
        //Set characters to load in dungeon
    }

    /// <summary>
    /// Default print
    /// </summary>
    public void PrintCurrentButton()
    {
        if(!characterSelectButtons[CurrentId].CurrentlyPrinting)
            StartCoroutine(characterSelectButtons[CurrentId].PrintText());
    }

    public void PopulateBaseData()
    {
        if (characterSlotButtons.Count <= 0)
        {
            characterSlotButtons = GetComponentsInChildren<CharacterSlotButton>(true).ToList();
            characterSelectButtons = GetComponentsInChildren<CharacterSelectButton>(true).ToList();

            foreach (CharacterSlotButton button in characterSlotButtons)
                InUseIds.Add(button.SelectedCharacterId);
        }
    }

    /// <summary>
    /// Set default image for select buttons
    /// </summary>
    public void SetDefaultSelectedImage()
    {
        characterSelectButtons[characterSlotButtons[CurrentId].SelectedCharacterId].OverrideSelectCharacter();
    }

    /// <summary>
    /// Resets all button colors
    /// </summary>
    public void ResetSlotButtons()
    {
        foreach (CharacterSlotButton button in characterSlotButtons)
            button.DeactiveMe();
    }

    /// <summary>
    /// Reset any selected images
    /// </summary>
    public void ResetSelectedImages()
    {
        foreach (CharacterSelectButton button in characterSelectButtons)
            button.DeselectCharacter();
    }

    /// <summary>
    /// Updates the image on the selected button
    /// </summary>
    /// <param name="newId"></param>
    public void UpdateCharacterImageOnSlotButton(int newId)
    {
        InUseIds[CurrentId] = newId;
        characterSlotButtons[CurrentId].SetButton(newId);
    }
}
