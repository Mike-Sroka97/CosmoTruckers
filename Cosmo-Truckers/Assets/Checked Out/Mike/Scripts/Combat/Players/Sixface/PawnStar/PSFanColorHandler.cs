using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSFanColorHandler : MonoBehaviour
{
    [SerializeField] float deactivatedAlpha;
    [SerializeField] SpriteRenderer myRenderer;
    Animator myAnimator;
    ParticleUpdater particleUpdater;

    GeneralUseFan fan;
    Color startingColorMyRenderer;

    private void Awake()
    {
        startingColorMyRenderer = myRenderer.color;
        fan = GetComponent<GeneralUseFan>();
        myAnimator = myRenderer.GetComponent<Animator>();
        particleUpdater = GetComponent<ParticleUpdater>();
    }

    public void ActivateFan()
    {
        myRenderer.color = startingColorMyRenderer;
        fan.enabled = true;
        fan.GetComponent<Collider2D>().enabled = true;
        myAnimator.speed = 1;
        particleUpdater.SetParticleState(true);
    }

    public void DeactivateFan()
    {
        myRenderer.color = new Color(myRenderer.color.r, myRenderer.color.g, myRenderer.color.b, deactivatedAlpha);
        fan.enabled = false;
        fan.GetComponent<Collider2D>().enabled = false;
        myAnimator.speed = 0;
        particleUpdater.SetParticleState(false);
    }
}
