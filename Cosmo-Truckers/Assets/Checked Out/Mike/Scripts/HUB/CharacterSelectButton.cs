using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterSelectButton : MonoBehaviour, ISelectHandler
{
    [SerializeField] int characterID;
    [SerializeField] string characterName;
    [SerializeField] GameObject selectedGO;
    TextMeshProUGUI characterText;
    private void OnEnable()
    {
        characterText = transform.parent.parent.Find("CharacterName").GetComponent<TextMeshProUGUI>();

        foreach (CharacterSO character in PlayerManager.Instance.SelectedCharacters)
        {
            if (character.PlayerID == characterID)
            {
                selectedGO.SetActive(true);
                return;
            }
        }

        selectedGO.SetActive(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        characterText.text = characterName;
    }

    public void SelectCharacter()
    {
        if (selectedGO.activeInHierarchy)
            return;
    }
}
