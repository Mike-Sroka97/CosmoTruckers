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

    [HideInInspector] public int CurrentValue;

    SpriteRenderer myRenderer;
    SwitchMasterHand hand;
    int lastRandom = -1;
    int random;

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
        foreach(SwitchMasterItem item in items)
        {
            item.GetComponent<SpriteRenderer>().enabled = true;
            StartCoroutine(item.Flicker());
        }

        StartCoroutine(Flicker());
    }

    private void ItemSmoothing()
    {
        int currentItem0 = 0;
        int currentItem1 = 0;
        int currentItem2 = 0;
        int currentItem3 = 0;

        //Count up what items we have
        foreach(SwitchMasterItem item in items)
        {
            if(item.GetComponent<SpriteRenderer>().sprite == differentItems[0])
            {
                currentItem0++;
            }
            else if(item.GetComponent<SpriteRenderer>().sprite == differentItems[1])
            {
                currentItem1++;
            }
            else if(item.GetComponent<SpriteRenderer>().sprite == differentItems[2])
            {
                currentItem2++;
            }
            else
            {
                currentItem3++;
            }
        }

        //Smooth out those items
        if(currentItem0 == 0)
        {
            int highestNumberItem = GreatestNumber(currentItem0, currentItem1, currentItem2, currentItem3);

            foreach (SwitchMasterItem item in items)
            {
                if(item.GetComponent<SpriteRenderer>().sprite == differentItems[highestNumberItem])
                {
                    item.GetComponent<SpriteRenderer>().sprite = differentItems[0];
                    item.CurrentValue = 0;
                }
            }
        }
        if(currentItem1 == 0)
        {
            int highestNumberItem = GreatestNumber(currentItem0, currentItem1, currentItem2, currentItem3);

            foreach (SwitchMasterItem item in items)
            {
                if (item.GetComponent<SpriteRenderer>().sprite == differentItems[highestNumberItem])
                {
                    item.GetComponent<SpriteRenderer>().sprite = differentItems[1];
                    item.CurrentValue = 1;
                }
            }
        }
        if(currentItem2 == 0)
        {
            int highestNumberItem = GreatestNumber(currentItem0, currentItem1, currentItem2, currentItem3);

            foreach (SwitchMasterItem item in items)
            {
                if (item.GetComponent<SpriteRenderer>().sprite == differentItems[highestNumberItem])
                {
                    item.GetComponent<SpriteRenderer>().sprite = differentItems[2];
                    item.CurrentValue = 2;
                }
            }
        }
        if(currentItem3 == 0)
        {
            int highestNumberItem = GreatestNumber(currentItem0, currentItem1, currentItem2, currentItem3);

            foreach (SwitchMasterItem item in items)
            {
                if (item.GetComponent<SpriteRenderer>().sprite == differentItems[highestNumberItem])
                {
                    item.GetComponent<SpriteRenderer>().sprite = differentItems[3];
                    item.CurrentValue = 3;
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

    private IEnumerator Flicker()
    {
        float currentTime = 0;

        while(currentTime < totalFlickerDuration)
        {
            while(lastRandom == random)
            {
                random = Random.Range(0, differentItems.Length);
            }

            //flicker
            lastRandom = random;
            myRenderer.sprite = differentItems[random];

            yield return new WaitForSeconds(flickerDuration);

            currentTime += Time.deltaTime + flickerDuration;

            yield return null;
        }

        CurrentValue = random;

        if (masterItem)
            ItemSmoothing();

        yield return new WaitForSeconds(itemDecisionDuration);

        if(masterItem)
            StartCoroutine(hand.Grab());
    }
}
