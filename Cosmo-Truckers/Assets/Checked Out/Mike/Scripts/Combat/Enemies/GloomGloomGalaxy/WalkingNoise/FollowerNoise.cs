using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerNoise : MonoBehaviour
{
    [SerializeField] float moveTowardsSpeed;
    [SerializeField] float shockCD;
    [SerializeField] float shockDuration;
    [SerializeField] Color shockColor;

    GameObject playerPosition;
    float currentTime = 0;
    bool isShocking = false;
    Color startingColor;
    SpriteRenderer myRenderer;
    Collider2D myCollider;
    float currentSpeed;

    private void Start()
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player");
        myRenderer = GetComponent<SpriteRenderer>();
        startingColor = myRenderer.color;
        myCollider = GetComponent<Collider2D>();
        currentSpeed = moveTowardsSpeed;
    }

    private void Update()
    {
        MoveTowardsPlayer();
        TrackTime();
    }

    private void MoveTowardsPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, playerPosition.transform.position, currentSpeed * Time.deltaTime);
    }
    private void TrackTime()
    {
        if(currentTime >= shockCD && !isShocking)
        {

            StartCoroutine(Shock());
        }
        else if(!isShocking)
        {
            currentTime += Time.deltaTime;
        }
    }

    IEnumerator Shock()
    {
        currentSpeed = 0;
        myRenderer.color = shockColor;
        myCollider.enabled = true;
        isShocking = true;

        yield return new WaitForSeconds(shockDuration);

        currentTime = 0;
        currentSpeed = moveTowardsSpeed;
        myRenderer.color = startingColor;
        myCollider.enabled = false;
        isShocking = false;
    }
}
