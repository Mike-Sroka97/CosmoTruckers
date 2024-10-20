using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnUntoAsh : EventNodeBase
{
    [SerializeField] float waitTime = 1f;

    public void Rebirth()
    {
        currentCharacter.TakeDamage(999, true);
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(waitTime);

        currentCharacter.Resurrect(currentCharacter.Health / 2, true);
        IteratePlayerReference();
        StartCoroutine(SelectionChosen());
    }
}
