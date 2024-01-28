using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlorbnusAI : Enemy
{
    public override void Die()
    {
        FindObjectOfType<OrbnusAI>().ShredArmor();
        base.Die();
    }
}
