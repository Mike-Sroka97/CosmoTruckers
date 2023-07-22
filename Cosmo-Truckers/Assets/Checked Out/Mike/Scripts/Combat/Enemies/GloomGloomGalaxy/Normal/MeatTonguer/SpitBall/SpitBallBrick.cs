using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitBallBrick : MonoBehaviour
{
    [SerializeField] GameObject lingeringSpit;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.GetComponent<BallBounce>())
        {
            Vector3 collisionPoint = new Vector3(collision.GetContact(0).point.x, collision.GetContact(0).point.y, 0);
            Instantiate(lingeringSpit, collisionPoint, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
