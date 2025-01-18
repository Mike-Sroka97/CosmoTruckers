using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedSproutPickup : PlayerPickup
{
    [SerializeField] float topY = 0.5f;
    [SerializeField] float midY = -0.5f;
    [SerializeField] float botY = -1.5f;

    protected override void Start()
    {
        int random = Random.Range(0, 3);
        float startY;

        if (random == 0)
            startY = topY;
        else if (random == 1)
            startY = midY;
        else
            startY = botY;

        transform.localPosition = new Vector3(transform.localPosition.x, startY, transform.localPosition.z);

        base.Start();
    }
}
