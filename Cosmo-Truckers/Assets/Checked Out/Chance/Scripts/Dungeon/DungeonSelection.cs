using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class DungeonSelection : NetworkBehaviour
{
    [SerializeField] DungeonSO DungeonData;
    [SerializeField] GameObject DungeonPage;


    public override void OnStartClient()
    {
        if (PlayerPrefs.GetInt("CurrentDungeon", 0) >= DungeonData.CurrentDungeon)
        {
            GetComponent<Button>().onClick.AddListener(delegate
            {
                DungeonPage.SetActive(true);
                DungeonPage.GetComponent<DungeonDisplay>().DungeonScreenSetUp
                (
                    DungeonData.DungonSprite,
                    DungeonData.DungeonName,
                    DungeonData.DungeonDescription,
                    DungeonData.CurrentDungeon
                );
            });
        }
        else
        {
            GetComponent<Button>().interactable = false;
        }
    }
}
