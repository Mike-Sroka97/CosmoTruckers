using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloomGuardedProjectile : MonoBehaviour
{
    [SerializeField] float activateDelay;

    Graviton myGraviton;
    QmuavProjectile myProjectile;

    private void Start()
    {
        myGraviton = GetComponent<Graviton>();
        myProjectile = GetComponent<QmuavProjectile>();
        Invoke("ActivateMe", activateDelay);
    }

    private void ActivateMe()
    {
        myGraviton.enabled = true;
        myProjectile.enabled = true;
    }
}
