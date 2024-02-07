using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_Noise : Augment
{
    [SerializeField] GameObject augment;

    GameObject tempAugment;

    public override void Activate(DebuffStackSO stack = null)
    {
        base.Activate(stack);
        tempAugment = Instantiate(augment, FindObjectOfType<INAcombat>().transform);
        tempAugment.GetComponent<SpriteRenderer>().material.SetFloat("_MainValue", StatusEffect);
    }

    public override void StopEffect()
    {
        Destroy(tempAugment);
        Destroy(gameObject);
    }
}
