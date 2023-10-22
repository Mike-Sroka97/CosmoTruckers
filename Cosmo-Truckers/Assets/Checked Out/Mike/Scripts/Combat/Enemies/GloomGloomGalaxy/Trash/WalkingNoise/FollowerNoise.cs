using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerNoise : MonoBehaviour
{
    [SerializeField] float moveTowardsSpeed;
    [SerializeField] float shockCD;
    [SerializeField] float shockDuration;
    [SerializeField] Color shockColor;
    [SerializeField] SpriteRenderer followerLips; 
    [SerializeField] Sprite lipsAttack;

    Sprite lipsDefault; 
    GameObject playerPosition;
    float currentTime = 0;
    bool isShocking = false;
    Color startingColor;
    SpriteRenderer myRenderer;
    Collider2D myCollider;
    float currentSpeed;
    WhiteNoise minigame;

    private void Start()
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player");
        myRenderer = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        startingColor = myRenderer.color;
        myCollider = GetComponent<Collider2D>();
        currentSpeed = moveTowardsSpeed;
        minigame = GetComponentInParent<WhiteNoise>();
        lipsDefault = followerLips.sprite; 
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
        followerLips.sprite = lipsAttack; 
        myCollider.enabled = true;
        isShocking = true;

        yield return new WaitForSeconds(shockDuration);

        currentTime = 0;
        currentSpeed = moveTowardsSpeed;
        followerLips.sprite = lipsDefault;
        myRenderer.color = startingColor;
        myCollider.enabled = false;
        isShocking = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            minigame.Score++;
            myCollider.enabled = false;
            Debug.Log(minigame.Score);
        }
    }
}
