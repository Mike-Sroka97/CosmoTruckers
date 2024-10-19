using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrismaticBridge : EventNodeBase
{
    [SerializeField] int healingAmount = 20;
    [SerializeField] int maxHealthReduction = -10;

    public void Red(int buttonID)
    {
        currentCharacter.AdjustMaxHealth(maxHealthReduction);
        MultiplayerSelection(buttonID);
        CheckEndEvent();
    }

    public void Yellow(int buttonID)
    {
        currentCharacter.GetManaBase.SetMaxMana();
        MultiplayerSelection(buttonID);
        CheckEndEvent();
    }

    public void Green(int buttonID)
    {
        currentCharacter.TakeHealing(healingAmount);
        MultiplayerSelection(buttonID);
        CheckEndEvent();
    }

    public void Blue(int buttonID)
    {
        currentCharacter.RemoveAmountOfAugments(999, 2);
        MultiplayerSelection(buttonID);
        CheckEndEvent();
    }
}
