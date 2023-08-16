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

    float startingY;

    private void Start()
    {
        startingY = transform.position.y;

        if(startCrush)
            StartCoroutine(Crush());
    }

    public IEnumerator Crush()
    {
        float currentTime = 0;

        //Engage code
        yield return new WaitForSeconds(engageDelay);

        while(currentTime < engageDuration)
        {
            transform.position -= new Vector3(0, engageSpeed * Time.deltaTime, 0);
            currentTime += Time.deltaTime;
            yield return null;
        }

        //Crush code
        yield return new WaitForSeconds(crushDelay);

        while(transform.position.y > minCrushY)
        {
            transform.position -= new Vector3(0, crushSpeed * Time.deltaTime, 0);
            yield return null;
        }

        transform.position = new Vector3(transform.position.x, minCrushY, transform.position.z);

        //Retract code
        yield return new WaitForSeconds(retractDelay);

        while(transform.position.y < startingY)
        {
            transform.position += new Vector3(0, retractSpeed * Time.deltaTime, 0);
            yield return null;
        }

        transform.position = new Vector3(transform.position.x, startingY, transform.position.z);

        //Recursion code
        if(recursive)
            StartCoroutine(Crush());
    }
}
