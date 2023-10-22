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
    [SerializeField] float rotatorSpeeds = 300f; 

    List<Rotator> myRotators = new List<Rotator>(); 
    bool spinningLeft = true;
    BlackOut minigame;

    private void Start()
    {
        minigame = FindObjectOfType<BlackOut>();
        GetRotators(); 
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
                SetRotatorSpeeds(-rotatorSpeeds);
            }
            else
            {
                RotateSpeed = randomSpeed;
                SetRotatorSpeeds(rotatorSpeeds);
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
        SetRotatorSpeeds(0);
    }

    private void GetRotators()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            myRotators.Add(transform.GetChild(i).GetComponent<Rotator>());
        }
    }

    private void SetRotatorSpeeds(float speed)
    {
        for (int i = 0; i < myRotators.Count; i++)
        {
            myRotators[i].RotateSpeed = speed; 
        }
    }
}
