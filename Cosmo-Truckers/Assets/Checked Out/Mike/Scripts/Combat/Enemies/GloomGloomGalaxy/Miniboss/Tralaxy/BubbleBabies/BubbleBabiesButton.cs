using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleBabiesButton : MonoBehaviour
{
    [SerializeField] bool left = false;
    [SerializeField] BubbleBabiesNeedle myNeedle;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerAttack")
        {
            myNeedle.MoveMe(left);
        }
    }
}
