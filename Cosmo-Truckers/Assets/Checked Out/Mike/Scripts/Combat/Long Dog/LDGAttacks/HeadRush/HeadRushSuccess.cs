using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadRushSuccess : MonoBehaviour
{
    [SerializeField] GameObject gateToDisable;
    [SerializeField] GameObject gateToEnable;
    [SerializeField] int successToGive = 3;
    [SerializeField] bool lowerSuccessRate = false;

    HeadRush myMinigame;
    LongDogINA dog;

    private void Start()
    {
        myMinigame = GetComponentInParent<HeadRush>();
        dog = FindObjectOfType<LongDogINA>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //disable enemies and small success triggers
        if(collision.gameObject.tag == "Player" && !lowerSuccessRate)
        {
            gameObject.SetActive(false);
            gateToDisable.SetActive(false);
            gateToEnable.SetActive(true);
            dog.StretchingCollision(collision.tag);
            myMinigame.SuccessRate += successToGive;
            //myMinigame.DetermineLayout();
        }
        else if(collision.gameObject.tag == "Player")
        {
            myMinigame.SuccessRate += successToGive;
            Destroy(gameObject);
        }

        Debug.Log(myMinigame.SuccessRate);
    }
}
