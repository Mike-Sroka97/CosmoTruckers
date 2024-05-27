using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUBController : MonoBehaviour
{
    int playersLockedIn;

    public void DisableAll()
    {
        playersLockedIn++;

        //start dimension load if every player in the player manager has voted
        //if(playersLockedIn < playersInLobby)


        gameObject.SetActive(false);
    }
}
