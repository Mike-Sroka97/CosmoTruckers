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
        myImage.material = PlayerVesselManager.Instance.PlayerVessels[nodeHandler.Player].GetCharacterImage().material;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if(!MultiplayerSelected)
            myImage.material = null;
        else
        {
            int adjustedPlayer = nodeHandler.Player - 1;
            if (adjustedPlayer < 0)
                adjustedPlayer = 3;

            myImage.material = PlayerVesselManager.Instance.PlayerVessels[adjustedPlayer].GetCharacterImage().material;
        }

    }

    public void ResetMaterial()
    {
        myImage.material = PlayerVesselManager.Instance.PlayerVessels[nodeHandler.Player].GetCharacterImage().material;
    }

    public void DeleteMaterial()
    {
        myImage.material = null;
    }
}
