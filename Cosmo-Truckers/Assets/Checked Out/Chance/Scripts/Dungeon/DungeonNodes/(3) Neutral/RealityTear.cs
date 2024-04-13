using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RealityTear : NCNodePopUpOptions
{
    public string header = "You feel your self being moved, but are unsure where you ended up. . .";


    public override void SetUp(DebuffStackSO[] augs)
    {
        StartCoroutine(TextWait(augs));
    }

    void SwapPositions()
    {
        //swap positions
        allPlayersSorted = allPlayersSorted.OrderBy(character => character.CurrentHealth).ToList();

        for (int i = 0; i < allPlayersSorted.Count; i++)
            allPlayersSorted[i].FlipCharacter(i, true);

        Destroy(this.gameObject);
    }

    IEnumerator TextWait(DebuffStackSO[] augs)
    {
        currentPlayer.text = header;

        yield return new WaitForSeconds(2.5f);

        base.SetUp(augs);

        SwapPositions();
    }
}
