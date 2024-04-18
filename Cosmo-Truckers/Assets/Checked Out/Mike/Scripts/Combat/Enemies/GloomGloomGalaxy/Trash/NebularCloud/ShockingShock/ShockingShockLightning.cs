using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockingShockLightning : MonoBehaviour
{
    public bool AlwaysActivate;
    [SerializeField] float shockDelay;
    [SerializeField] int numberOfFlashes;
    [SerializeField] float shockDuration;
    [SerializeField] GameObject shockedPuddle;
    [SerializeField] GameObject regularPuddle;
    [HideInInspector] public bool IsShocking;

    [SerializeField] SpriteRenderer myRendererer;
    [SerializeField] ParticleSystem lightningParticles; 
    Collider2D myColliders;
    Color inactiveColor;
    Color activeColor;
    Color semiActiveColor;
    ShockingShock minigame;

    private void Start()
    {
        minigame = FindObjectOfType<ShockingShock>();
        //myRendererer = gameObject.GetComponentInChildren<SpriteRenderer>();
        activeColor = myRendererer.color;
        semiActiveColor = new Color(activeColor.r, activeColor.g, activeColor.b, activeColor.a / 8);
        inactiveColor = new Color(myRendererer.color.r, myRendererer.color.g, myRendererer.color.b, 0);
        myRendererer.color = inactiveColor;

        myColliders = GetComponent<Collider2D>();
        myColliders.enabled = false;
    }

    public void ActivateMe()
    {
        StartCoroutine(Shock());
    }

    IEnumerator Shock()
    {
        int currentNumberOfFlashes = 0;

        minigame.CurrentActivatedLightning++;
        while(currentNumberOfFlashes < numberOfFlashes)
        {
            myRendererer.color = semiActiveColor;
            yield return new WaitForSeconds(shockDelay);
            myRendererer.color = inactiveColor;
            yield return new WaitForSeconds(shockDelay);
            currentNumberOfFlashes++;
        }

        myRendererer.color = activeColor;
        myColliders.enabled = true;
        shockedPuddle.SetActive(true);
        regularPuddle.SetActive(false);
        lightningParticles.Play(); 

        yield return new WaitForSeconds(shockDuration);

        myRendererer.color = inactiveColor;
        IsShocking = false;
        minigame.CurrentActivatedLightning--;
        myColliders.enabled = false;
        regularPuddle.SetActive(true);
        shockedPuddle.SetActive(false);

    }
}
