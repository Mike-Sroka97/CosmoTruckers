using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunnyWackyDimensionSpin : MonoBehaviour
{
    [SerializeField] float spinRate;
    [SerializeField] float shrinkRate;

    bool stop = false;
    const float triggerValue = .005f;
    void Update()
    {
        if(!stop)
        {
            transform.Rotate(new Vector3(0, 0, Time.deltaTime * spinRate));
            transform.localScale = new Vector3(transform.localScale.x - Time.deltaTime * shrinkRate, transform.localScale.y - Time.deltaTime * shrinkRate, transform.localScale.z);
        }
        if(transform.localScale.x <= triggerValue)
        {
            stop = true;
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
