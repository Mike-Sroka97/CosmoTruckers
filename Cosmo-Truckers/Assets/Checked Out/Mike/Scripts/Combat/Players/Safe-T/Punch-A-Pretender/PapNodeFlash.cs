using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PapNodeFlash : MonoBehaviour
{
    [SerializeField] Color badColorFlash;
    [SerializeField] Color hittableColorFlash;
    [SerializeField] float flashTime;
    [SerializeField] int numberOfFlashes;

    SpriteRenderer myRenderer;
    Color startingColor;
    PaPConveyorPart myConveyorPart;

    private void Start()
    {
        myConveyorPart = GetComponentInParent<PaPConveyorPart>();
        myRenderer = GetComponent<SpriteRenderer>();
        startingColor = myRenderer.color;
    }

    public IEnumerator FlashMe(bool badFlash)
    {
        myConveyorPart.NodeActive = true;
        int currentFlash = 0;

        if(badFlash)
        {
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
            while (currentFlash < numberOfFlashes)
            {
                myRenderer.color = hittableColorFlash;
                yield return new WaitForSeconds(flashTime);

                myRenderer.color = startingColor;
                currentFlash++;
                yield return new WaitForSeconds(flashTime);
            }
        }

        StartCoroutine(myConveyorPart.ActivateNode(badFlash));
    }
}
