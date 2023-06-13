using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperSlamScroll : MonoBehaviour
{
    [SerializeField] float scrollSpeed;
    [SerializeField] bool up;
    [SerializeField] float maxSpeed;

    SuperSlam minigame;
    SafeTINA player;

    private void Start()
    {
        player = FindObjectOfType<SafeTINA>();
        minigame = FindObjectOfType<SuperSlam>();
    }

    private void Update()
    {
        if(up)
        {
            scrollSpeed = maxSpeed;
        }
        else
        {
            scrollSpeed = Mathf.Abs(player.GetComponent<Rigidbody2D>().velocity.y) * 2;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            minigame.ScrollUp(scrollSpeed);
        }
    }
}
