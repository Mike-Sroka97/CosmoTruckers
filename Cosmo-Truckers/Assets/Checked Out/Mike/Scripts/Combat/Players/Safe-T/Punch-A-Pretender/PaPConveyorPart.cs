using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaPConveyorPart : MonoBehaviour
{
    
    [SerializeField] float swapPositionX;
    [SerializeField] float startPositionX;
    [SerializeField] GameObject badZone;
    [SerializeField] GameObject hittableZone;
    [SerializeField] float nodeLingerTime;
    [HideInInspector] public bool NodeActive;

    PapNodeFlash myFlasher;
    PaPConveyor myConveyor;
    private void Start()
    {
        myFlasher = GetComponentInChildren<PapNodeFlash>();
        myConveyor = GetComponentInParent<PaPConveyor>();
    }

    private void Update()
    {
        DestinationCheck();
    }

    private void DestinationCheck()
    {
        if(myConveyor.GetMoveSpeed() > 0)
        {
            if(transform.position.x >= swapPositionX)
            {
                transform.position = new Vector3(startPositionX, transform.position.y, transform.position.z);
            }
        }
        else
        {
            if (transform.position.x <= swapPositionX)
            {
                transform.position = new Vector3(startPositionX, transform.position.y, transform.position.z);
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
            //yield return new WaitForSeconds(nodeLingerTime);
            //badZone.SetActive(false);
        }
        else
        {
            hittableZone.SetActive(true);
            yield return new WaitForSeconds(nodeLingerTime);
            hittableZone.SetActive(false);
            NodeActive = false;
        }
    }
}
