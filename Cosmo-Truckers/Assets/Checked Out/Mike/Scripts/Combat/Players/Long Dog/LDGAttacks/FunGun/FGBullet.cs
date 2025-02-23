using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class FGBullet : MonoBehaviour
{
    [SerializeField] Vector2 forceToApply;
    [SerializeField] Vector3 noseOffsetPosition = new Vector3(-0.25f, 0f, 0f);
    [SerializeField] GameObject destroyParticle;

    bool attached = false;
    LongDogINA player; 
    CombatMove minigame;
    Transform nose;
    Collider2D myCollider;
    Rigidbody2D myRigidBody;

    float moveToNoseTime = 0.25f; 
    float moveToNoseTimer = 0f; 

    private void Start()
    {
        minigame = FindObjectOfType<CombatMove>();
        player = FindObjectOfType<LongDogINA>();
        nose = FindObjectOfType<LongDogNose>().transform;
        myCollider = GetComponent<Collider2D>();
        myRigidBody = GetComponent<Rigidbody2D>();

        player.FlipEvent.AddListener(ShootBullet);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerAttack" && player.GetStretching() && !attached)
        {
            Debug.Log("Attacked!"); 
            attached = true;
            StartCoroutine(MoveToNose());
        }
        else if (collision.tag == "EnemyDamaging" )
        {
            Instantiate(destroyParticle, transform.position, Quaternion.identity, minigame.transform);
            Destroy(gameObject); 
        }
    }

    /// <summary>
    /// Move the bullet to the nose
    /// </summary>
    /// <returns></returns>
    private IEnumerator MoveToNose()
    {
        myCollider.enabled = false;
        myRigidBody.isKinematic = true; 
        myRigidBody.velocity = Vector3.zero;
        transform.parent = nose;

        while (moveToNoseTimer < moveToNoseTime)
        {
            moveToNoseTimer += Time.deltaTime;
            Vector3 endPosition = nose.position + noseOffsetPosition;
            Vector3 newPosition = Vector3.Lerp(transform.position, endPosition, moveToNoseTime);
            transform.position = newPosition;
            yield return null; 
        }

        transform.position = nose.position + noseOffsetPosition; 
        moveToNoseTimer = 0f;
    }

    /// <summary>
    /// Straight hoopin'! 
    /// </summary>
    private void ShootBullet()
    {
        if (attached)
        {
            transform.parent = minigame.transform; 

            // Re-enable the collider and rigid body
            myCollider.enabled = true;
            myRigidBody.isKinematic = false;

            // Launch it left if facing left
            Vector3 force = forceToApply;
            if (player.transform.GetChild(0).transform.rotation.y == 0)
                force = new Vector3(forceToApply.x * -1, forceToApply.y, 0f); 

            // Apply force to the bullet
            myRigidBody.AddForce(force, ForceMode2D.Impulse);
            FindObjectOfType<FGBulletSpawner>().SpawnBullet();

            attached = false; 
        }
    }

    private void OnDestroy()
    {
        player.FlipEvent.RemoveListener(ShootBullet);
    }
}
