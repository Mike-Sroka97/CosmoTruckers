using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalaxyBusterSpawner : MonoBehaviour
{
    [SerializeField] GameObject splittingBallRight;
    [SerializeField] GameObject splittingBallLeft;
    [SerializeField] Transform spawn;

    bool first = true;

    public void ShootProjectile(bool right)
    {
        if(first)
        {
            first = false;
            return;
        }

        if (right)
            Instantiate(splittingBallRight, spawn.position, spawn.rotation, transform.parent.parent);
        else
            Instantiate(splittingBallLeft, spawn.position, spawn.rotation, transform.parent.parent);
    }
}
