using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TownButtons : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] Sprite imageToDisplay;
    [SerializeField] Image imageToChange;
    public void OnPointerEnter(PointerEventData eventData)
    {
        imageToChange.color = new Color(1, 1, 1, 1);
        imageToChange.sprite = imageToDisplay;
    }
}
