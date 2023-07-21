using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolidShlurpingFollow : MonoBehaviour
{
    Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
    }
}
