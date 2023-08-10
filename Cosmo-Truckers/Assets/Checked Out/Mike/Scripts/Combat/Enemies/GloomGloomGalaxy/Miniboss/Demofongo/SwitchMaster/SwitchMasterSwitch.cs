using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMasterSwitch : MonoBehaviour
{
    [SerializeField] bool goLeft;
    [SerializeField] float degreesToRotate = 45f;
    [SerializeField] float rotationDuration;

    SwitchMasterHand myHand;

    private void Start()
    {
        myHand = FindObjectOfType<SwitchMasterHand>();
        if (goLeft)
            degreesToRotate = -degreesToRotate;
    }

    IEnumerator RotateMe()
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerAttack")
        {
            StartCoroutine(RotateMe());
        }
    }
}
