using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameCharacterSelectButton : Button
{
    TrainingButtonInfo info;

    protected override void Awake()
    {
        info = GetComponent<TrainingButtonInfo>();
    }

    protected override void OnEnable()
    {
        if(info.CharacterButton)
        {
            FindObjectOfType<InaPractice>().SetPlayerMinigameButton(gameObject);
        }
    }
}
