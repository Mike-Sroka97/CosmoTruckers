using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CascadingGoliathHand : MonoBehaviour
{
    [SerializeField] bool startHand;
    [SerializeField] float stretchDelay;
    [SerializeField] float stretchSpeed;
    [SerializeField] float retractDelay;
    [SerializeField] float retractSpeed;
    [SerializeField] float rotateSpeed;
    [SerializeField] Transform hand;
    [SerializeField] CascadingGoliathHand otherHand;
    [SerializeField] Vector3 offScreenPosition; 

    Vector3 startPosition;
    Vector3 goalPosition;
    Player currentTarget;
    Player[] players;
    bool stretching = false;
    bool active;
    bool trackTime;
    float currentTime = 0;
    Quaternion startingRotation;

    private void Start()
    {
        startPosition = hand.position;
        startingRotation = transform.rotation;
        players = FindObjectsOfType<Player>();

        if(startHand)
        {
            trackTime = true;
            active = true;
        }
    }

    private void Update()
    {
        if (!active)
            return;
        LookAtPlayer();
        TrackTime();
        Stretch();
    }

    private void TrackTime()
    {
        if (!trackTime)
            return;

        currentTime += Time.deltaTime;

        if(currentTime >= stretchDelay)
        {
            goalPosition = currentTarget.transform.position;
            stretching = true;
            trackTime = false;
            currentTime = 0;
        }
    }

    private void LookAtPlayer()
    {
        if (stretching)
            return;

        CalculatePlayerDistances();

        float angle = Mathf.Atan2(currentTarget.transform.position.y - transform.position.y, currentTarget.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }

    private void Stretch()
    {
        if (!stretching)
            return;

        hand.position = Vector2.MoveTowards(hand.position, goalPosition, stretchSpeed * Time.deltaTime);

        if(hand.position == goalPosition)
        {
            stretching = false;
            active = false;

            StartCoroutine(MoveBackToSpawn());
        }
    }

    IEnumerator MoveBackToSpawn()
    {
        yield return new WaitForSeconds(retractDelay);

        while(hand.position != startPosition)
        {
            hand.position = Vector2.MoveTowards(hand.position, startPosition, retractSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, startingRotation, Time.deltaTime);
            yield return null;
        }

        transform.rotation = startingRotation;
        hand.position = startPosition;
        otherHand.active = true;
        otherHand.trackTime = true;
        otherHand.CalculatePlayerDistances();
    }

    public void CalculatePlayerDistances()
    {
        float closestDistance = 100;

        foreach(Player player in players)
        {
            if(Vector2.Distance(transform.position, player.transform.position) < closestDistance)
            {
                currentTarget = player;
                closestDistance = Vector2.Distance(transform.position, player.transform.position);
            }
        }
    }


    //Cole added 9/8/2023
    public IEnumerator MoveOffScreen()
    {
        startPosition = offScreenPosition; 

        while (hand.position != offScreenPosition)
        {
            hand.position = Vector2.MoveTowards(hand.position, offScreenPosition, retractSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, startingRotation, Time.deltaTime); 
            yield return null; 
        }

        transform.rotation = startingRotation; 
        hand.position = offScreenPosition; 
    }
}
