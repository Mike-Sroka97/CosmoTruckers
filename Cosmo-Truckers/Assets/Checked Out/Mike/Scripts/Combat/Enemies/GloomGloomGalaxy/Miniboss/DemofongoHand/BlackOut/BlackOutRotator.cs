using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackOutRotator : Rotator
{
    [HideInInspector] public bool Rotating = false;
    [SerializeField] float minChangeTime;
    [SerializeField] float maxChangeTime;
    [SerializeField] float minSpeed;
    [SerializeField] float maxSpeed;
    [SerializeField] float spinTime;
    [SerializeField] float holdTime;

    bool spinningLeft = true;
    BlackOut minigame;

    private void Start()
    {
        minigame = FindObjectOfType<BlackOut>();
    }


    private void Update()
    {
        RotateMe();
    }

    public IEnumerator RotateAlteration()
    {
        Rotating = true;

        StartCoroutine(RotateTimeTracker());

        while(Rotating)
        {
            float randomRotationSwap = Random.Range(minChangeTime, maxChangeTime);
            float randomSpeed = Random.Range(minSpeed, maxSpeed);

            if (spinningLeft)
            {
                RotateSpeed = -randomSpeed;
            }
            else
            {
                RotateSpeed = randomSpeed;
            }


            yield return new WaitForSeconds(randomRotationSwap);

            spinningLeft = !spinningLeft;
        }

        yield return new WaitForSeconds(holdTime);

        minigame.NumberOfCycles++;
        if(minigame.NumberOfCycles < minigame.MaxNumberOfCycles)
        {
            minigame.SelectHand();
        }
        else
        {
            minigame.EndMove();
        }
    }

    private IEnumerator RotateTimeTracker()
    {
        yield return new WaitForSeconds(spinTime);

        Rotating = false;
        RotateSpeed = 0;
    }
}
