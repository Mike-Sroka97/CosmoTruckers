using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosmicCaster : CombatMove
{
    [SerializeField] AugmentStackSO novaSword;
    [SerializeField] AugmentStackSO spikyShield;

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

        base.StartMove();
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
        else
        {
            CheckSuccess();
        }
    }

    public override void EndMove()
    {
        if (FindObjectOfType<InaPractice>())
            return;

        MoveEnded = true;

        int damage = CalculateScore();
        CombatManager.Instance.CharactersSelected[0].AddAugmentStack(novaSword);
        CombatManager.Instance.CharactersSelected[1].AddAugmentStack(spikyShield);
        DealDamageOrHealing(CombatManager.Instance.CharactersSelected[2], damage); //2 is always Crust
    }
}
