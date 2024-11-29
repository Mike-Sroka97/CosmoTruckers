using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMasterItem : MonoBehaviour
{
    [SerializeField] bool masterItem = false;
    [SerializeField] SwitchMasterItem[] items;
    [SerializeField] Sprite[] differentItems;
    [SerializeField] float flickerDuration;
    [SerializeField] float totalFlickerDuration;
    [SerializeField] float itemDecisionDuration;
    [SerializeField] SpriteRenderer[] myItemRenderers;
    [SerializeField] SpriteRenderer[] myDiscRenderers;
    [SerializeField] Color neutralColor, negativeColor, positiveColor;

    public int CurrentValue;

    SpriteRenderer myRenderer;
    SwitchMasterHand hand;
    int currentRenderer = 0;
    int lastRandom = -1;
    int random;
    private bool canFlicker = true;

    private void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        hand = FindObjectOfType<SwitchMasterHand>();

        random = Random.Range(0, differentItems.Length);
        lastRandom = random;
        myRenderer.sprite = differentItems[random];

        if (masterItem)
            ItemSmoothing();
    }

    public void ActivateMe()
    {
        StartCoroutine(Flicker());

        foreach (SwitchMasterItem item in items)
        {
            item.GetComponent<SpriteRenderer>().enabled = true;
            StartCoroutine(item.Flicker());
        }
    }

    private void ItemSmoothing()
    {
        // Stop the flickering on the items
        foreach (SwitchMasterItem item in items)
        {
            item.StopFlicker(); 
        }

        int currentItem0 = 0;
        int currentItem1 = 0;
        int currentItem2 = 0;
        int currentItem3 = 0;


        // Count up what items we have
        foreach(SwitchMasterItem item in items)
        {
            if(item.GetComponent<SpriteRenderer>().sprite == differentItems[0])
            {
                currentItem0++;
                item.CurrentValue = 0; 
            }
            else if(item.GetComponent<SpriteRenderer>().sprite == differentItems[1])
            {
                currentItem1++;
                item.CurrentValue = 1;
            }
            else if(item.GetComponent<SpriteRenderer>().sprite == differentItems[2])
            {
                currentItem2++;
                item.CurrentValue = 2;
            }
            else
            {
                currentItem3++;
                item.CurrentValue = 3;
            }
        }

        int[] currentItems = new int[] { currentItem0, currentItem1, currentItem2, currentItem3 };

        // Smooth out the item count
        for (int i = 0; i < 4; i++)
        {
            if (currentItems[i] == 0)
            {
                // Get the highest number of items for one shape 
                int highestNumberItem = GreatestNumber(currentItems[0], currentItems[1], currentItems[2], currentItems[3]);
                
                // The highest number should never be 1
                if (currentItems[highestNumberItem] != 1)
                {
                    SetItemValues(highestNumberItem, i);
                    // Make sure to decrease the count of this item and increase the count of the other item
                    currentItems[highestNumberItem]--;
                    currentItems[i]++;
                }
                else
                {
                    Debug.Log("Current Item count is 1, but 1 is the highest item count!"); 
                }
            }
        }
    }

    private int GreatestNumber(int item0, int item1, int item2, int item3)
    {
        int[] greatestNumberArray = { item0, item1, item2, item3 };
        int highestNumber = Mathf.Max(greatestNumberArray);
        int numberToReturn = 0;

        if(highestNumber == item0)
        {
            numberToReturn = 0;
        }
        else if(highestNumber == item1)
        {
            numberToReturn = 1;
        }
        else if(highestNumber == item2)
        {
            numberToReturn = 2;
        }
        else
        {
            numberToReturn = 3;
        }
        return numberToReturn;
    }

    // Smooth out the item count by setting this item to a new item
    private void SetItemValues(int highestNumberItem, int newItemValue)
    {
        foreach (SwitchMasterItem item in items)
        {
            if (item.CurrentValue == highestNumberItem)
            {
                item.GetComponent<SpriteRenderer>().sprite = differentItems[newItemValue];
                item.CurrentValue = newItemValue;
                break; 
            }
        }
    }

    public void StopFlicker()
    {
        canFlicker = false; 
    }

    private IEnumerator Flicker()
    {
        if (masterItem)
        {
            myItemRenderers[currentRenderer].color = neutralColor;
            myDiscRenderers[currentRenderer].color = neutralColor;
        }

        float currentTime = 0;
        canFlicker = true; 

        while(currentTime < totalFlickerDuration && canFlicker)
        {
            while(lastRandom == random)
            {
                random = Random.Range(0, differentItems.Length);
            }

            //flicker
            lastRandom = random;

            if (masterItem)
            {
                myItemRenderers[currentRenderer].sprite = differentItems[random];
            }
            else
            {
                myRenderer.sprite = differentItems[random];
            }

            yield return new WaitForSeconds(flickerDuration);

            currentTime += Time.deltaTime + flickerDuration;

            yield return null;
        }

        if (masterItem)
        {
            CurrentValue = random;
            ItemSmoothing();

            yield return new WaitForSeconds(itemDecisionDuration);

            if (masterItem)
                StartCoroutine(hand.Grab());
        }
    }

    public void IncrementToNextRenderer(bool correctChoice)
    {
        if (correctChoice)
        {
            myItemRenderers[currentRenderer].color = positiveColor;
            myDiscRenderers[currentRenderer].color = positiveColor;
        }
        else
        {
            myItemRenderers[currentRenderer].color = negativeColor;
            myDiscRenderers[currentRenderer].color = negativeColor;
        }

        currentRenderer++; 
    }
}
