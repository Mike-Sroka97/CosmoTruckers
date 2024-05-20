using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaliceAI : Enemy
{
    [SerializeField] float yDestination = -.75f;
    [SerializeField] float fallSpeed;

    protected override void SpecialTarget(int attackIndex)
    {
        //LOL
    }

    public IEnumerator Fall()
    {
        while(transform.position.y > yDestination)
        {
            transform.position -= new Vector3(0, fallSpeed * Time.deltaTime, 0);
            yield return null;
        }

        //dust effect
        transform.position = new Vector3(transform.position.x, yDestination, transform.position.z);
        CurrentTargets.Add(FindObjectOfType<AeglarCharacter>());
        CurrentTargets.Add(FindObjectOfType<ProtoCharacter>());
        CurrentTargets.Add(FindObjectOfType<SixFaceCharacter>());
        CurrentTargets.Add(FindObjectOfType<SafeTCharacter>());
        StartTurn();
    }
}
