using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraggyCoating : CombatMove
{
    [SerializeField] float timeBetweenRocks = 1f;
    [SerializeField] CraggyCoatingEnemy[] rocks;

    int currentRock = 0;

    private void Start()
    {
        foreach (CraggyCoatingEnemy rock in rocks)
            rock.enabled = false;
    }

    public override void StartMove()
    {
        base.StartMove();
        StartCoroutine(RockStarter());
    }

    IEnumerator RockStarter()
    {
        yield return new WaitForSeconds(timeBetweenRocks);

        rocks[currentRock].enabled = true;
        currentRock++;

        if(currentRock < rocks.Length)
            StartCoroutine(RockStarter());
    }


    public override void EndMove()
    {
        if (FindObjectOfType<InaPractice>())
            return;

        AugmentScore = CalculateAugmentScore();

        foreach(Character character in CombatManager.Instance.GetCharactersSelected)
            if (character.GetComponent<Enemy>())
                character.AddAugmentStack(DebuffToAdd, AugmentScore);
    }
}
