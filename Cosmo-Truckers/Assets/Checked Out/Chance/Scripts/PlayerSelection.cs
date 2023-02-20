using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using TMPro;

public class PlayerSelection : NetworkBehaviour
{
    [SerializeField] List<CharacterSO> Characters;
    [SyncVar] [SerializeField] int CharacterSelected = 0;
    [SyncVar][SerializeField] bool IsReady = false;

    [Space(10)]
    [SerializeField] Button NextPanel;
    [SerializeField] Button PrevPanel;
    [Space(5)]
    [SerializeField] Image CharacterImage;
    [SerializeField] TMP_Text CharacterName;

    public bool GetReady { get => IsReady; }

    private void Start()
    {
        if(!hasAuthority)
        {
            NextPanel.gameObject.SetActive(false);
            PrevPanel.gameObject.SetActive(false);
            return;
        }

        NextPanel.onClick.AddListener(delegate { GoToNextPanel(); });
        PrevPanel.onClick.AddListener(delegate { GoToPrevPanel(); });
    }
    private void OnDisable()
    {
        NextPanel.onClick.RemoveAllListeners();
        PrevPanel.onClick.RemoveAllListeners();
    }

    [Command]
    public void CmdReadyUp()
    {
        CmdSelectCharacter(CharacterSelected);
        IsReady = true;
    }

    [Command(requiresAuthority = false)]
    public void CmdSelectCharacter(int selection)
    {
        CharacterSelected = selection;

        RpcShowAllClients(selection);
    }

    [ClientRpc]
    void RpcShowAllClients(int selection)
    {
        CharacterImage.sprite = Characters[selection].CharacterImage;
        CharacterName.text = Characters[selection].CharacterName;
    }

    void GoToNextPanel()
    {
        if (GetReady) return;

        if(CharacterSelected < Characters.Count - 1)
            CmdSelectCharacter(CharacterSelected + 1);
        else
            CmdSelectCharacter(0);
    }
    void GoToPrevPanel()
    {
        if (GetReady) return;

        if (CharacterSelected > 0)
            CmdSelectCharacter(CharacterSelected - 1);
        else
            CmdSelectCharacter(Characters.Count - 1);
    }
}
