using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorsingAroundHorse : MonoBehaviour
{
    [SerializeField] int numberOfFlips;
    [SerializeField] float flipDelay;
    [SerializeField] float hoverTime;
    [SerializeField] float secondFlipDelay;
    [SerializeField] float time;
    [SerializeField] GameObject[] feet;
    [SerializeField] float shockYValue = -1.4f;
    [SerializeField] Vector3 endPosition;
    [SerializeField] float xClamps;
    [SerializeField] GameObject fire;
    [SerializeField] Transform spawnedFiresParent; 
    [SerializeField] Transform[] fireSpawns;
    [SerializeField] Transform[] shockwaveSpawns;
    [SerializeField] GameObject leftShock;
    [SerializeField] GameObject rightShock;
    [SerializeField] ParticleSystem[] footSparks;
    [SerializeField] SpriteRenderer horseRenderer;
    [SerializeField] Sprite[] horseSprites;
    [SerializeField] Material[] outlineMaterials; 

    Vector3 goalPosition;

    Vector3 startingPosition;
    float speed;
    float currentTimeFlipOne = 0;
    float currentTimeFlipTwo = 0;
    Rigidbody2D myBody;
    int currentFlips = 0;
    HorsingAround minigame;
    Collider2D[] myColliders;
    int orderInLayer = 2; 

    bool isFlipping = false;
    bool horsing = false;
    bool secondFlip = false;

    private void Start()
    {
        myColliders = GetComponentsInChildren<Collider2D>();
        float randomX = UnityEngine.Random.Range(-xClamps, xClamps);
        goalPosition = new Vector3(randomX, 0, 0);
        startingPosition = transform.position;
        myBody = GetComponent<Rigidbody2D>();
        speed = Vector3.Distance(transform.position, goalPosition) / time;
    }

    private void Update()
    {
        TrackTime();
        Flip();
    }

    private void TrackTime()
    {
        if (isFlipping || currentFlips >= numberOfFlips)
            return;

        if(!secondFlip)
        {
            currentTimeFlipOne += Time.deltaTime;

            if(currentTimeFlipOne >= flipDelay)
            {
                isFlipping = true;
            }
        }
        else if(secondFlip)
        {
            currentTimeFlipTwo += Time.deltaTime;

            if(currentTimeFlipTwo >= secondFlipDelay)
            {
                horseRenderer.sprite = horseSprites[0];
                horseRenderer.material = outlineMaterials[1]; 

                foreach (GameObject foot in feet)
                {
                    foot.SetActive(false);
                }
                foreach(Collider2D collider in myColliders)
                {
                    collider.enabled = false;
                }
                myBody.gravityScale = 0;
                myBody.constraints = RigidbodyConstraints2D.None;
                isFlipping = true;
            }
        }
    }

    private void Flip()
    {
        if (!isFlipping)
            return;

        if(!horsing)
            StartCoroutine(SpinHorse());
    }

    IEnumerator SpinHorse()
    {
        horsing = true;
        float elapsedTime = 0f;
        Vector3 startRotation = transform.localEulerAngles;
        Vector3 targetRotation = startRotation + new Vector3(0f, 0f, -360f); // Rotate 360 degrees around the Z-axis

        if(secondFlip)
        {
            goalPosition = endPosition;
        }
        else
        {
            horseRenderer.material = outlineMaterials[0];
            float randomX = UnityEngine.Random.Range(-xClamps, xClamps);
            goalPosition = new Vector3(randomX, 0, 0);
        }

        speed = Vector3.Distance(transform.position, goalPosition) / time;

        while (elapsedTime < time)
        {
            transform.position = Vector2.MoveTowards(transform.position, goalPosition, Time.deltaTime * speed);
            float t = elapsedTime / time;
            transform.localEulerAngles = Vector3.Lerp(startRotation, targetRotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localEulerAngles = targetRotation; // Ensure the rotation is exactly 360 degrees

        yield return new WaitForSeconds(hoverTime);

        if(!secondFlip)
        {
            foreach(GameObject foot in feet)
            {
                foot.SetActive(true);
            }
            myBody.gravityScale = 5;
            myBody.constraints = RigidbodyConstraints2D.FreezePositionX;

            while(transform.position.y > shockYValue)
            {
                yield return null;
            }

            //Horse has hit the ground
            horseRenderer.sprite = horseSprites[1]; 

            foreach(Transform spawn in fireSpawns)
            {
                GameObject fireObject = Instantiate(fire, spawn.position, Quaternion.identity);
                fireObject.transform.parent = spawnedFiresParent; 
                fireObject.GetComponent<SpriteRenderer>().sortingOrder = orderInLayer;
                orderInLayer++; 
            }

            foreach (ParticleSystem sparks in footSparks)
            {
                sparks.Play(); 
            }

            Instantiate(leftShock, shockwaveSpawns[0].position, Quaternion.identity, transform.parent);
            Instantiate(rightShock, shockwaveSpawns[1].position, Quaternion.identity, transform.parent);
        }
        else
        {
            transform.position = startingPosition;
            currentFlips++;
            foreach (Collider2D collider in myColliders)
            {
                collider.enabled = true;
            }
        }

        isFlipping = false;
        horsing = false;
        currentTimeFlipOne = 0;
        currentTimeFlipTwo = 0;
        secondFlip = !secondFlip;
    }
}
