using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPPsuccess : MonoBehaviour
{
    [SerializeField] float deltaDistance;
    [SerializeField] float moveSpeed;

    SugarPillPlacebo minigame;
    bool movingUp = true;
    Vector3 startingPosition;

    private void Start()
    {
        minigame = FindObjectOfType<SugarPillPlacebo>();
        startingPosition = transform.position;
    }

    private void Update()
    {
        Bob();
    }

    private void Bob()
    {
        if(movingUp)
        {
            transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);
            if(transform.position.y > startingPosition.y + deltaDistance)
            {
                movingUp = !movingUp;
            }
        }
        else
        {
            transform.position -= new Vector3(0, moveSpeed * Time.deltaTime, 0);
            if (transform.position.y < startingPosition.y - deltaDistance)
            {
                movingUp = !movingUp;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            minigame.Score++;
            Debug.Log(minigame.Score);
            Destroy(gameObject);
        }
    }
}
