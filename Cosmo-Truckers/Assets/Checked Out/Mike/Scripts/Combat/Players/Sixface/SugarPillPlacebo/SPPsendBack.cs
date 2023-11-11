using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPPsendBack : MonoBehaviour
{
    [SerializeField] float moveDelay = 0.1f;

    Transform player;
    SugarPillPlacebo minigame;
    SPPlayoutGenerator layoutGenerator;

    private void Start()
    {
        layoutGenerator = transform.parent.parent.GetComponent<SPPlayoutGenerator>();
        minigame = FindObjectOfType<SugarPillPlacebo>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (player == null)
            player = FindObjectOfType<SixfaceINA>().transform;

        if (collision.transform.parent.transform == player.transform)
        {
            Invoke("MovePlayer", moveDelay);
        }
    }

    private void MovePlayer()
    {
        if(layoutGenerator)
        {
            layoutGenerator.DestroyMe();
        }

        player.position = minigame.CurrentCheckPointLocation;
    }
}
