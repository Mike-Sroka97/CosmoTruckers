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
        player = FindObjectOfType<SixfaceINA>().transform;
        minigame = FindObjectOfType<SugarPillPlacebo>();

        Debug.Log(player.name); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
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
