using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemShockBlade : MonoBehaviour
{
    [SerializeField] float staticTime = 3f;
    [HideInInspector] public bool Active;

    Animator myAnimator;

    private void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    public void StartBeam()
    {
        Active = true;
        myAnimator.Play("ssBladeCharge");
        StartCoroutine(StaticBeam());
    }

    private void StopBeam()
    {
        myAnimator.Play("ssBladeExtinguish");
        Active = false;
    }

    IEnumerator StaticBeam()
    {
        yield return new WaitForSeconds(staticTime);
        StopBeam();
    }
}
