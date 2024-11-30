using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crusher : MonoBehaviour
{
    [Header("Crush Variables")]
    [SerializeField] float engageDelay;
    [SerializeField] float engageDuration;
    [SerializeField] float engageSpeed;
    [SerializeField] float crushDelay;
    [SerializeField] float crushSpeed;
    [SerializeField] float minCrushY;
    [SerializeField] float retractDelay;
    [SerializeField] float retractSpeed;

    [Space(20)]
    [Header("Extras")]
    [SerializeField] bool recursive = true;
    [SerializeField] bool startCrush = true;

    protected bool retracting;
    protected bool engaging; 
    float startingY;

    private void Start()
    {
        startingY = transform.localPosition.y;

        if(startCrush)
            StartCoroutine(Crush());
    }

    public virtual IEnumerator Crush()
    {
        float currentTime = 0;
        retracting = false; 

        //Engage code
        yield return new WaitForSeconds(engageDelay);

        while(currentTime < engageDuration)
        {
            engaging = true; 
            transform.localPosition -= new Vector3(0, engageSpeed * Time.deltaTime, 0);
            currentTime += Time.deltaTime;
            yield return null;
        }

        //Crush code
        yield return new WaitForSeconds(crushDelay);

        engaging = false;

        while (transform.localPosition.y > minCrushY)
        {
            transform.localPosition -= new Vector3(0, crushSpeed * Time.deltaTime, 0);
            yield return null;
        }

        transform.localPosition = new Vector3(transform.localPosition.x, minCrushY, transform.localPosition.z);

        //Retract code
        yield return new WaitForSeconds(retractDelay);

        while(transform.localPosition.y < startingY)
        {
            retracting = true; 
            transform.position += new Vector3(0, retractSpeed * Time.deltaTime, 0);
            yield return null;
        }

        transform.localPosition = new Vector3(transform.localPosition.x, startingY, transform.localPosition.z);

        //Recursion code
        if(recursive)
            StartCoroutine(Crush());
    }
}
