using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cole_FallingStar : MonoBehaviour
{
    public SpriteRenderer starSR;
    public GameObject explosionPrefab;
    public float timeToDestroy;
    private float timer;
    private bool destroy;

    // Update is called once per frame
    void Update()
    {
        if (destroy)
        {
            timer += Time.deltaTime; 

            if (timer > timeToDestroy)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        gameObject.GetComponent<Collider2D>().enabled = false;
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f; 
        starSR.enabled = false;
        Instantiate(explosionPrefab, transform.position, explosionPrefab.transform.rotation);

        destroy = true; 
    }
}
