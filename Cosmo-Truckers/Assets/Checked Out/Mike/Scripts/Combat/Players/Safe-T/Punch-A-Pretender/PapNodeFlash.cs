using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PapNodeFlash : MonoBehaviour
{
    [SerializeField] Color badColorFlash;
    [SerializeField] Color hittableColorFlash;
    [SerializeField] float flashTime;
    [SerializeField] int numberOfFlashes;
    [SerializeField] Sprite goodSprite, badSprite;

    SpriteRenderer myRenderer;
    Sprite startingSprite;
    Color startingColor;
    PaPConveyorPart myConveyorPart;

    private void Start()
    {
        myConveyorPart = GetComponentInParent<PaPConveyorPart>();
        myRenderer = GetComponent<SpriteRenderer>();
        startingColor = myRenderer.color;
        startingSprite = myRenderer.sprite;  
    }

    public IEnumerator FlashMe(bool badFlash)
    {
        myConveyorPart.NodeActive = true;
        int currentFlash = 0;

        if(badFlash)
        {
            myRenderer.sprite = badSprite;

            while (currentFlash < numberOfFlashes)
            {
                myRenderer.color = badColorFlash;
                yield return new WaitForSeconds(flashTime);

                myRenderer.color = startingColor;
                currentFlash++;
                yield return new WaitForSeconds(flashTime);
            }
        }
        else
        {
            myRenderer.sprite = goodSprite;

            while (currentFlash < numberOfFlashes)
            {
                myRenderer.color = hittableColorFlash;
                yield return new WaitForSeconds(flashTime);

                myRenderer.color = startingColor;
                currentFlash++;
                yield return new WaitForSeconds(flashTime);
            }
        }

        myRenderer.sprite = startingSprite;

        StartCoroutine(myConveyorPart.ActivateNode(badFlash));
    }
}
