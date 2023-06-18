using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSBullet : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float lifeTime;

    SuperSlam minigame;

    private void Start()
    {
        minigame = FindObjectOfType<SuperSlam>();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            minigame.PlayerDead = true;
            Debug.Log(minigame.PlayerDead);
        }
    }
}
