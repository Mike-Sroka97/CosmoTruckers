using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeggieVengeanceProjectile : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    private void Update()
    {
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "TopKiller" || collision.name == "RightKiller" || collision.name == "BottomKiller" || collision.GetComponent<VeggieVengeanceVeggie>())
        {
            Destroy(gameObject);
        }
    }
}
