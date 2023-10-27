using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSBullet : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float lifeTime;

    private void Start()
    {
        Invoke("DestroyMe", lifeTime);
    }

    private void DestroyMe()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        transform.position -= transform.up * moveSpeed * Time.deltaTime;
    }
}
