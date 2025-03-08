using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellCraftingCharacterSelectButton : MonoBehaviour
{
    [SerializeField] int id;

    SpellCraftingController controller;
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
            controller = MathHelpers.FindNearestParentOfType<SpellCraftingController>(transform);

        controller.CurrentCharacterId = PlayerManager.Instance.ActivePlayerIDs[id];
    }
}
