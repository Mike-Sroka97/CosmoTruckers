using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AS_AmberStar : MonoBehaviour
{
    [SerializeField] float minSpeed;
    [SerializeField] float maxSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] int damage = 3;

    Collider2D myCollider;
    float speed;

    private void Start()
    {
        speed = Random.Range(minSpeed, maxSpeed);
        Debug.Log("My Speed is: " + speed);
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.Rotate(new Vector3(0, 0, Time.deltaTime * rotationSpeed));
        transform.position += new Vector3(0, -(Time.deltaTime * speed), 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerNonHurtable"))
        {
            Destroy(gameObject);
        }
    }

    public int GetDamage() { return damage; }
}
