using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbnusInfluence : NCNodePopUpOptions
{
    public string header = "You feel the presure of Orbnus upon you. . .";

    public override void SetUp(DebuffStackSO[] augs)
    {
        StartCoroutine(TextWait(augs));
    }

    IEnumerator TextWait(DebuffStackSO[] augs)
    {
        currentPlayer.text = header;

        yield return new WaitForSeconds(2.5f);

        base.SetUp(augs);

        foreach (var player in allPlayersSorted)
            player.AddDebuffStack(augs[0], 1);


        Destroy(this.gameObject);
    }
}
