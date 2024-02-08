using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskMasterFlag : MonoBehaviour
{
    TaskMaster minigame;

    private void Start()
    {
        minigame = FindObjectOfType<TaskMaster>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.GetComponent<PlayerBody>())
        {
            Player player = collision.transform.GetComponentInChildren<PlayerBody>().Body;
            minigame.PlayerScores[player.MyCharacter]--;
            minigame.PlayerAugmentScores[player.MyCharacter]--;
            player.enabled = false;
        }
    }
}
