using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using System.Linq;

public class PlayerVoting : NetworkBehaviour
{
    [SerializeField] float maxVoteTime = 60f;
    [SerializeField] float smallTimer = 5.0f;
    [SerializeField] int[] voteTimeReductions;

    [SyncVar] bool trackTime = false;
    [SyncVar] float currentTime;
    [SyncVar] int playersVoted = 0;
    bool loading = false;

    Dictionary<string, int> Location = new Dictionary<string, int>();

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
        if(isServer && !loading)
            CmdTrackTime();
    }

    [Command(requiresAuthority = false)]
    private void CmdTrackTime()
    {
        if (trackTime)
        {
            currentTime = (playersVoted == NetworkManager.singleton.numPlayers) && (currentTime > smallTimer)  ? smallTimer : currentTime -= Time.deltaTime;

            RpcTrackTime(((int)currentTime).ToString());
            if (currentTime <= 0)
            {
                loading = true;
                trackTime = false;
                RpcTrackTime("0");

                NetworkManager.singleton.ServerChangeScene(Getlocation());
            }
        }
    }

    string Getlocation()
    {
        var sortedDict = Location.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

        return sortedDict;
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
    public void CmdPlayerCount(string dimention)
    {
        playersVoted++;

        if (Location.ContainsKey(dimention))
            Location[dimention]++;
        else
            Location.Add(dimention, 1);

        CmdDecrementVoteTime();
    }
}
