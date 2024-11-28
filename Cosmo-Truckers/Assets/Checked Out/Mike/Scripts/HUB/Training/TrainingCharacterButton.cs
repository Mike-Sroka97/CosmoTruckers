using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainingCharacterButton : Button
{
    InaPractice ina;

    protected override void Start()
    {
        ina = transform.parent.parent.parent.GetComponent<InaPractice>();
    }

    public void SetTrainingCharacter(int traineeIndex)
    {
        ina.SetPlayer(traineeIndex);
    }
}
