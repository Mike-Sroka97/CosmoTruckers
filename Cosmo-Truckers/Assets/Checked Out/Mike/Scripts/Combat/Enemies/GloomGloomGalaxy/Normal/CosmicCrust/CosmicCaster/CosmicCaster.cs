using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosmicCaster : CombatMove
{
    [SerializeField] int maxScore;

    CosmicCasterCollectable[] collectables;

    private void Start()
    {
        StartMove();
        GenerateLayout();
        collectables = FindObjectsOfType<CosmicCasterCollectable>();

        NextCollectable();
    }

    private void Update()
    {
        TrackScore();
    }

    private void TrackScore()
    {
        if (Score >= maxScore && !MoveEnded)
        {
            Score = maxScore;
            EndMove();
        }
    }

    public void NextCollectable()
    {
        if(Score < maxScore)
        {
            int random = UnityEngine.Random.Range(0, collectables.Length);

            bool allFull = true;
            foreach(CosmicCasterCollectable collectable in collectables)
            {
                if (!collectable.Activated)
                    allFull = false;
            }
            if (allFull)
                return;

            while (collectables[random].Activated)
            {
                random = UnityEngine.Random.Range(0, collectables.Length);
            }

            collectables[random].ActivateMe();
        }
    }

    public override void EndMove()
    {
        Debug.Log(Score);
        MoveEnded = true;
    }
}
