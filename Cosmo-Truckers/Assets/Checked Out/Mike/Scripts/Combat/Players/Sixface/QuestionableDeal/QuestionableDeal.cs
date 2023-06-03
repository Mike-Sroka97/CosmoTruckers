using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionableDeal : MonoBehaviour
{
    [HideInInspector] public int Score = 0;
    [HideInInspector] public bool PlayerDead = false;
    [HideInInspector] public bool Moving = true;
    [SerializeField] int[] successThresholds;
    [SerializeField] float moveSpeed;
    [SerializeField] GameObject[] layouts;

    float currentTime = 0;
    Rigidbody2D myBody;

    private void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        int random = UnityEngine.Random.Range(0, layouts.Length);
        Instantiate(layouts[random], transform);
    }

    private void Update()
    {
        TrackTime();
        MoveMe();
    }

    private void TrackTime()
    {
        if(!PlayerDead)
        {
            currentTime += Time.deltaTime;
        }
        if(currentTime >= successThresholds[successThresholds.Length - 1] && Moving)
        {
            Moving = false;
            PlayerDead = true;
            Score = successThresholds.Length;
            //handle minigame ending
        }
    }

    private void MoveMe()
    {
        if(Moving)
        {
            myBody.velocity = new Vector2(-moveSpeed, 0);
        }
    }

    public void CalculateSuccess()
    {
        int timeSurvived = Mathf.RoundToInt(currentTime);

        for(int i = 0; i < successThresholds.Length; i++)
        {
            if(timeSurvived > successThresholds[i])
            {
                Score++;
            }
        }

        Debug.Log(Score);
        //TODO send info to minigame manager and end minigame
    }
}
