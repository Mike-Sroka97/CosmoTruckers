using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MajorVigor : Augment
{
    [SerializeField] bool vigorIncrease = true;
    const int majorBaseStatusEffect = 50;
    const int majorAdditionalStatusEffect = 10;

    private void Start()
    {
        baseStatusEffect = majorBaseStatusEffect;
        additionalStatusEffect = majorAdditionalStatusEffect;

        if(!vigorIncrease)
        {
            baseStatusEffect = -baseStatusEffect;
            additionalStatusEffect = -additionalStatusEffect;
        }
    }
    public override void StopEffect()
    {
        DebuffSO.MyCharacter.AdjustDefense(-(int)StatusEffect);
    }
}
