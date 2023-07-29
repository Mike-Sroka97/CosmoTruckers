using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalCrustStalactite : MonoBehaviour
{
    [SerializeField] bool ground = false;

    Rigidbody2D parentBody;
    PolygonCollider2D parentCollider;

    private void Start()
    {
        if(!ground)
        {
            parentBody = GetComponentInParent<Rigidbody2D>();
            parentCollider = GetComponentInParent<PolygonCollider2D>();
            Invoke("EnableParentCollider", 0.5f);
        }
    }

    private void EnableParentCollider()
    {
        parentCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<OrbitalCrustStalactite>() && !ground)
        {
            parentBody.bodyType = RigidbodyType2D.Static;
            parentBody.velocity = Vector2.zero;
            Destroy(parentBody);
        }
    }
}
