using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToTongueOrNotToTongue : CombatMove
{
    [SerializeField] DebuffStackSO shloppedAug;

    Rotator myRotator;
    float rotatorSpeed;

    private void Start()
    {
        GenerateLayout();
        myRotator = GetComponentInChildren<Rotator>();
        rotatorSpeed = myRotator.RotateSpeed;
        myRotator.RotateSpeed = 0;
    }

    public override void StartMove()
    {
        ToTongueOrNotToTongueSpitter[] spitters = GetComponentsInChildren<ToTongueOrNotToTongueSpitter>();

        foreach (ToTongueOrNotToTongueSpitter spitter in spitters)
            spitter.enabled = true;

        myRotator.RotateSpeed = rotatorSpeed;
        int random = UnityEngine.Random.Range(0, 2);
        if (random == 1)
        {
            myRotator.RotateSpeed = -myRotator.RotateSpeed;
        }
        base.StartMove();
    }

    public override void EndMove()
    {
        base.EndMove();
        CombatManager.Instance.GetCharactersSelected[0].AddDebuffStack(shloppedAug, CalculateAugmentScore());
    }
}
