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
    public int GetCharacterSelected { get => CharacterSelected; }
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
        if (!authority)
        {
            NextPanel.gameObject.SetActive(false);
            PrevPanel.gameObject.SetActive(false);
            return;
        }

        CheckSelection();

        NetworkTestManager.OnClientChange.AddListener(() => CmdSelectCharacter(CharacterSelected));
        NextPanel.onClick.AddListener(delegate { GoToNextPanel(); });
        PrevPanel.onClick.AddListener(delegate { GoToPrevPanel(); });
    }
    private void OnDisable()
    {
        NetworkTestManager.OnClientChange.RemoveListener(() => CmdSelectCharacter(CharacterSelected));
        NextPanel.onClick.RemoveAllListeners();
        PrevPanel.onClick.RemoveAllListeners();
    }

    public void ReadyUp()
    {
        NetworkIdentity ni = NetworkClient.connection.identity;
        PlayerManager pm = ni.GetComponent<PlayerManager>();
        pm.SetPlayerCharacter(Characters[CharacterSelected].PlayerID);

        CmdReadyUp();
    }

    [Command(requiresAuthority = false)]
    public void CmdReadyUp()
    {
        IsReady = true;
        CmdSelectCharacter(CharacterSelected);

        foreach (var obj in GameObject.FindGameObjectsWithTag("PlayerSelection"))
        {
            if (!obj.GetComponent<PlayerSelection>().GetReady)
            {
                obj.GetComponent<PlayerSelection>().CheckSelection();
            }
        }
    }

    [Command]
    public void CmdReadyUpAI()
    {
        IsReady = true;
        CmdSelectCharacter(CharacterSelected);
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

        if (CharacterSelected < Characters.Count - 1)
            CharacterSelected++;
        else
            CharacterSelected = 0;

        CheckSelection();
        CmdSelectCharacter(CharacterSelected);
    }
    void GoToPrevPanel()
    {
        if (GetReady) return;

        if (CharacterSelected > 0)
            CharacterSelected--;
        else
            CharacterSelected = Characters.Count - 1;

        CheckSelection(false);
        CmdSelectCharacter(CharacterSelected);
    }

    public void CheckSelection(bool Add = true)
    {
        List<int> na = new List<int>();

        foreach (var obj in GameObject.FindGameObjectsWithTag("PlayerSelection"))
        {
            if (obj.GetComponent<PlayerSelection>().GetReady)
            {
                na.Add(obj.GetComponent<PlayerSelection>().CharacterSelected);
            }
        }

        na.Sort();

        if (na.Contains(CharacterSelected))
        {
            if (Add)
                GoToNextPanel();
            else
                GoToPrevPanel();
        }
    }
}
