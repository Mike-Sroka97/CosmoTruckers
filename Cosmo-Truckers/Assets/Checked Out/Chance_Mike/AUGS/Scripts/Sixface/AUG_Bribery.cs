using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_Bribery : Augment
{
    [SerializeField] GameObject briberyNode;

    const float yBounds = 3.75f;
    const float lowerxBound = -2.75f;
    const float upperxBound = 3.75f;

    public override void Activate(DebuffStackSO stack = null)
    {
        base.Activate(stack);

        float randomX = Random.Range(lowerxBound, upperxBound);
        float randomY = Random.Range(-yBounds, yBounds);

        GameObject briberyTemp = Instantiate(briberyNode, transform);
        briberyTemp.transform.localPosition = new Vector3(randomX, randomY, 0);
    }
    public override void StopEffect()
    {
        //LOL
    }
}
