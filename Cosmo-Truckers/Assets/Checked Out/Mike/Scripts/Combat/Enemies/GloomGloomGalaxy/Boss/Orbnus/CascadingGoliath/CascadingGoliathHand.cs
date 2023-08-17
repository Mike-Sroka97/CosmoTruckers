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
    [SerializeField] Transform hand;
    [SerializeField] CascadingGoliathHand otherHand;

    Vector3 startPosition;
    Vector3 goalPosition;
    Player currentTarget;
    Player[] players;
    bool stretching = false;
    bool active;
    bool trackTime;
    float currentTime = 0;

    private void Start()
    {
        startPosition = hand.position;
        players = FindObjectsOfType<Player>();

        if(startHand)
            CalculatePlayerDistances();
    }

    private void Update()
    {
        if (!active)
            return;

        TrackTime();
        LookAtPlayer();
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

        transform.right = currentTarget.transform.position - transform.position;
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
            yield return null;
        }

        otherHand.CalculatePlayerDistances();
    }

    public void CalculatePlayerDistances()
    {
        active = true;
        trackTime = true;
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
}
