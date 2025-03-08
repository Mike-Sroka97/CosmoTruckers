using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QDpufferFish : MonoBehaviour
{
    [SerializeField] float rotationSpeed;
    [SerializeField] float movementSpeed;
    [SerializeField] GameObject deathParticle; 

    Transform target;
    GameObject gust;
    SpriteRenderer myRenderer;
    Collider2D myCollider; 

    private void Start()
    {
        gust = transform.Find("Gust").gameObject;
        myRenderer = GetComponent<SpriteRenderer>();
        myCollider = GetComponent<Collider2D>();
        target = FindObjectOfType<SixfaceINA>().transform;
    }

    private void Update()
    {
        MoveTowardsPlayer();
    }

    private void MoveTowardsPlayer()
    {
        //handles rotation
        Vector3 direction = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

        //handles movement
        transform.Translate(Vector3.up * movementSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerAttack"))
        {
            myCollider.enabled = false;
            Instantiate(deathParticle, transform.position, Quaternion.identity, FindObjectOfType<CombatMove>().gameObject.transform);
            myRenderer.enabled = false;
            transform.eulerAngles = Vector3.zero;
            gust.SetActive(true);
            enabled = false;

        }
    }
}
