using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadRushBigSuccess : MonoBehaviour
{
    [SerializeField] GameObject gateToDisable;
    [SerializeField] GameObject gateToEnable;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //disable enemies and small success triggers
        if(collision.gameObject.tag == "Player")
        {
            gameObject.SetActive(false);
            gateToDisable.SetActive(false);
            gateToEnable.SetActive(true);
            //score += 3;
        }
    }
}
