using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualAugment : Augment
{
    [SerializeField] GameObject augment;
    [SerializeField] int maxVisualStacks = 0;

    GameObject tempAugment;

    public override void Activate(DebuffStackSO stack = null)
    {
        if (maxVisualStacks == 0)
            maxVisualStacks = stack.MaxStacks;

        base.Activate(stack);

        //handles augments that have a visual limit but a tertiary effect that goes beyond that stack limit
        if (maxVisualStacks > 0 && stack != null)
            MaxStatusEffect = stack.StackValue.x + stack.StackValue.y * (maxVisualStacks - 1);
        AdjustMaxStatusEffect();

        tempAugment = Instantiate(augment, FindObjectOfType<INAcombat>().transform);
        tempAugment.transform.position -= new Vector3(0, .5f);
        tempAugment.GetComponent<SpriteRenderer>().material.SetFloat("_MainValue", StatusEffect);
    }

    public override void StopEffect()
    {
        Destroy(tempAugment);
        Destroy(gameObject);
    }
}
