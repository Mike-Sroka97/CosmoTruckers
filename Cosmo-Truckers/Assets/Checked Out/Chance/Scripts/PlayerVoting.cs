using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class PlayerVoting : NetworkBehaviour
{
    [SerializeField] float maxVoteTime = 60f;
    [SerializeField] int[] voteTimeReductions;

    [SyncVar] bool trackTime = false;
    [SyncVar] float currentTime;
    [SyncVar] int playersVoted = 0;

    private void Start()
    {
        CmdSetTimer();
    }
    [Command(requiresAuthority = false)]
    void CmdSetTimer()
    {
        //adjusts vote timer remaining time as necessary
        if (playersVoted == 2 && currentTime > voteTimeReductions[0])
        {
            currentTime = voteTimeReductions[0];
        }
        else if (playersVoted == 3 && currentTime > voteTimeReductions[1])
        {
            currentTime = voteTimeReductions[1];
        }
        else if (playersVoted == 4 && currentTime > voteTimeReductions[2])
        {
            currentTime = voteTimeReductions[2];
        }
        else
        {
            currentTime = maxVoteTime;
        }
    }

    private void Update()
    {
        //Only the host should call this to prevent time speed up 
        if(isServer)
            CmdTrackTime();
    }

    [Command(requiresAuthority = false)]
    private void CmdTrackTime()
    {
        if (trackTime)
        {
            currentTime -= Time.deltaTime;

            RpcTrackTime(((int)currentTime).ToString());
            if (currentTime <= 0)
            {
                trackTime = false;
                RpcTrackTime("0");
            }
        }
    }
    [ClientRpc]
    void RpcTrackTime(string time)
    {
        this.GetComponent<TextMeshProUGUI>().text = time;
    }


    [Command(requiresAuthority = false)]
    public void CmdDecrementVoteTime()
    {
        //adjusts vote timer remaining time as necessary
        if (playersVoted == 2 && currentTime > voteTimeReductions[0])
        {
            currentTime = voteTimeReductions[0];
        }
        else if (playersVoted == 3 && currentTime > voteTimeReductions[1])
        {
            currentTime = voteTimeReductions[1];
        }
        else if (playersVoted == 4 && currentTime > voteTimeReductions[2])
        {
            currentTime = voteTimeReductions[2];
        }
    }
    [Command(requiresAuthority = false)]
    public void CmdTrackTimeValue(bool value)
    {
        trackTime = value;
    }
    [Command(requiresAuthority = false)]
    public void CmdPlayerCount()
    {
        playersVoted++;
        CmdDecrementVoteTime();
    }
}
