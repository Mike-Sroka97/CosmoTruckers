using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaPConveyorPart : MonoBehaviour
{
    
    [SerializeField] float swapPositionX;
    [SerializeField] float startPositionX;
    [SerializeField] GameObject badZone;
    [SerializeField] GameObject hittableZone;
    [SerializeField] float nodeLingerTime, goodNodeDisappearTime, lidAnimationTime = 0.25f;
    [HideInInspector] public bool NodeActive;

    [Header("Animations")]
    [SerializeField] Animator lidAnimator;
    //Each animator will have a trigger to the next animation, since it only goes in 1 direction
    [SerializeField] string animationTrigger = "trigger"; 
    Animator goodAnimator, badAnimator;


    PapNodeFlash myFlasher;
    PaPConveyor myConveyor;
    private void Start()
    {
        myFlasher = GetComponentInChildren<PapNodeFlash>();
        myConveyor = GetComponentInParent<PaPConveyor>();
        goodAnimator = hittableZone.GetComponent<Animator>();
        badAnimator = badZone.GetComponent<Animator>();
    }

    private void Update()
    {
        DestinationCheck();
    }

    private void DestinationCheck()
    {
        Vector3 truePosition = transform.position - myConveyor.transform.parent.position; 

        if (myConveyor.GetMoveSpeed() > 0)
        {
            if(truePosition.x >= swapPositionX)
            {
                transform.position = new Vector3(myConveyor.transform.parent.position.x + startPositionX, transform.position.y, transform.position.z);
            }
        }
        else
        {
            if (truePosition.x <= swapPositionX)
            {
                transform.position = new Vector3(myConveyor.transform.parent.position.x + startPositionX, transform.position.y, transform.position.z);
            }
        }
    }

    public void StartFlash(bool bad)
    {
        StartCoroutine(myFlasher.FlashMe(bad));
    }

    public IEnumerator ActivateNode(bool bad)
    {
        if(bad)
        {
            badZone.SetActive(true);
            lidAnimator.SetTrigger(animationTrigger); 
        }
        else
        {
            lidAnimator.SetTrigger(animationTrigger);
            yield return new WaitForSeconds(lidAnimationTime);
            hittableZone.SetActive(true);
            yield return new WaitForSeconds(nodeLingerTime);
            
            goodAnimator.SetTrigger(animationTrigger);
            hittableZone.GetComponent<Collider2D>().enabled = false; 
            yield return new WaitForSeconds(goodNodeDisappearTime);
            
            lidAnimator.SetTrigger(animationTrigger);
            hittableZone.SetActive(false);
            yield return new WaitForSeconds(lidAnimationTime); 

            NodeActive = false;
        }
    }
}
