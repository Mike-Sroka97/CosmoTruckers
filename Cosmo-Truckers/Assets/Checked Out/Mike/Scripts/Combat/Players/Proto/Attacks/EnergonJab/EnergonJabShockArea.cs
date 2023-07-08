using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergonJabShockArea : MonoBehaviour
{
    [SerializeField] float activeDuration;
    [SerializeField] float activationDelay;
    [SerializeField] float startupAlpha;

    Collider2D[] myColliders;
    SpriteRenderer[] myRenderers;

    private void Start()
    {
        myColliders = GetComponentsInChildren<Collider2D>();
        myRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    public void ActivateMe()
    {
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        //Semi-Active State
        foreach(SpriteRenderer sprite in myRenderers)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, startupAlpha);
        }

        yield return new WaitForSeconds(activationDelay);

        //Active State
        foreach (SpriteRenderer sprite in myRenderers)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1);
        }
        foreach(Collider2D collider in myColliders)
        {
            collider.enabled = true;
        }

        yield return new WaitForSeconds(activeDuration);

        //Deactive State
        foreach (SpriteRenderer sprite in myRenderers)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0);
        }
        foreach (Collider2D collider in myColliders)
        {
            collider.enabled = false;
        }
    }
}
