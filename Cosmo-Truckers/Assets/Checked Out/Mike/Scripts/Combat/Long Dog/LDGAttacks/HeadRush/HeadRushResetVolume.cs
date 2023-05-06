using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadRushResetVolume : MonoBehaviour
{
    [SerializeField] GameObject gate;
    [SerializeField] GameObject bigSuccess;
 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            gate.SetActive(true);
            bigSuccess.SetActive(true);
            transform.parent.gameObject.SetActive(false);
            FindObjectOfType<LongDogINA>().StretchingCollision(collision.tag);
            //generate new layout from prefab list
        }
    }
}
