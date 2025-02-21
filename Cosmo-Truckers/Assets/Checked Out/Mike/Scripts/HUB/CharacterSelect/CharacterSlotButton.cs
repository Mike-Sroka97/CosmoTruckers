using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSlotButton : MonoBehaviour
{
    [SerializeField] Sprite myActiveSprite;
    [SerializeField] Sprite myDeactiveSprite;
    [SerializeField] Image characterImage;
    [SerializeField] int id;

    Image myImage;
    CharacterSelectController characterController;
    public int SelectedCharacterId;

    /// <summary>
    /// Sets to active button if id 0
    /// </summary>
    private void OnEnable()
    {
        myImage = GetComponent<Image>();

        if (!characterController)
            characterController = transform.parent.parent.GetComponent<CharacterSelectController>();

        if (id == 0)
        {
            myImage.sprite = myActiveSprite;
            characterController.CurrentId = id;
            ActivateMe();
        }

        SetCharacterImage();
    }

    /// <summary>
    /// set the image for the player select button
    /// </summary>
    public void SetCharacterImage()
    {
        if (!characterController)
            characterController = transform.parent.parent.GetComponent<CharacterSelectController>();

        characterImage.sprite = characterController.CharacterSprites[SelectedCharacterId];
    }

    /// <summary>
    /// Sets a new id for the character button
    /// </summary>
    /// <param name="newId"></param>
    public void SetButton(int newId)
    {
        SelectedCharacterId = newId;
        SetCharacterImage();
    }

    /// <summary>
    /// Activate this button
    /// </summary>
    public void ActivateMe()
    {
        if (!characterController)
            characterController = transform.parent.parent.GetComponent<CharacterSelectController>();

        characterController.CurrentId = id;
        characterController.ResetSelectedImages();
        characterController.SetDefaultSelectedImage();
        characterController.ResetSlotButtons();
        characterController.PrintCurrentButton();
        myImage.sprite = myActiveSprite;
    }

    /// <summary>
    /// Deactivate all before Activate
    /// </summary>
    public void DeactiveMe()
    {
        myImage = GetComponent<Image>();
        myImage.sprite = myDeactiveSprite;
    }
}
