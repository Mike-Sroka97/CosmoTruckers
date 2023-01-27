using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FunnyWackyDimensionSpin : MonoBehaviour
{
    [SerializeField] float spinRate;
    [SerializeField] float shrinkRate;
    [SerializeField] GameObject voteTimerObject;
    [SerializeField] float maxVoteTime = 60f;
    [SerializeField] int[] voteTimeReductions;

    bool stop = false;
    bool trackTime = false;
    float currentTime;
    int playersVoted = 0;
    const float triggerValue = .005f;

    private void Start()
    {
        currentTime = maxVoteTime;
    }
    void Update()
    {
        FunnyRotate();
        TrackTime();
    }

    private void TrackTime()
    {
        if(trackTime)
        {
            currentTime -= Time.deltaTime;
            voteTimerObject.GetComponent<TextMeshProUGUI>().text = ((int)currentTime).ToString();
            if(currentTime <= 0)
            {
                trackTime = false;
                voteTimerObject.GetComponent<TextMeshProUGUI>().text = "0";
            }
        }
    }

    private void FunnyRotate()
    {
        if (!stop)
        {
            transform.Rotate(new Vector3(0, 0, Time.deltaTime * spinRate));
            transform.localScale = new Vector3(transform.localScale.x - Time.deltaTime * shrinkRate, transform.localScale.y - Time.deltaTime * shrinkRate, transform.localScale.z);
        }
        if (transform.localScale.x <= triggerValue && !stop)
        {
            stop = true;
            trackTime = true;
            GetComponent<SpriteRenderer>().enabled = false;
            playersVoted++;
            DecrementVoteTime();
        }
    }

    private void DecrementVoteTime()
    {
        //displays vote timer
        if(!voteTimerObject.activeInHierarchy)
        {
            voteTimerObject.SetActive(true);
        }
       
        //adjusts vote timer remaining time as necessary
        if(playersVoted == 2 && currentTime > voteTimeReductions[0])
        {
            currentTime = voteTimeReductions[0];
        }
        else if(playersVoted == 3 && currentTime > voteTimeReductions[1])
        {
            currentTime = voteTimeReductions[1];
        }
        else if(playersVoted == 4 && currentTime > voteTimeReductions[2])
        {
            currentTime = voteTimeReductions[2];
        }
    }
}
