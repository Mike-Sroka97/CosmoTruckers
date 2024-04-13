using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpVortex : NCNodePopUpOptions
{
    public string header = "You have a strange feeling upon going into the vortex. . .";


    public override void SetUp(DebuffStackSO[] augs)
    {
        StartCoroutine(TextWait(augs));
    }

    void SwapHP()
    {
        List<int> oldHp = new();

        for(int i = 0; i < allPlayersSorted.Count; i++)
            oldHp.Add(allPlayersSorted[i].CurrentHealth);

        MathCC.Shuffle<int>(oldHp);

        for (int i = 0; i < allPlayersSorted.Count; i++)
            allPlayersSorted[i].CurrentHealth = oldHp[i];

        Destroy(this.gameObject);
    }

    IEnumerator TextWait(DebuffStackSO[] augs)
    {
        currentPlayer.text = header;

        yield return new WaitForSeconds(2.5f);

        base.SetUp(augs);

        SwapHP();
    }
}
