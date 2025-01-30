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
    [SerializeField] SpriteRenderer outlineRenderer;
    [SerializeField] Material outlineAttackMaterial; 

    Sprite lipsDefault;
    Material outlineStartMaterial; 
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
        playerPosition = FindObjectOfType<PlayerBody>().gameObject;
        myRenderer = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        minigame = GetComponentInParent<WhiteNoise>();
        myCollider = GetComponent<Collider2D>();

        currentSpeed = moveTowardsSpeed;
        startingColor = myRenderer.color;
        lipsDefault = followerLips.sprite; 
        outlineStartMaterial = outlineRenderer.material;
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
        //Shock
        currentSpeed = 0;
        myRenderer.color = shockColor;
        followerLips.sprite = lipsAttack;
        outlineRenderer.material = outlineAttackMaterial; 
        myCollider.enabled = true;
        isShocking = true;

        yield return new WaitForSeconds(shockDuration);

        //Stop Shock
        currentTime = 0;
        currentSpeed = moveTowardsSpeed;
        followerLips.sprite = lipsDefault;
        myRenderer.color = startingColor;
        outlineRenderer.material = outlineStartMaterial; 
        myCollider.enabled = false;
        isShocking = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            minigame.Score++;
            minigame.AugmentScore++;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            minigame.Score++;
            minigame.AugmentScore++;
        }
    }
}
