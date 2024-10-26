using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasedParabolaMovement : MonoBehaviour
{
    [HideInInspector] public bool Active;
    [SerializeField] Transform[] spawns;
    [SerializeField] float waitTime;
    [SerializeField] float yToCheck = 3.25f;
    [SerializeField] float graceTime = 1f;

    [SerializeField] float speed = 5f;
    float height;
    float vertexX;
    float vertexY;
    float currentSpeed;
    Rigidbody2D myBody;
    float startTime;
    Player player;
    CombatMove minigame; 

    bool moving = false;
    bool trackTime = true;
    bool trackY = false;
    float currentTime = 0;

    private void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        minigame = FindObjectOfType<CombatMove>(); 
        startTime = Time.time;
    }

    private void Update()
    {
        if (!Active || player == null)
            return;

        TrackTime();
        MoveMe();
    }

    public void SetPlayer(Player mPlayer)
    {
        player = mPlayer;
    }

    private void TrackTime()
    {
        if (!trackTime)
            return;

        currentTime += Time.deltaTime;

        if(currentTime >= waitTime)
        {
            currentTime = 0;
            ActivateMe();
        }
    }

    private void MoveMe()
    {
        if (!moving)
            return;

        // Calculate the time elapsed since the start
        float timeElapsed = Time.time - startTime;

        // Calculate the horizontal position based on time and speed
        float horizontalPosition = vertexX + (currentSpeed * timeElapsed);

        // Calculate the vertical position based on time and the parabolic equation
        float verticalPosition;
        if(currentSpeed < 0)
        {
            verticalPosition = vertexY + (height * Mathf.Sin(currentSpeed * timeElapsed));
        }
        else
        {
            verticalPosition = vertexY + (-height * Mathf.Sin(currentSpeed * timeElapsed));
        }

        // Set the object's position
        Vector3 newPosition = new Vector3(horizontalPosition, verticalPosition, transform.localPosition.z);
        myBody.MovePosition(newPosition + minigame.transform.position);

        if(trackY && transform.localPosition.y >= yToCheck && yToCheck > 0)
        {
            moving = false;
            trackTime = true;
        }
        else if(trackY && transform.localPosition.y <= yToCheck)
        {
            moving = false;
            trackTime = true;
        }
    }

    public void ActivateMe()
    {
        moving = true;
        trackTime = false;

        int random = UnityEngine.Random.Range(0, spawns.Length);
        transform.localPosition = spawns[random].localPosition;
        startTime = Time.time;

        //check player x greater or less than and flip sprite
        Transform sprite = transform.GetChild(0);
        if (player.transform.localPosition.x < transform.localPosition.x)
        {
            sprite.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            sprite.eulerAngles = new Vector3(0, 180, 0);
        }

        height = transform.localPosition.y - player.transform.localPosition.y;
        vertexX = player.transform.localPosition.x / 2;
        vertexY = transform.localPosition.y;

        if(player.transform.localPosition.x > transform.localPosition.x)
        {
            if(yToCheck < 0)
            {
                currentSpeed = speed;
            }
            else
            {
                currentSpeed = -speed;
            }

            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            if (yToCheck < 0)
            {
                currentSpeed = -speed;
            }
            else
            {
                currentSpeed = speed;
            }

            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        StartCoroutine(GracePeriod());
    }

    IEnumerator GracePeriod()
    {
        trackY = false;
        yield return new WaitForSeconds(graceTime);
        trackY = true;
    }
}
