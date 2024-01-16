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
    [SerializeField] GameObject astron;

    int currentSelection = -1;
    SpriteRenderer currentButtonRenderer = null;
    public bool platformsDisabled { get; private set; }

    private void Start()
    {
        platformsDisabled = false; 
        GenerateLayout();
    }

    public override void StartMove()
    {
        base.StartMove();
        Invoke("DisablePlatforms", ballDelayTime);
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
            Score = 4 - score; // 4 - score because we want the higher score to be more beneficial for the player

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

    public override void EndMove()
    {
        MoveEnded = true;

        int currentNumberOfAstorToSpawn = Score;

        for(int i = 8; i <= 11; i++)
        {
            if(EnemyManager.Instance.EnemyCombatSpots[i] == null)
            {
                currentNumberOfAstorToSpawn--;
                EnemyManager.Instance.UpdateEnemySummons(astron);
            }

            if (currentNumberOfAstorToSpawn <= 0)
                return;
        }
    }
}
