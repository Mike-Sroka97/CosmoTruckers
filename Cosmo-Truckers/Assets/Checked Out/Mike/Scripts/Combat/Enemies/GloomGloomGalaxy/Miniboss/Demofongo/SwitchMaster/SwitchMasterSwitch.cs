using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMasterSwitch : Switch
{
    [SerializeField] bool goLeft;
    [SerializeField] float degreesToRotate = 45f;
    [SerializeField] float rotationDuration;
    [SerializeField] Sprite defaultSwitch, hitSwitch;

    SwitchMasterHand myHand;

    private void Start()
    {
        Initialize();

        myHand = FindObjectOfType<SwitchMasterHand>();
        if (goLeft)
            degreesToRotate = -degreesToRotate;
    }

    protected override void ToggleMe()
    {
        if (!CanBeToggled)
        {
            myRenderer.sprite = hitSwitch;
            CanBeToggled = true; 
        }
        else
        {
            myRenderer.sprite = defaultSwitch;
            CanBeToggled = false;
        }
    }

    IEnumerator RotateMe()
    {
        ToggleMe();

        if (!myHand.CurrentlyGrabbing)
        {
            Quaternion initialRotation = myHand.transform.parent.rotation;
            Quaternion targetRotation = initialRotation * Quaternion.Euler(0f, 0f, degreesToRotate);

            float elapsedTime = 0f;

            while (elapsedTime < rotationDuration)
            {
                float t = elapsedTime / rotationDuration;
                myHand.transform.parent.rotation = Quaternion.Slerp(initialRotation, targetRotation, t);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            myHand.transform.parent.rotation = targetRotation;
        }

        ToggleMe(); 

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerAttack")
        {
            StartCoroutine(RotateMe());
        }
    }
}
