using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloomGuardSwitch : Switch
{
    [SerializeField] GloomGuardSwitch nextSwitch;
    [SerializeField] GloomGuardedBlackHole nextBlackHole;

    GloomGuarded minigame;

    private void Awake()
    {
        minigame = FindObjectOfType<GloomGuarded>();
    }

    protected override void ToggleMe()
    {
        minigame.Score--;

        if (!nextSwitch)
            minigame.EndMove();
        else
        {
            nextSwitch.ActivateMe();
            nextBlackHole.ActivateMe();
        }

        CanBeToggled = false;
    }

    public void ActivateMe()
    {
        CanBeToggled = true;
        myRenderer.color = new Color(myRenderer.color.r, myRenderer.color.g, myRenderer.color.b, 1);
    }
}
