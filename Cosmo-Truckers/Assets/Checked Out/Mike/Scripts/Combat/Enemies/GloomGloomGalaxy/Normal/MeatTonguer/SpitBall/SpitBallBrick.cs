using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitBallBrick : MonoBehaviour
{
    [SerializeField] GameObject lingeringSpit;
    [SerializeField] GameObject burstParticle;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.GetComponent<BallBounce>())
        {
            Vector3 collisionPoint = collision.ClosestPoint(transform.position);
            Instantiate(lingeringSpit, collisionPoint, Quaternion.identity, FindObjectOfType<SpitBall>().transform);
            Instantiate(burstParticle, collisionPoint, Quaternion.identity, FindObjectOfType<SpitBall>().transform);
            Destroy(gameObject);
        }
    }
}
