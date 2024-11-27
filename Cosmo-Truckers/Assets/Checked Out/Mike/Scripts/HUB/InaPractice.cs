using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InaPractice : INAcombat
{
    AutoSelectMeButton returnButton;
    [HideInInspector] public HUBController Hub;
    protected override void Start()
    {
        base.Start();

        returnButton = transform.Find("ReturnButton").GetComponent<AutoSelectMeButton>();
    }

    public void StartPracticeShutDown()
    {
        StartCoroutine(MoveINACombat(false));
    }

    public override IEnumerator MoveINACombat(bool moveUp)
    {
        if (moveUp)
        {
            aboveMask.gameObject.SetActive(true);

            //Move up
            while (transform.localPosition.y < goalPosition.y)
            {
                transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);
                yield return null;
            }

            transform.localPosition = goalPosition;
            returnButton.enabled = true;
        }
        else
        {
            returnButton.enabled = false;
            aboveMask.gameObject.SetActive(false);

            //Move Down
            while (transform.localPosition.y > startingPosition.y)
            {
                transform.position -= new Vector3(0, moveSpeed * Time.deltaTime, 0);
                yield return null;
            }

            transform.localPosition = startingPosition;
            Hub.Training(false);
        }
    }
}
