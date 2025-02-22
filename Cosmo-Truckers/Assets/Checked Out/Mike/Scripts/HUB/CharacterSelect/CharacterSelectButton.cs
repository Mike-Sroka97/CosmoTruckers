using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterSelectButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] int characterID;
    [SerializeField] string characterName;
    [SerializeField] [TextArea] string characterDescription;
    [SerializeField] GameObject selectedGO;
    [SerializeField] float characterWaitTime;
    [HideInInspector] public bool CurrentlyPrinting;
    TextMeshProUGUI characterText;
    TextMeshProUGUI characterYapAuraText;
    CharacterSelectController characterController;

    private void OnEnable()
    {
        if (!characterController)
            characterController = transform.parent.parent.GetComponent<CharacterSelectController>();

        characterText = transform.parent.parent.Find("CharacterName").GetComponent<TextMeshProUGUI>();
        characterYapAuraText = transform.parent.parent.Find("CharacterYapAura").GetComponent<TextMeshProUGUI>();

        //foreach (CharacterSO character in PlayerManager.Instance.SelectedCharacters)
        //{
        //    if (character.PlayerID == characterID)
        //    {
        //        selectedGO.SetActive(true);
        //        return;
        //    }
        //}
    }

    public void OnSelect(BaseEventData eventData)
    {
        if(!CurrentlyPrinting)
            StartCoroutine(PrintText());
    }

    public IEnumerator PrintText()
    {
        CurrentlyPrinting = true;
        characterText = transform.parent.parent.Find("CharacterName").GetComponent<TextMeshProUGUI>();
        characterYapAuraText = transform.parent.parent.Find("CharacterYapAura").GetComponent<TextMeshProUGUI>();

        characterText.text = "";
        characterYapAuraText.text = "";

        int currentYapCharacter = 0;

        while(currentYapCharacter < characterDescription.Length)
        {
            if(currentYapCharacter < characterName.Length)
                characterText.text += characterName[currentYapCharacter];

            characterYapAuraText.text += characterDescription[currentYapCharacter];

            currentYapCharacter++;

            yield return new WaitForSeconds(characterWaitTime);
        }

        CurrentlyPrinting = false;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        StopAllCoroutines();
    }

    public void SelectCharacter()
    {
        if (selectedGO.activeInHierarchy || characterController.InUseIds.Contains(characterID))
            return;

        characterController.ResetSelectedImages();
        selectedGO.SetActive(true);
        characterController.UpdateCharacterImageOnSlotButton(characterID);
    }

    public void OverrideSelectCharacter()
    {
        if (!characterController)
            characterController = transform.parent.parent.GetComponent<CharacterSelectController>();

        selectedGO.SetActive(true);
    }

    public void DeselectCharacter()
    {
        if (selectedGO)
            selectedGO.SetActive(false);
    }

    public void SetCharacterIDs()
    {
        PlayerManager.Instance.SetActivePlayers(characterController.InUseIds);
    }
}
