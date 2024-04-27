using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mirror;

public class ChangingRoom : NetworkBehaviour
{
    public UnityEvent EnterChangingRoom = new UnityEvent();
    public UnityEvent LeaveChangingRoom = new UnityEvent();

    [SerializeField] CostumeSelection Costumes;


    PlayerManager CurrentPlayer;
    List<Sprite> PlayerAltImages; 

    public void GetCurrentPlayer()
    {
        foreach(var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if(player.GetComponent<PlayerManager>().authority)
            {
                CurrentPlayer = player.GetComponent<PlayerManager>();
                break;
            }
        }

        PlayerAltImages = new List<Sprite>(CurrentPlayer.GetPlayer.AltCharacterImages);
        Costumes.SetOptions(PlayerAltImages);
    }

    public void UpdatePlayerSprite(int selection)
    {
        CurrentPlayer.GetPlayer.SpriteChoice = selection;
    }

    public void SaveChanges()
    {
        foreach (var player in GameObject.FindGameObjectsWithTag("MovementBlocker"))
        {
            if (player.name == CurrentPlayer.name)
            {
                CmdShowClients(CurrentPlayer, player);
            }
        }
    }

    [Command(requiresAuthority = false)]
    void CmdShowClients(PlayerManager pm, GameObject player)
    {
        RpcShowClients(pm, player);
    }
    [ClientRpc]
    void RpcShowClients(PlayerManager pm, GameObject player)
    {
        player.GetComponent<SpriteRenderer>().sprite = pm.GetPlayer.AltCharacterImages[pm.GetPlayer.SpriteChoice];
    }
}
