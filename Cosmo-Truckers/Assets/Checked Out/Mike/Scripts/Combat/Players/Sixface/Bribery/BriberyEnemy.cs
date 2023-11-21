using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BriberyEnemy : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] float sendBackDistance;
    [SerializeField] float totalDistance; 
    [SerializeField] int row;
    [SerializeField] SpriteRenderer moneyLine;
    [SerializeField] Vector2 pixelSlice = new Vector2(-7f, 5f); 

    [HideInInspector] public float StartDelay;

    float currentTime = 0;
    float currentDistance = 0;
    float distanceMoved; 
    bool delayOver = false;
    bool movingBack = false;
    float startingX; 
    bool doneMoving = false;
    Bribery minigame;
    Material moneyMaterial;
    float pixelSliceAdder;

    private void Start()
    {
        startingX = transform.position.x;
        minigame = FindObjectOfType<Bribery>();
        moneyMaterial = moneyLine.material;

        pixelSliceAdder = Mathf.Abs(pixelSlice.x) + Mathf.Abs(pixelSlice.y);  
    }

    private void Update()
    {
        Movement();
        TrackTime();

        float movePercentage = distanceMoved / totalDistance;
        float sliceValue = pixelSlice.x + (pixelSliceAdder * movePercentage); 

        moneyMaterial.SetFloat("_MainVal", movePercentage);
        moneyMaterial.SetFloat("_PixelSlice", sliceValue); 
    }

    public void DoneMoving()
    {
        doneMoving = true;
        minigame.DisabledRows[row] = true;
        minigame.IncreaseSpawnTimer();
    }

    private void Movement()
    {
        if(!doneMoving && delayOver && !movingBack)
        {
            transform.position += new Vector3(movementSpeed * Time.deltaTime, 0, 0);
            //Change this value for Shader Material 
            distanceMoved += movementSpeed * Time.deltaTime; 
        }
    }

    private void TrackTime()
    {
        if(!delayOver)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= StartDelay)
            {
                delayOver = true;
            }
        }
    }

    public void SendBack()
    {
        if(!movingBack)
        {
            StartCoroutine(SendBackCoroutine());
        }
    }

    IEnumerator SendBackCoroutine()
    {
        movingBack = true;
        while(currentDistance < sendBackDistance)
        {
            transform.position -= new Vector3(movementSpeed * Time.deltaTime * 2, 0, 0);
            currentDistance += movementSpeed * Time.deltaTime * 2;

            //Change this value for Shader Material 
            distanceMoved -= movementSpeed * Time.deltaTime * 2;

            if (transform.position.x < startingX)
            {
                transform.position = new Vector3(startingX, transform.position.y, transform.position.z);
                currentDistance = sendBackDistance;
            }
            yield return new WaitForSeconds(0);
        }
        currentDistance = 0;
        movingBack = false;
    }
}
