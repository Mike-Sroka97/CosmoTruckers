using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPddrActivated : MonoBehaviour
{
    [SerializeField] float twoPointDistance;
    [SerializeField] float onePointDistance;
    [SerializeField] bool trashZone = false;

    FunkyPersuasion miniGame;

    private void Start()
    {
        miniGame = FindObjectOfType<FunkyPersuasion>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(trashZone && collision.tag == "PlayerHurtable")
        {
            miniGame.Score--;
            Debug.Log(miniGame.Score);
            Destroy(collision.gameObject);
        }
        else if(collision.tag == "PlayerHurtable")
        {
            float distance = Vector2.Distance(collision.transform.position, transform.position);
            if(distance <= twoPointDistance)
            {
                miniGame.Score += 2;
                Debug.Log(miniGame.Score);
                Destroy(collision.gameObject);
            }
            else if(distance <= onePointDistance)
            {
                miniGame.Score += 1;
                Debug.Log(miniGame.Score);
                Destroy(collision.gameObject);
            }
        }
    }
}
