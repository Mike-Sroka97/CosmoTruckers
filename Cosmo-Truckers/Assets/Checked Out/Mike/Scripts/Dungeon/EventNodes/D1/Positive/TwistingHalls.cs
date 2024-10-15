using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TwistingHalls : EventNodeBase
{
    [SerializeField] string goodText;
    [SerializeField] string badText;

    bool leftHallGood;
    Dictionary<PlayerCharacter, bool> playerHalls = new Dictionary<PlayerCharacter, bool>();

    protected override void Start()
    {
        leftHallGood = MathCC.RandomBool();
        base.Start();
    }

    public void PickLeftHall(int buttonID)
    {
        playerHalls.Add(currentCharacter, true);
        MultiplayerSelection(buttonID);
        CheckEndEvent();
    }

    public void PickRightHall(int buttonID)
    {
        playerHalls.Add(currentCharacter, false);
        MultiplayerSelection(buttonID);
        CheckEndEvent();
    }

    protected override void CheckEndEvent()
    {
        currentTurns++;

        if (currentTurns > 3)
        {
            if (leftHallGood)
            {
                PlayerVesselManager.Instance.PlayerVessels[nodeHandler.Player].MyCharacter.AddAugmentStack(augmentsToAdd[0], 2);
                myButtons[0].GetComponentInChildren<TextMeshProUGUI>().text = goodText;
                myButtons[1].GetComponentInChildren<TextMeshProUGUI>().text = goodText;
                myButtons[2].GetComponentInChildren<TextMeshProUGUI>().text = badText;
                myButtons[3].GetComponentInChildren<TextMeshProUGUI>().text = badText;

                for (int i = 0; i < playerHalls.Count; i++)
                    if (playerHalls[PlayerVesselManager.Instance.PlayerVessels[i].MyCharacter])
                        PlayerVesselManager.Instance.PlayerVessels[i].MyCharacter.AddAugmentStack(augmentsToAdd[0]);
            }
            else
            {
                PlayerVesselManager.Instance.PlayerVessels[nodeHandler.Player].MyCharacter.RemoveAmountOfAugments(2, 0);
                myButtons[0].GetComponentInChildren<TextMeshProUGUI>().text = badText;
                myButtons[1].GetComponentInChildren<TextMeshProUGUI>().text = badText;
                myButtons[2].GetComponentInChildren<TextMeshProUGUI>().text = goodText;
                myButtons[3].GetComponentInChildren<TextMeshProUGUI>().text = goodText;

                for(int i = 0; i < playerHalls.Count; i++)
                    if (!playerHalls[PlayerVesselManager.Instance.PlayerVessels[i].MyCharacter])
                        PlayerVesselManager.Instance.PlayerVessels[i].MyCharacter.AddAugmentStack(augmentsToAdd[0]);
            }

            StartCoroutine(SelectionChosen());
        }
    }
}
