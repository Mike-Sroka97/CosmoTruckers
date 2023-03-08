using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Mirror;

public class FunnyWackyDimensionSpin : NetworkBehaviour
{
    [SerializeField] float spinRate;
    [SerializeField] float shrinkRate;
    [SerializeField] GameObject voteTimerObject;

    bool stop = false;
    const float triggerValue = .005f;

    void Update()
    {
        FunnyRotate();
    }

    private void FunnyRotate()
    {
        if (!stop)
        {
            transform.Rotate(new Vector3(0, 0, Time.deltaTime * spinRate));
            transform.localScale = new Vector3(transform.localScale.x - Time.deltaTime * shrinkRate, transform.localScale.y - Time.deltaTime * shrinkRate, transform.localScale.z);
        }
        if (transform.localScale.x <= triggerValue && !stop)
        {
            stop = true;
            GetComponent<SpriteRenderer>().enabled = false;
            CmdVoteCounter();
            this.gameObject.SetActive(false);
        }
    }

    [Command(requiresAuthority = false)]
    void CmdVoteCounter()
    {
        if (!voteTimerObject.activeInHierarchy)
            RpcTurnOnCounter();

        voteTimerObject.GetComponent<PlayerVoting>().CmdTrackTimeValue(true);
        voteTimerObject.GetComponent<PlayerVoting>().CmdPlayerCount();
    }

    [ClientRpc]
    void RpcTurnOnCounter()
    {
        voteTimerObject.SetActive(true);
    }
}
