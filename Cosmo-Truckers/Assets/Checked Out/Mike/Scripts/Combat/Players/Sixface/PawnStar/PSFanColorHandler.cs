using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSFanColorHandler : MonoBehaviour
{
    [SerializeField] float deactivatedAlpha;

    SpriteRenderer myRenderer;
    SpriteRenderer myChildRenderer;
    Color startingColorMyRenderer;
    Color startingColorMyChildRenderer;


    private void Awake()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        myChildRenderer = GetComponentsInChildren<SpriteRenderer>()[1];
        startingColorMyRenderer = myRenderer.color;
        startingColorMyChildRenderer = myChildRenderer.color;
    }

    public void ActivateColor()
    {
        transform.tag = "EnemyDamaging";
        myRenderer.color = startingColorMyRenderer;
        myChildRenderer.color = startingColorMyChildRenderer;
        myChildRenderer.GetComponent<GeneralUseFan>().enabled = true;
        myChildRenderer.GetComponent<Collider2D>().enabled = true;
    }

    public void DeactivateColor()
    {
        transform.tag = "Untagged";
        myRenderer.color = new Color(myRenderer.color.r, myRenderer.color.g, myRenderer.color.b, deactivatedAlpha);
        myChildRenderer.color = new Color(myChildRenderer.color.r, myChildRenderer.color.g, myChildRenderer.color.b, 0);
        myChildRenderer.GetComponent<GeneralUseFan>().enabled = false;
        myChildRenderer.GetComponent<Collider2D>().enabled = false;
    }
}
