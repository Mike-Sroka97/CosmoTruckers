using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMasterHand : MonoBehaviour
{
    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;
    [SerializeField] float lurchSpeed;
    [SerializeField] float retractSpeed;
    [SerializeField] Sprite openHand;
    [SerializeField] Sprite closedHand;
    [SerializeField] SwitchMasterItem masterItem;
    [SerializeField] SpriteRenderer demofongo;
    [SerializeField] Sprite demDefaultSprite;
    [SerializeField] Sprite demSuccessSprite;
    [SerializeField] Sprite demFailureSprite;
    [SerializeField] float defaultReturnTime = 0.5f; 

    SpriteRenderer myRenderer;
    SwitchMaster minigame;
    int currentValue;
    public bool CurrentlyGrabbing { get; private set;}  

    private void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        minigame = FindObjectOfType<SwitchMaster>();
    }

    public IEnumerator Grab()
    {
        CurrentlyGrabbing = true; 

        currentValue = -1;

        while(transform.position != endPoint.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPoint.position, lurchSpeed * Time.deltaTime);
            yield return null;
        }

        myRenderer.sprite = closedHand;

        while(transform.position != startPoint.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPoint.position, lurchSpeed * Time.deltaTime);
            yield return null;
        }

        myRenderer.sprite = openHand;
        minigame.CurrentNumberOfCycles++;
        CurrentlyGrabbing = false; 

        if(currentValue == masterItem.CurrentValue)
        {
            //Success
            StartCoroutine(ChangeDemofongoSprite(true)); 
            masterItem.IncrementToNextRenderer(true); 
        }
        else
        {
            //failure
            minigame.Score++;
            StartCoroutine(ChangeDemofongoSprite(false));
            masterItem.IncrementToNextRenderer(false);
        }

        if(minigame.CurrentNumberOfCycles < minigame.MaxNumberOfCycles)
        {
            masterItem.ActivateMe();
        }
    }

    private IEnumerator ChangeDemofongoSprite(bool succeeded)
    {
        if (succeeded)
        {
            demofongo.sprite = demSuccessSprite;
            demofongo.color = new Color(0, 1, 0, 1);
        }
        else
        {
            demofongo.sprite = demFailureSprite;
            demofongo.color = new Color(1, 0, 0, 1);
        }

        yield return new WaitForSeconds(defaultReturnTime);

        demofongo.sprite = demDefaultSprite;
        demofongo.color = new Color(1, 1, 1, 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<SwitchMasterItem>())
        {
            collision.GetComponent<SpriteRenderer>().enabled = false;
            currentValue = collision.GetComponent<SwitchMasterItem>().CurrentValue;
        }
    }
}
