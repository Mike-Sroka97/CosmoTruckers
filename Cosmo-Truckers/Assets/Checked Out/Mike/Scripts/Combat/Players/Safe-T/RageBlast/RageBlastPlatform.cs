using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageBlastPlatform : MonoBehaviour
{
    [SerializeField] Color[] disablingColors;
    [SerializeField] float timeBeforeDisable;
    [SerializeField] float timeToStayDisabled;
    [SerializeField] int numberOfBlinks = 10;

    RageBlast minigame;
    Collider2D myCollider;
    SpriteRenderer myRenderer;
    Color startingColor;
    
    bool disableMe;
    float timeBetweenEachBlink = 0.1f;
    float disableTimer; 

    private void Start()
    {
        minigame = FindObjectOfType<RageBlast>();
        myCollider = GetComponent<Collider2D>();
        myRenderer = GetComponent<SpriteRenderer>();
        startingColor = myRenderer.color;
        timeBetweenEachBlink = timeBeforeDisable / numberOfBlinks; 
    }

    public void DisableMe()
    {
        disableMe = true;
        myRenderer.color = disablingColors[0]; 
        StartCoroutine(EnableMe());
    }

    public void Update()
    {
        if (disableMe)
        {
            disableTimer += Time.deltaTime; 

            // Set it to one of the disabling colors
            if (disableTimer > timeBetweenEachBlink)
            {
                myRenderer.color = myRenderer.color == disablingColors[0] ? disablingColors[1] : disablingColors[0];
                disableTimer = 0; 
            }
        }       
    }

    private IEnumerator EnableMe()
    {
        yield return new WaitForSeconds(timeBeforeDisable);
        myCollider.enabled = false;
        myRenderer.enabled = false;
        disableMe = false; 

        yield return new WaitForSeconds(timeToStayDisabled);
        myRenderer.enabled = true;
        myCollider.enabled = true;
        myRenderer.color = startingColor;

        minigame.NextPlatform();
    }
}
