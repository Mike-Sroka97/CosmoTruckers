using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    [SerializeField] bool destroyParent = true;
    [SerializeField] bool destroyOnContact = true;

    public float MoveSpeed;

    const int xClamp = 10;
    const int yClamp = 8;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && destroyOnContact)
        {
            if(destroyParent)
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void Update()
    {
        MoveMe();
    }

    private void MoveMe()
    {
        transform.Translate(Vector3.right * MoveSpeed * Time.deltaTime);

        if(transform.position.y > yClamp 
            || transform.position.y < -yClamp 
            || transform.position.x > xClamp 
            || transform.position.x < -xClamp)
        {
            if(destroyParent)
                Destroy(transform.parent.gameObject);
            Destroy(gameObject);
        }
    }
}
