using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncrustableSuccessBlock : Block
{
    Encrustable minigame;

    private void Start()
    {
        minigame = FindObjectOfType<Encrustable>();
    }

    protected override void DestroyMe(Collider2D collision)
    {
        if(collision.CompareTag("PlayerAttack"))
        {
            minigame.AugmentScore--;
            minigame.CheckAugmentSuccessForEnemyAttack(); 
        }
        base.DestroyMe(collision);
    }
}
