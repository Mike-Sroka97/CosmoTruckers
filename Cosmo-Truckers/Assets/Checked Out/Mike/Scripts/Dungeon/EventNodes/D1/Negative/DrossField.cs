using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrossField : EventNodeBase
{
    [SerializeField] string responseText;

    public void AcceptOption()
    {
        //Swap reflex
        List<PlayerVessel> randomOrder = new List<PlayerVessel>(PlayerVesselManager.Instance.PlayerVessels);
        randomOrder.Shuffle();

        foreach (PlayerVessel player in PlayerVesselManager.Instance.PlayerVessels)
        {
            player.MyCharacter.AddAugmentStack(augmentsToAdd[0]);
            player.MyCharacter.Stats.Reflex = randomOrder[0].MyCharacter.Stats.Reflex;

            randomOrder.RemoveAt(0);
        }

        //cleanup
        descriptionText.text = responseText;
        IteratePlayerReference();
        StartCoroutine(SelectionChosen());
    }
    public override void HandleButtonSelect(int buttonId)
    {
        PopupOne.gameObject.SetActive(true);

        SetButtonWithAugInfo(augmentsToAdd[buttonId]);
    }
}
