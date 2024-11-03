using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalCrustStalactite : MonoBehaviour
{
    [SerializeField] bool ground = false;
    [SerializeField] Material noHurtMaterial;
    [SerializeField] GameObject deathParticle;
    float groundLayer = 9;
    float minHeight = -1.3f; 

    Rigidbody2D parentBody;
    PolygonCollider2D parentCollider;
    TrackPlayerDeath playerDeath;
    SpriteRenderer sprite; 

    private void Start()
    {
        if(!ground)
        {
            parentBody = GetComponentInParent<Rigidbody2D>();
            parentCollider = GetComponentInParent<PolygonCollider2D>();
            playerDeath = GetComponent<TrackPlayerDeath>();
            sprite = transform.parent.GetComponentInChildren<SpriteRenderer>();
            Invoke("EnableParentCollider", 0.5f);
        }
    }

    private void EnableParentCollider()
    {
        parentCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!ground && transform.parent.localPosition.y < minHeight)
        {
            if (collision.GetComponent<OrbitalCrustStalactite>() || collision.gameObject.layer == groundLayer)
            {
                ground = true;
                GetComponent<Collider2D>().enabled = false;
                transform.parent.GetComponent<Collider2D>().enabled = true;
                Destroy(playerDeath); 
                sprite.material = noHurtMaterial;
                parentBody.bodyType = RigidbodyType2D.Static;
                parentBody.velocity = Vector2.zero;
                Destroy(parentBody);
            }
            if (collision.tag == "Player")
            {
                Instantiate(deathParticle, transform.position, Quaternion.identity, FindObjectOfType<CombatMove>().transform);
                Destroy(transform.parent.gameObject); 
            }
        }
    }
}
