using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_Noise : Augment
{
    [SerializeField] GameObject augment;

    GameObject tempAugment;

    private void Start()
    {
        tempAugment = Instantiate(augment);
        Activate();
    }

    public override void Activate(DebuffStackSO stack = null)
    {
        base.Activate(stack);
        tempAugment.GetComponent<SpriteRenderer>().material.SetFloat("_MainValue", StatusEffect);
    }

    public override void StopEffect()
    {
        Destroy(tempAugment);
    }
}
