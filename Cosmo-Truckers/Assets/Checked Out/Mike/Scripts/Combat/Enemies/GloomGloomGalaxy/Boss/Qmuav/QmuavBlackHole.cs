
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QmuavBlackHole : MonoBehaviour
{
    [SerializeField] float rotateSpeed;
    [SerializeField] float shrinkSpeed;
    [SerializeField] float minScale;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && collision.GetComponentInParent<Player>())
        {
            collision.GetComponentInParent<Graviton>().IsAttractee = false;

            //TODO LONG DOG SPECIAL CASE FML
            if (collision.GetComponentInParent<LongDogINA>() != null)
                StartCoroutine(LDGSpecialCase(collision.GetComponentInParent<LongDogINA>()));

            else { StartCoroutine(ShrinkAndKill(collision.GetComponentInParent<Player>())); }    
        }

        if (collision.GetComponent<QmuavProjectile>())
            collision.GetComponent<QmuavProjectile>().ResetPosition();
    }

    IEnumerator ShrinkAndKill(Player player)
    {
        while (player.transform.localScale.x > minScale)
        {
            player.transform.Rotate(new Vector3(0, 0, rotateSpeed * Time.deltaTime));
            player.transform.localScale -= new Vector3(shrinkSpeed * Time.deltaTime, shrinkSpeed * Time.deltaTime, shrinkSpeed * Time.deltaTime);
            player.transform.position = transform.position;
            yield return null;
        }

        player.transform.localScale = new Vector3(minScale, minScale, minScale);
        foreach (SpriteRenderer spriteRenderer in player.MyRenderers)
            spriteRenderer.enabled = false;
    }

    IEnumerator LDGSpecialCase(LongDogINA dog)
    {
        dog.LDGSpecialDeath(); 
        
        // Shrink and kill Long Dog
        Transform player = dog.transform.GetChild(0);

        while (player.transform.localScale.x > minScale)
        {
            player.transform.Rotate(new Vector3(0, 0, rotateSpeed * Time.deltaTime));
            player.transform.localScale -= new Vector3(shrinkSpeed * Time.deltaTime, shrinkSpeed * Time.deltaTime, shrinkSpeed * Time.deltaTime);
            player.transform.position = transform.position;
            yield return null;
        }

        player.GetComponent<Rigidbody2D>().isKinematic = true;

        player.transform.localScale = new Vector3(minScale, minScale, minScale);
        foreach (SpriteRenderer spriteRenderer in dog.MyRenderers)
            spriteRenderer.enabled = false;
    }
}
