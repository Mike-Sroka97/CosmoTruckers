using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class DungeonDisplay : NetworkBehaviour
{
    [SerializeField] Image DungonSprite;
    [SerializeField] TMP_Text DungeonName;
    [SerializeField] TMP_Text DungeonDescription;
    public int CurrentDungeon;

    public override void OnStartClient()
    {
        gameObject.SetActive(false);
    }

    public void DungeonScreenSetUp(Sprite sprite, string name, string description, int currentDungeon)
    {
        DungonSprite.sprite = sprite;
        if(isServer)
        {
            DungeonName.GetComponent<Button>().onClick.RemoveAllListeners();
            DungeonName.GetComponent<Button>().onClick.AddListener(delegate { EnterDungeon(name); });
            DungeonName.text = $"Enter {name}";
            PlayerPrefs.SetInt("CurrentDungeon", currentDungeon);
        }
        else
        {
            DungeonName.GetComponent<Button>().enabled = false;
            DungeonName.text = $"{name}";
        }
        DungeonDescription.text = description;
    }

    void EnterDungeon(string name)
    {
        print($"Going to dungeon {name}");

        CmdEnterDungeon(name);
    }

    [Command(requiresAuthority = false)]
    private void CmdEnterDungeon(string name)
    {
        NetworkManager.singleton.ServerChangeScene("DungeonSelection");
    }
}
