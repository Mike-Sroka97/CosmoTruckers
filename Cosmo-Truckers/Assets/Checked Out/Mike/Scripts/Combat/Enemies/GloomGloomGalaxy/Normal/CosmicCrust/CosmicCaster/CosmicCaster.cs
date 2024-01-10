using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosmicCaster : CombatMove
{
    [SerializeField] int maxScore;

    CosmicCasterCollectable[] collectables;

    private void Start()
    {
        GenerateLayout();
        collectables = FindObjectsOfType<CosmicCasterCollectable>();

        NextCollectable();
    }

    public override void StartMove()
    {
        CosmicCrustSword[] swords = GetComponentsInChildren<CosmicCrustSword>();

        foreach (CosmicCrustSword sword in swords)
            sword.Initialize();
    }

    public void NextCollectable()
    {
        if(Score < maxScore && !MoveEnded)
        {
            int random = Random.Range(0, collectables.Length);

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
                random = Random.Range(0, collectables.Length);
            }

            collectables[random].ActivateMe();
        }
    }

    public override void EndMove()
    {
        MoveEnded = true;

        int damage = CalculateScore();
        DealDamageOrHealing(CombatManager.Instance.CharactersSelected[2], damage); //2 is always Crust
    }
}
