using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    const int xClamp = 10;
    const int yClamp = 8;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Destroy(transform.parent.gameObject);
        }
    }

    private void Update()
    {
        MoveMe();
    }

    private void MoveMe()
    {
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

        if(transform.position.y > yClamp 
            || transform.position.y < -yClamp 
            || transform.position.x > xClamp 
            || transform.position.x < -xClamp)
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
