using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadRushResetVolume : MonoBehaviour
{
    [SerializeField] GameObject gate;
    [SerializeField] GameObject bigSuccess;

    HeadRush myMinigame;

    private void Start()
    {
        myMinigame = transform.parent.GetComponentInParent<HeadRush>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !collision.GetComponent<LongDogNeck>())
        {
            gate.SetActive(true);
            bigSuccess.SetActive(true);
            transform.parent.gameObject.SetActive(false);
            FindObjectOfType<LongDogINA>().StretchingCollision(collision.tag);
            //generate new layout from prefab list

            myMinigame.DetermineLayout();
        }
    }
}
