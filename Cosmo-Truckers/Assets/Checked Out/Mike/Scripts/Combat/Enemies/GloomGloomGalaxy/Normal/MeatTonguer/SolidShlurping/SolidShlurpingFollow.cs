using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolidShlurpingFollow : MonoBehaviour
{
    Player player;
    bool initialized;

    public void Initialize()
    {
        player = FindObjectOfType<Player>();
        initialized = true;
    }

    private void Update()
    {
        if (!initialized)
            return;

        transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
    }
}
