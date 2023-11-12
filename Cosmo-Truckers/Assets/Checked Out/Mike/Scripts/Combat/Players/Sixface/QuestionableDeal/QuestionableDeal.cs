using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionableDeal : CombatMove
{
    [HideInInspector] public bool Moving = true;
    [SerializeField] int[] successThresholds;
    [SerializeField] float moveSpeed;

    bool trackTime = false;
    float currentTime = 0;
    Rigidbody2D myBody;

    private void Start()
    {
        GenerateLayout();
        myBody = GetComponent<Rigidbody2D>();
    }

    public override void StartMove()
    {
        myBody.velocity = new Vector2(-moveSpeed, 0);
        GetComponent<ParentPlayer>().AdjustPlayerVelocity(myBody.velocity.x, myBody.velocity.y);
        trackTime = true;
    }

    private void Update()
    {
        TrackTime();
    }

    private void TrackTime()
    {
        if (!trackTime)
            return;

        if(!PlayerDead)
        {
            currentTime += Time.deltaTime;
        }
        if(currentTime >= successThresholds[successThresholds.Length - 1] && Moving)
        {
            myBody.velocity = Vector2.zero;
            Moving = false;
            PlayerDead = true;
            Score = successThresholds.Length;
        }
    }

    public override void EndMove()
    {
        base.EndMove();
        FindObjectOfType<SixFaceMana>().UpdateFace();
    }
}
