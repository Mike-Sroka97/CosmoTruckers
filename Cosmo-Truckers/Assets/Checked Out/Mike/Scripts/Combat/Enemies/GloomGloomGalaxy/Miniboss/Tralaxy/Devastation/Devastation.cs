using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Devastation : CombatMove
{
    PlayerBasedParabolaMovement[] balls;

    private void Start()
    {
        Player[] players = FindObjectsOfType<Player>();
        int numberOfPlayers = players.Length - 1;
        balls = FindObjectsOfType<PlayerBasedParabolaMovement>();

        for(int i = 0; i < balls.Length; i++)
        {
            if(i <= numberOfPlayers)
            {
                balls[i].Active = true;
                balls[i].SetPlayer(players[i]);
            }
            else
            {
                balls[i].Active = false;
            }
        }

        StartMove();
        GenerateLayout();
    }

    private void Update()
    {
        TrackTime();
    }

    public override void EndMove()
    {
        Score = (int)currentTime;
        base.EndMove();
    }
}
