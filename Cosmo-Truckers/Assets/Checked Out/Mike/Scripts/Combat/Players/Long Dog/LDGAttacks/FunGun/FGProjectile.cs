using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FGProjectile : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    private void Update()
    {
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "LDGNoInteraction")
        {
            Destroy(collision.gameObject);
            FindObjectOfType<FunGun>().Score++;
            Debug.Log(FindObjectOfType<FunGun>().Score);
        }
        else if(collision.tag != "PlayerNonHurtable")
        {
            Destroy(gameObject);
        }
    }
}
