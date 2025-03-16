using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellCraftingCharacterSelectButton : MonoBehaviour
{
    [SerializeField] Sprite myActiveSprite;
    [SerializeField] Sprite myDeactiveSprite;
    [SerializeField] int id;

    SpellCraftingController controller;
    Image myImage;
    Image characterImage;

    /// <summary>
    /// set the image for the player select button
    /// </summary>
    public void SetCharacterImage(Sprite characterSprite)
    {
        if (!characterImage)
            characterImage = transform.Find("characterImage").GetComponent<Image>();

        characterImage.sprite = characterSprite;
    }

    /// <summary>
    /// Update current id
    /// </summary>
    public void SelectMe()
    {
        if(!controller)
            controller = HelperFunctions.FindNearestParentOfType<SpellCraftingController>(transform);

        controller.CurrentCharacterId = controller.PlayerData.SelectedCharacters[id];

        controller.ResetSelectedCharacter();
        myImage.sprite = myActiveSprite;
    }

    public void ResetButton()
    {
        if (!myImage)
            myImage = GetComponent<Image>();

        myImage.sprite = myDeactiveSprite;
    }
}
