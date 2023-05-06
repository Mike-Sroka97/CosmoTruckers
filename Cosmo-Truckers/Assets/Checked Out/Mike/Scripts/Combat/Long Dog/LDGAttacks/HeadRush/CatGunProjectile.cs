using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatGunProjectile : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    private void Update()
    {
        transform.position += new Vector3(moveSpeed * Time.deltaTime, 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
