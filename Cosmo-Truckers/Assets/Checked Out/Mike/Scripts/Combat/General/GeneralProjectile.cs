using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralProjectile : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] bool destroyOnPlayer = false;

    const int xBound = 14;
    const int yBound = 12;
    const float gracePeriod = 0.05f;

    private void Update()
    {
        MoveMe();
        BoundsCheck();
    }

    private void MoveMe()
    {
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
    }

    private void BoundsCheck()
    {
        if(transform.position.x > xBound || transform.position.x < -xBound || transform.position.y > yBound || transform.position.y < -yBound)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && destroyOnPlayer)
        {
            StartCoroutine(DestroyProjectile()); 
        }
    }

    IEnumerator DestroyProjectile()
    {
        yield return new WaitForSeconds(gracePeriod);
        Destroy(gameObject);
    }
}
