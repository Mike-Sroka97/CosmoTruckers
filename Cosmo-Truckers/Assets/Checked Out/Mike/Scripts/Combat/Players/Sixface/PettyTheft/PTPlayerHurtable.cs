using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PTPlayerHurtable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack"))
        {
            GetComponentInParent<PettyTheftEnemy>().Hurt();
        }
    }
}
