using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MainMenu : MonoBehaviour
{
    public void HostGame()
    {
        FindObjectOfType<NetworkManager>().StartHost();
    }

    public void JoinGame()
    {
        FindObjectOfType<NetworkManager>().StartClient();
    }
}
