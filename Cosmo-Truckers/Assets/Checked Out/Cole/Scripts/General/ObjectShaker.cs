using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectShaker : MonoBehaviour
{
    [SerializeField] float maxDistance = 0.015f;
    [SerializeField] float timeBetweenResets = 0.2f;
    [SerializeField] bool childShaker = false;
    [SerializeField] bool shakeState = true; 

    float shakeTimer;
    Vector2 randomPosition; 
    Vector2 startingPosition;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (shakeState)
        {
            Shake();
        }
    }

    private void Shake()
    {
        if (shakeTimer < timeBetweenResets)
        {
            if (childShaker)
            {
                startingPosition = transform.parent.position;
            }

            shakeTimer += Time.deltaTime;
            randomPosition = startingPosition + (Random.insideUnitCircle * maxDistance);

            transform.position = randomPosition;
        }
        else
        {
            transform.position = startingPosition;
            shakeTimer = 0;
        }
    }

    public void SetShakeState(bool state)
    {
        shakeState = state; 

        if (!state)
        {
            transform.position = startingPosition;
            shakeTimer = 0;
        }
    }
}
