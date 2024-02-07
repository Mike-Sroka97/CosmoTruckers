using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_BullsEye : Augment
{
    public override void Activate(DebuffStackSO stack = null)
    {
        foreach(Enemy enemy in FindObjectsOfType<Enemy>())
        {
            if(stack.MyCharacter.GetComponent<PlayerCharacter>())
            {
                enemy.TauntedBy = stack.MyCharacter.GetComponent<PlayerCharacter>();
            }
        }
    }

    public override void StopEffect()
    {
        foreach (Enemy enemy in FindObjectsOfType<Enemy>())
        {
            enemy.TauntedBy = null;
        }
    }
}
