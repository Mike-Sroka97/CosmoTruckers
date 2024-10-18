using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CosmicCurse : EventNodeBase
{
    [SerializeField] List<AugmentStackSO> buffs;
    [SerializeField] List<AugmentStackSO> debuffs;

    protected override void Start()
    {
        base.Start();

        MathHelpers.Shuffle(buffs);
        MathHelpers.Shuffle(debuffs);

        for(int i = 0; i < 3; i++)
            myButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = $"<color=green>Gain (1) {buffs[i].DebuffName}</color><color=red> Gain (1) {debuffs[i].DebuffName}";
    }

    public void PickCurse(int buttonIndex)
    {
        currentCharacter.AddAugmentStack(buffs[buttonIndex]);
        currentCharacter.AddAugmentStack(debuffs[buttonIndex]);

        MultiplayerSelection(buttonIndex);
        CheckEndEvent();
    }
}
