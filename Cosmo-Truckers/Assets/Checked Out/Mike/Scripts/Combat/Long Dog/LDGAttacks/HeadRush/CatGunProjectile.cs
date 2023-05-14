using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatGunProjectile : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    private void Update()
    {
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
