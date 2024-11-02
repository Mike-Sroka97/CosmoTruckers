using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosmicCrustSword : MonoBehaviour
{
    [SerializeField] float spinSpeed;
    [SerializeField] float spinTime;
    [SerializeField] float rotateTowardsSpeed;
    [SerializeField] float rotateTowardsTime;
    [SerializeField] float chargeSpeed;
    [SerializeField] float chargeTime;

    float xClamp = 5.5f;
    float yClamp = 3.5f;

    GameObject target;

    public void Initialize()
    {
        target = FindObjectOfType<Player>().gameObject;
        StartCoroutine(CCSword());
    }

    IEnumerator CCSword()
    {
        float currentTime = 0;

        while(currentTime < spinTime)
        {
            transform.Rotate(new Vector3(0, 0, spinSpeed * Time.deltaTime));
            currentTime += Time.deltaTime;
            yield return null;
        }

        currentTime = 0;
        while(currentTime <= rotateTowardsTime)
        {
            float angle = Mathf.Atan2(target.transform.localPosition.y - transform.localPosition.y, target.transform.localPosition.x - transform.localPosition.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateTowardsSpeed * Time.deltaTime);
            currentTime += Time.deltaTime;
            yield return null;
        }

        currentTime = 0;
        while (currentTime <= chargeTime)
        {
            transform.localPosition += transform.right * chargeSpeed * Time.deltaTime;
            bool clampCheck = ClampCheck();
            if (clampCheck)
                currentTime = chargeTime;
            currentTime += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(CCSword());
    }

    private bool ClampCheck()
    {
        if(transform.localPosition.x >= xClamp
            || transform.localPosition.x <= -xClamp
            || transform.localPosition.y >= yClamp
            || transform.localPosition.y <= -yClamp)
        {
            return true;
        }
        return false;
    }
}
