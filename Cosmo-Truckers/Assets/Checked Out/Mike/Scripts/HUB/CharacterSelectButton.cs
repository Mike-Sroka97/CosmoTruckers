using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterSelectButton : MonoBehaviour, ISelectHandler
{
    [SerializeField] string characterName;
    TextMeshProUGUI characterText;
    private void Start()
    {
        characterText = transform.parent.parent.Find("CharacterName").GetComponent<TextMeshProUGUI>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        characterText.text = characterName;
    }
}
