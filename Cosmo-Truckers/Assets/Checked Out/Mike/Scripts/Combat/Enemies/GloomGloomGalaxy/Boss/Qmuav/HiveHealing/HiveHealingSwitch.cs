using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiveHealingSwitch : Switch
{
    [SerializeField] HiveHealingBlackHoles[] blackHoles;

    HiveHealing minigame;
    bool stopItGetSomeHelp = false;

    private void Start()
    {
        minigame = FindObjectOfType<HiveHealing>();
        myRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void ToggleMe()
    {
        if (stopItGetSomeHelp)
            return;

        stopItGetSomeHelp = true;

        foreach (HiveHealingBlackHoles blackHole in blackHoles)
        {
            blackHole.ToggleMe();
        }

        minigame.Score--;
    }
}
