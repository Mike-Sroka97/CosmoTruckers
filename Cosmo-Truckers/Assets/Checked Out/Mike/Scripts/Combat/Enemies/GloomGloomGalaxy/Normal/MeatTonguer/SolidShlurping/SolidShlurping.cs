using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolidShlurping : CombatMove
{
    [Space(20)]
    [Header("Minigame Specifics")]
    [SerializeField] float alternateDelay;
    [SerializeField] Transform tongue;
    [SerializeField] float tongueMoveSpeed;
    [SerializeField] float tongueMinY;
    [SerializeField] float tongueMaxY;
    [SerializeField] float pause;
    [SerializeField] float maxTime;

    SolidShlurpingPlatforms[] platforms;
    Rigidbody2D tongueBody;
    float currentAlternateTime = 3; //sue me
    float currentScoreTime = 0;
    bool trackTime = true;
    bool firstSide = true;

    private void Start()
    {
        tongueBody = tongue.GetComponent<Rigidbody2D>();

        StartMove();
        GenerateLayout();
        platforms = FindObjectsOfType<SolidShlurpingPlatforms>();
        platforms[1].gameObject.SetActive(false);
    }

    private void Update()
    {
        if(!MoveEnded)
        {
            TrackDeath();
            TrackTime();
        }
    }

    private void TrackDeath()
    {
        currentScoreTime += Time.deltaTime;

        if(currentScoreTime >= maxTime || PlayerDead)
        {
            EndMove();
        }
    }

    private void TrackTime()
    {
        if (!trackTime)
            return;

        currentAlternateTime += Time.deltaTime;

        if(currentAlternateTime >= alternateDelay)
        {
            StartCoroutine(Alternate());
        }
    }

    IEnumerator Alternate()
    {
        trackTime = false;
        currentAlternateTime = 0;
        tongueBody.velocity = new Vector2(0, -tongueMoveSpeed);

        foreach (SolidShlurpingPlatforms platform in platforms)
        {
            if(platform.gameObject.activeInHierarchy)
            {
                platform.PhaseOut();
            }
            platform.gameObject.SetActive(true);
        }

        while (tongue.position.y > tongueMinY)
        {
            yield return null;
        }

        tongue.position = new Vector3(tongue.position.x, tongueMinY, tongue.position.z);
        tongueBody.velocity = Vector2.zero;

        yield return new WaitForSeconds(pause);

        tongueBody.velocity = new Vector2(0, tongueMoveSpeed);

        while(tongue.position.y < tongueMaxY)
        {
            yield return null;
        }

        tongue.position = new Vector3(tongue.position.x, tongueMaxY, tongue.position.z);
        tongueBody.velocity = Vector2.zero;

        if (firstSide)
        {
            platforms[0].gameObject.SetActive(false);
        }
        else
        {
            platforms[1].gameObject.SetActive(false);
        }

        firstSide = !firstSide;
        trackTime = true;
    }

    public override void EndMove()
    {
        MoveEnded = true;
        Score = (int)currentScoreTime;
        Debug.Log(Score);
    }
}
