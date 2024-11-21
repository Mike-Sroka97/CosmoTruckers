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
    EventNodeBase node;
    int buttonID;

    private void Awake()
    {
        nodeHandler = FindObjectOfType<EventNodeHandler>();
        myImage = GetComponent<Image>();
        node = transform.parent.parent.GetComponent<EventNodeBase>();
        if (name.Contains("3"))
            buttonID = 3;
        else if (name.Contains("2"))
            buttonID = 2;
        else if (name.Contains("1"))
            buttonID = 1;
        else
            buttonID = 0;
    }

    public void OnSelect(BaseEventData eventData)
    {
        myImage.material = PlayerVesselManager.Instance.PlayerVessels[nodeHandler.Player].GetCharacterImage().material;
        node.HandleButtonSelect(buttonID);
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

        node.HandleButtonDeselect();
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
