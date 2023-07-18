using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeIronTrigger : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] Transform barrel;

    LargeIronClock clock;
    Collider2D myCollider;

    private void Start()
    {
        clock = FindObjectOfType<LargeIronClock>();
        myCollider = GetComponent<Collider2D>();

        //We have to do this to prevent the colliders from not working when the player does not move
        FindObjectOfType<PlayerBody>().transform.position -= new Vector3(-.01f, 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerAttack")
        {
            clock.Fire();
            myCollider.enabled = false;
            GameObject bulletTemp = Instantiate(bullet, barrel);
            bulletTemp.transform.parent = null;
            bulletTemp.transform.localScale = new Vector3(1, 1, 1);
            bulletTemp.transform.position = barrel.position;
        }
    }
}
