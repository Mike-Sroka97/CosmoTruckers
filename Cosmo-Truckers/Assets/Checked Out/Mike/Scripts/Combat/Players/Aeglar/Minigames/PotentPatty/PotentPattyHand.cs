using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotentPattyHand : MonoBehaviour
{
    [SerializeField] SpriteRenderer arrow;
    [SerializeField] float flashTime;
    [SerializeField] float punchTime;
    [SerializeField] float holdTime;
    [SerializeField] float xVelocity;
    [SerializeField] int numberofFlashes;

    public bool Activated { get; private set; }

    Rigidbody2D myBody;
    Vector3 startingPosition;

    private void Start()
    {
        Activated = false;
        myBody = GetComponent<Rigidbody2D>();
        startingPosition = transform.position;
    }

    private void Update()
    {
        ResetMe();
    }

    private void ResetMe()
    {
        if(transform.position.x < startingPosition.x && xVelocity > 0)
        {
            Activated = false;
            myBody.velocity = Vector2.zero;
            transform.position = startingPosition;
        }
        else if(transform.position.x > startingPosition.x && xVelocity < 0)
        {
            Activated = false;
            myBody.velocity = Vector2.zero;
            transform.position = startingPosition;
        }
    }

    public IEnumerator Activate()
    {
        Activated = true;
        int currentNumberOfFlashes = 0;
        float currentTime = 0;

        //Flashing arrow warning
        while (currentNumberOfFlashes < numberofFlashes)
        {
            arrow.enabled = true;
            yield return new WaitForSeconds(flashTime);
            arrow.enabled = false;
            yield return new WaitForSeconds(flashTime);
            currentNumberOfFlashes++;
        }

        //Punch
        myBody.velocity = new Vector2(xVelocity, 0);
        while(currentTime < punchTime)
        {
            currentTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        //Hold
        currentTime = 0;
        myBody.velocity = Vector2.zero;
        while (currentTime < holdTime)
        {
            currentTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        //Retract
        myBody.velocity = new Vector2(-xVelocity, 0);
    }
}
