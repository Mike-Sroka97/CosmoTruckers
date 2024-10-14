using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_Bribery : Augment
{
    [SerializeField] GameObject briberyNode;

    const float yBounds = 3.25f;
    const float xBounds = 5.25f;

    public override void Activate(AugmentStackSO stack = null)
    {
        base.Activate(stack);

        float randomX = Random.Range(-xBounds, xBounds);
        float randomY = Random.Range(-yBounds, yBounds);

        transform.position = Vector3.zero;
        GameObject briberyTemp = Instantiate(briberyNode, transform);
        briberyTemp.transform.localPosition = new Vector3(randomX, randomY, 0);
    }
    public override void StopEffect()
    {
        //LOL
    }
}
