using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalaxyBusterSpawner : MonoBehaviour
{
    [SerializeField] GameObject splittingBallRight;
    [SerializeField] GameObject splittingBallLeft;

    public void ShootProjectile(bool right)
    {
        if (right)
            Instantiate(splittingBallRight, transform.position, transform.rotation);
        else
            Instantiate(splittingBallLeft, transform.position, transform.rotation);
    }
}
