using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleBabiesButton : MonoBehaviour
{
    [SerializeField] bool left = false;
    [SerializeField] BubbleBabiesNeedle myNeedle;
    [SerializeField] float waitBetweenHits = 0.5f;
    [SerializeField] Material defaultMaterial, toggledMaterial; 

    private bool canHit = true; 
    private float timer;

    private void Update()
    {
        if (!canHit)
        {
            timer += Time.deltaTime; 

            if (timer >= waitBetweenHits)
            {
                canHit = true;
                timer = 0f;
                GetComponent<SpriteRenderer>().material = defaultMaterial;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerAttack")
        {
            if (canHit)
            {
                myNeedle.MoveMe(left);
                canHit = false;
            }
        }
    }
}
