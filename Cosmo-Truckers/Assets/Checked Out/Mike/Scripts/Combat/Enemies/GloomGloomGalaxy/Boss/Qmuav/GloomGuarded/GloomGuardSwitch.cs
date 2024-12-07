using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloomGuardSwitch : Switch
{
    [SerializeField] GloomGuardSwitch nextSwitch;
    [SerializeField] GloomGuardedBlackHole nextBlackHole;
    [SerializeField] AnimationClip openAnimation, closeAnimation;
    [SerializeField] Material startMaterial; 

    Animator myAnimator; 
    GloomGuarded minigame;

    private void Awake()
    {
        minigame = FindObjectOfType<GloomGuarded>();
        myAnimator = GetComponent<Animator>();
    }

    protected override void Initialize()
    {
        base.Initialize();
        myRenderer.color = new Color(0.75f, 0.75f, 0.75f, 0.75f);
        myRenderer.material = startMaterial; 

        if (CanBeToggled)
            ActivateMe(); 
    }

    protected override void ToggleMe()
    {
        minigame.Score--;

        if (!nextSwitch)
            minigame.EndMove();
        else
        {
            myAnimator.Play(closeAnimation.name);
            myRenderer.color = new Color(0.75f, 0.75f, 0.75f, 0.75f);
            nextSwitch.ActivateMe();
            nextBlackHole.ActivateMe();
        }

        CanBeToggled = false;
    }

    public void ActivateMe()
    {
        myAnimator.Play(openAnimation.name);
        CanBeToggled = true;
        myRenderer.color = new Color(1, 1, 1, 1);
    }
}
