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
    [SerializeField] Collider2D[] collidersToDisable;
    [SerializeField] SpriteRenderer handSprite; 

    Vector3 startPosition;
    Vector3 goalPosition;
    Player currentTarget;
    Player[] players;
    bool stretching = false;
    bool active;
    bool trackTime;
    float currentTime = 0;
    Vector3 startingRotation;
    Quaternion targetRotation = new Quaternion(0,0,0,0);
    Material startingMaterial;
    CascadingGoliath minigame; 

    private void Start()
    {
        minigame = FindObjectOfType<CascadingGoliath>();
        startPosition = hand.localPosition;
        startingRotation = hand.transform.localEulerAngles;
        startingMaterial = handSprite.material; 
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
            goalPosition = currentTarget.transform.localPosition;
            stretching = true;
            trackTime = false;
            currentTime = 0;
        }
    }

    private void LookAtPlayer()
    {
        if (stretching)
        {
            targetRotation.z = startingRotation.z; 
            return;
        }

        CalculatePlayerDistances();

        float angle = Mathf.Atan2(currentTarget.transform.localPosition.y - hand.localPosition.y, currentTarget.transform.localPosition.x - hand.localPosition.x) * Mathf.Rad2Deg;
        targetRotation = Quaternion.Euler(new Vector3(startingRotation.x, startingRotation.y, angle)); 
        hand.localRotation = Quaternion.RotateTowards(hand.localRotation, targetRotation, rotateSpeed * Time.deltaTime);
    }

    private void Stretch()
    {
        if (!stretching)
            return;

        // Set grab collider to active
        SetGrabbingColliders(true, 1);

        hand.localPosition = Vector2.MoveTowards(hand.localPosition, goalPosition, stretchSpeed * Time.deltaTime);

        if(hand.localPosition == goalPosition)
        {
            stretching = false;
            active = false;

            StartCoroutine(MoveBackToSpawn());
        }
    }

    IEnumerator MoveBackToSpawn()
    {
        yield return new WaitForSeconds(retractDelay);

        // Set grabbing colliders to inactive
        SetGrabbingColliders(false);
        handSprite.material = minigame.OffMaterial;

        while (hand.localPosition != startPosition)
        {
            hand.localPosition = Vector2.MoveTowards(hand.localPosition, startPosition, retractSpeed * Time.deltaTime);
            hand.localEulerAngles = Vector3.Lerp(hand.localEulerAngles, startingRotation, Time.deltaTime);
            yield return null;
        }

        // Set grab and hand colliders to active
        SetGrabbingColliders(true, 2);
        handSprite.material = startingMaterial;

        hand.transform.localEulerAngles = startingRotation;
        hand.localPosition = startPosition;
        otherHand.active = true;
        otherHand.trackTime = true;
        otherHand.CalculatePlayerDistances();
    }

    public void CalculatePlayerDistances()
    {
        float closestDistance = 100;

        foreach(Player player in players)
        {
            if(Vector2.Distance(hand.transform.localPosition, player.transform.localPosition) < closestDistance)
            {
                currentTarget = player;
                closestDistance = Vector2.Distance(hand.transform.localPosition, player.transform.localPosition);
            }
        }
    }

    public IEnumerator MoveOffScreen()
    {
        startPosition = offScreenPosition;
        SetGrabbingColliders(false);

        while (hand.localPosition != offScreenPosition)
        {
            hand.localPosition = Vector2.MoveTowards(hand.localPosition, offScreenPosition, retractSpeed * Time.deltaTime);
            hand.localEulerAngles = Vector3.Lerp(hand.localEulerAngles, startingRotation, Time.deltaTime);
            yield return null; 
        }

        hand.localEulerAngles = startingRotation; 
        hand.localPosition = offScreenPosition; 
    }

    public void SetGrabbingColliders (bool state, int collidersToSet = 3)
    {
        for (int i = 0; i < collidersToSet; i++)
        {
            collidersToDisable[i].enabled = state; 
        }
    }
}
