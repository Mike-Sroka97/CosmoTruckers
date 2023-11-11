using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSFanColorHandler : MonoBehaviour
{
    [SerializeField] float deactivatedAlpha;
    [SerializeField] SpriteRenderer myRenderer;
    Animator myAnimator;

    GeneralUseFan fan;
    Color startingColorMyRenderer;

    private void Awake()
    {
        startingColorMyRenderer = myRenderer.color;
        fan = GetComponent<GeneralUseFan>();
        myAnimator = myRenderer.GetComponent<Animator>();
    }

    public void ActivateColor()
    {
        transform.tag = "EnemyDamaging";
        myRenderer.color = startingColorMyRenderer;
        fan.enabled = true;
        fan.GetComponent<Collider2D>().enabled = true;
        myAnimator.speed = 1;
    }

    public void DeactivateColor()
    {
        transform.tag = "Untagged";
        myRenderer.color = new Color(myRenderer.color.r, myRenderer.color.g, myRenderer.color.b, deactivatedAlpha);
        fan.enabled = false;
        fan.GetComponent<Collider2D>().enabled = false;
        myAnimator.speed = 0;
    }
}
