using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPPsendBack : MonoBehaviour
{
    [SerializeField] float moveDelay = 0.1f;

    Transform player;
    SugarPillPlacebo minigame;

    private void Start()
    {
        player = FindObjectOfType<SixfaceINA>().transform;
        minigame = FindObjectOfType<SugarPillPlacebo>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform == player.transform)
        {
            Invoke("MovePlayer", moveDelay);
        }
    }

    private void MovePlayer()
    {
        player.position = minigame.CurrentCheckPointLocation;
    }
}
