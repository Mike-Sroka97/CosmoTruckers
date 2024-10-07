using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EventNodeButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public bool MultiplayerSelected;

    EventNodeHandler nodeHandler;
    Image myImage;

    private void Awake()
    {
        nodeHandler = FindObjectOfType<EventNodeHandler>();
        myImage = GetComponent<Image>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        myImage.material = nodeHandler.OutlineMaterials[nodeHandler.Player];
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if(!MultiplayerSelected)
            myImage.material = null;
    }

    private void OnDisable()
    {
        myImage.material = nodeHandler.OutlineMaterials[nodeHandler.Player];
    }
}
