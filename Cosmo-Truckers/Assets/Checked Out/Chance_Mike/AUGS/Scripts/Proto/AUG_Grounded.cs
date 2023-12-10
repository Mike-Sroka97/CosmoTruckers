using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_Grounded : Augment
{
    public override void Activate(DebuffStackSO stack = null)
    {
        base.Activate(stack);

        //just in case someone else gets this for some reason
        if(DebuffSO.MyCharacter.GetComponent<ProtoCharacter>())
            DebuffSO.MyCharacter.AddDebuffStack(DebuffSO.MyCharacter.GetComponent<ProtoCharacter>().Megawatt, (int)StatusEffect);
    }

    public override void StopEffect()
    {
        //doesn't need to do anything
    }
}
