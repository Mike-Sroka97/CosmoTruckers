using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LunarLuck : EventNodeBase
{
    [SerializeField] string goodText;
    [SerializeField] string badText;

    bool heads;
    Dictionary<PlayerCharacter, bool> playerPicks = new Dictionary<PlayerCharacter, bool>();

    protected override void Start()
    {
        heads = MathCC.RandomBool();
        base.Start();
    }

    public void PickHeads(int buttonID)
    {
        playerPicks.Add(currentCharacter, true);
        MultiplayerSelection(buttonID);
        CheckEndEvent();
    }

    public void PickTails(int buttonID)
    {
        playerPicks.Add(currentCharacter, false);
        MultiplayerSelection(buttonID);
        CheckEndEvent();
    }

    protected override void CheckEndEvent()
    {
        currentTurns++;

        if (currentTurns > 3)
        {
            if (heads)
            {
                myButtons[0].GetComponentInChildren<TextMeshProUGUI>().text = goodText;
                myButtons[1].GetComponentInChildren<TextMeshProUGUI>().text = goodText;
                myButtons[2].GetComponentInChildren<TextMeshProUGUI>().text = badText;
                myButtons[3].GetComponentInChildren<TextMeshProUGUI>().text = badText;

                for (int i = 0; i < playerPicks.Count; i++)
                {
                    if (playerPicks[PlayerVesselManager.Instance.PlayerVessels[i].MyCharacter])
                        PlayerVesselManager.Instance.PlayerVessels[i].MyCharacter.AddAugmentStack(augmentsToAdd[0], 3);
                    else
                        PlayerVesselManager.Instance.PlayerVessels[i].MyCharacter.AddAugmentStack(augmentsToAdd[1], 3);
                }

            }
            else
            {
                myButtons[0].GetComponentInChildren<TextMeshProUGUI>().text = badText;
                myButtons[1].GetComponentInChildren<TextMeshProUGUI>().text = badText;
                myButtons[2].GetComponentInChildren<TextMeshProUGUI>().text = goodText;
                myButtons[3].GetComponentInChildren<TextMeshProUGUI>().text = goodText;

                for (int i = 0; i < playerPicks.Count; i++)
                {
                    if (playerPicks[PlayerVesselManager.Instance.PlayerVessels[i].MyCharacter])
                        PlayerVesselManager.Instance.PlayerVessels[i].MyCharacter.AddAugmentStack(augmentsToAdd[1], 3);
                    else
                        PlayerVesselManager.Instance.PlayerVessels[i].MyCharacter.AddAugmentStack(augmentsToAdd[0], 3);
                }

            }

            StartCoroutine(SelectionChosen());
        }
    }
}
