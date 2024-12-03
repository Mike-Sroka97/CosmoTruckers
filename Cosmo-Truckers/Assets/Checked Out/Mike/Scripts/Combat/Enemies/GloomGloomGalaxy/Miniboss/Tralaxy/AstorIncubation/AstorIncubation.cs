using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstorIncubation : CombatMove
{
    [SerializeField] float ballDelayTime;
    [SerializeField] GameObject[] platformsToDisable;
    [SerializeField] SpriteRenderer[] ballRenderers;
    [SerializeField] Material defaultMaterial, selectedMaterial;
    [SerializeField] GameObject astron;

    int currentSelection = -1;
    AstorIncubatorChoiceButton currentButton = null;
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

        base.StartMove();
    }

    private void DisablePlatforms()
    {
        platformsDisabled = true; 

        foreach(GameObject platform in platformsToDisable)
        {
            platform.SetActive(false);
        }
    }

    public void ButtonInteraction(int score, int ballNumber, AstorIncubatorChoiceButton button)
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
                    currentButton.NotPressed(); 
                }

                ballRenderers[ballNumber].material = selectedMaterial;
                currentSelection = ballNumber;

                currentButton = button;
                currentButton.Pressed();
            }
        }
    }

    public override void EndMove()
    {
        if (FindObjectOfType<InaPractice>())
            return;

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
