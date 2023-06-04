using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongDogNose : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "PlayerHurtable" && collision.tag != "PlayerAttack")
        {
            GetComponentInParent<LongDogINA>().StretchingCollision(collision.tag);
        }
    }
}
