using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalaxyBurstSplitter : MonoBehaviour
{
    [SerializeField] float splitDelay;
    [SerializeField] float minXvelocity;
    [SerializeField] float maxXvelocity;
    [SerializeField] float minYvelocity;
    [SerializeField] float maxYvelocity;
    [SerializeField] int numberOfSplitees;
    [SerializeField] GameObject splitee;

    Rigidbody2D myBody;
    bool goingRight = true;

    private void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        if (myBody.velocity.x < 0)
            goingRight = false;

        Invoke("Split", splitDelay);
    }

    private void Split()
    {
        for(int i = 0; i < numberOfSplitees; i++)
        {
            float xRandom = Random.Range(minXvelocity, maxXvelocity);
            float yRandom = Random.Range(minYvelocity, maxYvelocity);

            if(!goingRight)
            {
                xRandom = -xRandom;
            }

            GameObject newSplitee = Instantiate(splitee, transform.position, transform.rotation);
            newSplitee.GetComponent<Rigidbody2D>().velocity = new Vector2(xRandom, yRandom);
        }

        Destroy(gameObject);
    }
}
