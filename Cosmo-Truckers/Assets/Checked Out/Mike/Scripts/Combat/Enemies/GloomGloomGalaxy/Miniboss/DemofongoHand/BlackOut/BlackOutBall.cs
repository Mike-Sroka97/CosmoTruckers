using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackOutBall : MonoBehaviour
{
    [SerializeField] Material successMaterial;
    [SerializeField] Material damagingMaterial;
    [SerializeField] float successDuration;

    TrackPlayerDeath deathTracker;
    SpriteRenderer myRenderer;
    Collider2D myCollider;
    BlackOut minigame;
    BlackOutRotator myRotator;

    private void Start()
    {
        deathTracker = GetComponent<TrackPlayerDeath>();
        myRenderer = GetComponent<SpriteRenderer>();
        myCollider = GetComponent<Collider2D>();
        minigame = FindObjectOfType<BlackOut>();
        myRotator = GetComponentInParent<BlackOutRotator>();
    }

    public void ActivateMe(bool success)
    {
        if(success)
        {
            deathTracker.TrackingDamage = false;
            myRenderer.material = successMaterial;
            myCollider.enabled = false;
            StartCoroutine(ShowSuccess());
        }
        else
        {
            deathTracker.TrackingDamage = true;
            myRenderer.material = damagingMaterial;
            myCollider.enabled = true;
        }
    }

    IEnumerator ShowSuccess()
    {
        yield return new WaitForSeconds(successDuration);

        myRenderer.material = damagingMaterial;

        StartCoroutine(myRotator.RotateAlteration());

        while(myRotator.Rotating)
        {
            yield return null;
        }

        myCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!deathTracker.TrackingDamage)
        {
            minigame.Score++;
            Debug.Log(minigame.Score);
            myRenderer.material = successMaterial;
            myCollider.enabled = false;
        }
    }
}
