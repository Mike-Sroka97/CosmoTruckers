using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QDspawnPF : MonoBehaviour
{
    [SerializeField] GameObject myEnemy;

    private void Start()
    {
        myEnemy.SetActive(false); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            myEnemy.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
