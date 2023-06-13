using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageBlastPlatform : MonoBehaviour
{
    [HideInInspector] bool Disabled = false;
    [SerializeField] Color disablingColor;
    [SerializeField] float timeBeforeDisable;
    [SerializeField] float timeToStayDisabled;

    RageBlast minigame;
    Collider2D myCollider;
    SpriteRenderer myRenderer;
    Color startingColor;

    private void Start()
    {
        minigame = FindObjectOfType<RageBlast>();
        myCollider = GetComponent<Collider2D>();
        myRenderer = GetComponent<SpriteRenderer>();
        startingColor = myRenderer.color;
    }

    public void DisableMe()
    {
        myRenderer.color = disablingColor;
        StartCoroutine(EnableMe());
    }

    private IEnumerator EnableMe()
    {
        yield return new WaitForSeconds(timeBeforeDisable);
        myCollider.enabled = false;
        myRenderer.enabled = false;

        yield return new WaitForSeconds(timeToStayDisabled);
        myRenderer.enabled = true;
        myCollider.enabled = true;
        myRenderer.color = startingColor;

        minigame.NextPlatform();
    }
}
