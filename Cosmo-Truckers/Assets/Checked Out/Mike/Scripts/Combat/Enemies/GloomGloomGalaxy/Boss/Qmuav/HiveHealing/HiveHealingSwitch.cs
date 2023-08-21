using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiveHealingSwitch : Switch
{
    [SerializeField] HiveHealingBlackHoles[] blackHoles;

    HiveHealing minigame;

    private void Start()
    {
        minigame = FindObjectOfType<HiveHealing>();
        myRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void ToggleMe()
    {
        foreach(HiveHealingBlackHoles blackHole in blackHoles)
        {
            blackHole.ToggleMe();
            minigame.Score++;
        }
    }
}
