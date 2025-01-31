using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPPsendBack : MonoBehaviour
{
    [SerializeField] float moveDelay = 0.1f;
    [SerializeField] GameObject sendBackParticle; 

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
            layoutGenerator.DestroyMe();

        Instantiate(sendBackParticle, player.position, Quaternion.identity, minigame.transform);
        player.position = minigame.CurrentCheckPointLocation;
        minigame.DefaultDeadZone.SetActive(true);
    }
}
