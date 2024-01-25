using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Devastation : CombatMove
{
    [SerializeField] PlayerBasedParabolaMovement[] balls;

    private void Start()
    {
        GenerateLayout();
    }

    public override void StartMove()
    {
        Player[] players = FindObjectsOfType<Player>();
        int numberOfPlayers = players.Length - 1;

        for (int i = 0; i < balls.Length; i++)
        {
            if (i <= numberOfPlayers)
            {
                balls[i].Active = true;
                balls[i].SetPlayer(players[i]);
            }
            else
            {
                balls[i].Active = false;
            }
        }

        GetComponentInChildren<DevastationCentralBall>().enabled = true;
        GetComponentInChildren<DevastationCentralBall>().GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        base.StartMove();
        SetupMultiplayer();

        base.StartMove();
    }

    public override void EndMove()
    {
        MoveEnded = true;

        for (int i = 0; i < CombatManager.Instance.GetCharactersSelected.Count; i++)
        {
            int damage = CalculateMultiplayerScore(PlayerScores[CombatManager.Instance.GetCharactersSelected[i].GetComponent<PlayerCharacter>()]);
            DealDamageOrHealing(CombatManager.Instance.GetCharactersSelected[i], damage);
        }
    }
}
