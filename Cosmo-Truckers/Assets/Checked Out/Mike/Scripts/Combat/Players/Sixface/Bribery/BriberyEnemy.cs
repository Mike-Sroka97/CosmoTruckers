using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BriberyEnemy : MonoBehaviour
{
    [SerializeField] float totalDistance; 
    [SerializeField] int row;
    [SerializeField] SpriteRenderer moneyLine;
    [SerializeField] float suckedInSpeed = 2f;
    [SerializeField] float rotationSpeed = 360f;
    [SerializeField] float shrinkScale = 0.5f;
    [SerializeField] float shrinkSpeed = 0.5f; 

    [HideInInspector] public float StartDelay;

    Rotator myRotator; 
    Bribery minigame;
    Material moneyMaterial;
    GameObject whaleMouthPosition;

    float movementSpeed;
    float sendBackDistance;
    float currentTime = 0;
    float currentDistance = 0;
    float distanceMoved; 
    float startingX; 
    float spriteStartWidth;

    bool delayOver = false;
    bool movingBack = false;
    bool doneMoving = false;

    private void Start()
    {
        minigame = FindObjectOfType<Bribery>();
        myRotator = GetComponentInChildren<Rotator>();

        whaleMouthPosition = GameObject.Find("WhaleTarget"); 

        startingX = transform.position.x;
        moneyMaterial = moneyLine.material;
        spriteStartWidth = moneyLine.size.x; 
    }

    private void Update()
    {
        Movement();
        TrackTime();
        UpdateMoneyLine();
    }

    public void DoneMoving()
    {
        doneMoving = true;
        GetComponent<Collider2D>().enabled = false;
        
        StartCoroutine(MoveToWhale());
        myRotator.RotateSpeed = rotationSpeed;

        minigame.DisabledRows[row] = true;
        minigame.IncreaseSpawnTimer();
    }

    IEnumerator MoveToWhale()
    {
        Vector3 shrunkScale = new Vector3(shrinkScale, shrinkScale, shrinkScale);

        while (transform.position.x < whaleMouthPosition.transform.position.x) 
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, shrunkScale, shrinkSpeed * Time.deltaTime); 
            transform.position = Vector3.MoveTowards(transform.position, whaleMouthPosition.transform.position, suckedInSpeed * Time.deltaTime);
            yield return null;
        }

        gameObject.SetActive(false);
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

    private void UpdateMoneyLine()
    {
        float movePercentage = distanceMoved / totalDistance;
        float spriteSizeUpdater = spriteStartWidth * (1 - movePercentage);

        moneyMaterial.SetFloat("_MainVal", movePercentage);
        moneyLine.size = new Vector2(spriteSizeUpdater, moneyLine.size.y);

        if (spriteSizeUpdater < 0.05)
        {
            moneyLine.enabled = false; 
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

    public void SetMoveValues(float _moveSpeed, float _sendBackDistance)
    {
        movementSpeed = _moveSpeed;
        sendBackDistance = _sendBackDistance; 
    }
}
