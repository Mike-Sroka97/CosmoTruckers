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
    TextMeshProUGUI characterText;
    TextMeshProUGUI characterYapAuraText;
    private void OnEnable()
    {
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

        if (selectedGO)
            selectedGO.SetActive(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        StartCoroutine(PrintText());
    }

    IEnumerator PrintText()
    {
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
    }

    public void OnDeselect(BaseEventData eventData)
    {
        StopAllCoroutines();
    }

    public void SelectCharacter()
    {
        if (selectedGO.activeInHierarchy)
            return;

        selectedGO.SetActive(true);

        //TODO actual code
    }


}
