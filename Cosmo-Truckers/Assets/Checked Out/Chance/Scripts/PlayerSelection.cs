using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerSelection : NetworkBehaviour
{
    [SyncVar][SerializeField] bool IsReady = false;
    public bool GetReady { get => IsReady; }

    [Command]
    public void CmdReadyUp() => IsReady = true;
}
