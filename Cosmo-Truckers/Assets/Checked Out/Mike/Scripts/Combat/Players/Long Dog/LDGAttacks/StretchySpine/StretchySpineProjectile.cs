using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StretchySpineProjectile : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float deadForceBoost;
    [SerializeField] bool good;

    bool movingRight = true;
    Rigidbody2D myBody;
    CircleCollider2D myCollider;
    StretchySpine minigame;
    private void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<CircleCollider2D>();
        minigame = FindObjectOfType<StretchySpine>();
    }

    private void Update()
    {
        if(movingRight)
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
    }

    private void SpecialDestroy()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            movingRight = false;
            myBody.velocity = Vector2.zero;
            myBody.gravityScale = 1;
            myBody.AddForce(new Vector2(-deadForceBoost, deadForceBoost), ForceMode2D.Impulse);
            myCollider.enabled = false;
            gameObject.transform.GetChild(0).GetComponent<SimpleRotation>().enabled = true; 
            Invoke("SpecialDestroy", 2.5f);
        }
        else if(collision.tag == "LDGNoInteraction" && collision.gameObject.name != "SoftPlatform")
        {
            AdvancedFrameAnimation frameAnimation = collision.gameObject.GetComponent<AdvancedFrameAnimation>();

            if(good)
            {
                minigame.Score += 2;
                frameAnimation.SwitchToHappyAnimation(); 
            }
            else
            {
                minigame.Score -= 1;
                frameAnimation.SwitchToHurtAnimation(); 
            }
            Destroy(gameObject);
        }
    }
}
