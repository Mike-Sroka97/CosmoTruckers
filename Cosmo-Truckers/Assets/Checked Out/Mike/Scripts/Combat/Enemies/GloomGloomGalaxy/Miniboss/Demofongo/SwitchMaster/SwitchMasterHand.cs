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

    SpriteRenderer myRenderer;
    SwitchMaster minigame;
    int currentValue;

    private void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        minigame = FindObjectOfType<SwitchMaster>();
    }

    public IEnumerator Grab()
    {
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

        if(currentValue == masterItem.CurrentValue)
        {
            minigame.Score++;
            demofongo.color = new Color(0, 1, 0, 1);
        }
        else
        {
            demofongo.color = new Color(1, 0, 0, 1);
        }

        if(minigame.CurrentNumberOfCycles < minigame.MaxNumberOfCycles)
        {
            masterItem.ActivateMe();
        }
        else
        {
            minigame.EndMove();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<SwitchMasterItem>())
        {
            collision.GetComponent<SpriteRenderer>().enabled = false;
            currentValue = collision.GetComponent<SwitchMasterItem>().CurrentValue;
            Debug.Log("I caught a " + currentValue);
        }
    }
}
