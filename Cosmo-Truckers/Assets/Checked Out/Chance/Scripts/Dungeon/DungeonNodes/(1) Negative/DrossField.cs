using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrossField : NCNodePopUpOptions
{
    public override void SetUp(DebuffStackSO[] augs)
    {
        base.SetUp(augs);

        List<PlayerCharacter> randomOrder = new List<PlayerCharacter>(allPlayersSorted);
        randomOrder.Shuffle();

        foreach (PlayerCharacter player in allPlayersSorted)
        {
            player.AddDebuffStack(augs[0]);
            player.Stats.Reflex = randomOrder[0].Stats.Reflex;

            randomOrder.RemoveAt(0);
        }

        StartCoroutine(Hold());
    }

    IEnumerator Hold()
    {
        currentPlayer.text = "Sorry Mike says your f***ed so have fun with the new reflex";

        yield return new WaitForSeconds(2.5f);

        Destroy(this.gameObject);
    }
}
