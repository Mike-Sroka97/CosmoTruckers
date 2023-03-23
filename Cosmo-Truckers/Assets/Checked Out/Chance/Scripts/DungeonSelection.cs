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
        GetComponent<Button>().onClick.AddListener(delegate 
        {
            DungeonPage.SetActive(true);
            DungeonPage.GetComponent<DungeonDisplay>().DungeonScreenSetUp
            (
                DungeonData.DungonSprite, 
                DungeonData.DungeonName, 
                DungeonData.DungeonDescription
            );
        });
    }
}
