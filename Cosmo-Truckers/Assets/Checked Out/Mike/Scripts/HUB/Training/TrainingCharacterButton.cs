using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TrainingCharacterButton : Button, ISelectHandler
{
    InaPractice ina;

    protected override void Start()
    {
        ina = transform.parent.parent.parent.GetComponent<InaPractice>();
    }

    public void SetTrainingCharacter(int traineeIndex)
    {
        ina.SetPlayer(traineeIndex, GetComponent<TrainingButtonInfo>().CharacterName);
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        ina.TypeCharacterName(GetComponent<TrainingButtonInfo>().CharacterName);
    }
}
