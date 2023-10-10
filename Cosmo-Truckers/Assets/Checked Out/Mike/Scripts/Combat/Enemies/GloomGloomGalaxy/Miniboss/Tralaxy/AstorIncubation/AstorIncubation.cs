using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstorIncubation : CombatMove
{
    [SerializeField] float ballDelayTime;
    [SerializeField] GameObject[] platformsToDisable;
    [SerializeField] SpriteRenderer[] ballRenderers;
    [SerializeField] Material defaultMaterial, selectedMaterial;
    [SerializeField] Material defaultButtonMaterial, buttonToggleMaterial;

    int currentSelection = -1;
    SpriteRenderer currentButtonRenderer = null;
    public bool platformsDisabled { get; private set; }

    private void Start()
    {
        platformsDisabled = false; 

        StartMove();
        GenerateLayout();
        Invoke("DisablePlatforms", ballDelayTime);
    }

    private void Update()
    {
        TrackTime();        
    }

    private void DisablePlatforms()
    {
        platformsDisabled = true; 

        foreach(GameObject platform in platformsToDisable)
        {
            platform.SetActive(false);
        }
    }

    public void ButtonInteraction(int score, int ballNumber, SpriteRenderer buttonRenderer)
    {
        if (!platformsDisabled)
        {
            //Set the score here instead of in choice button, so player can't set again after the balls have been dropped
            Score = score;

            if (currentSelection != ballNumber)
            {
                if (currentSelection > -1)
                {
                    ballRenderers[currentSelection].material = defaultMaterial;
                    currentButtonRenderer.material = defaultButtonMaterial;
                }

                ballRenderers[ballNumber].material = selectedMaterial;
                currentSelection = ballNumber;

                buttonRenderer.material = buttonToggleMaterial;
                currentButtonRenderer = buttonRenderer;
            }
        }
    }

    public Material GetDefaultMaterial()
    {
        return defaultMaterial; 
    }

    protected override void TrackTime()
    {
        base.TrackTime();
    }

    public override void EndMove()
    {
        base.EndMove();
    }
}
