using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AUG_VisionSpots : Augment
{
    [SerializeField] GameObject augment;

    GameObject tempAugment;
    public override void Activate(AugmentStackSO stack = null)
    {
        base.Activate(stack);

        tempAugment = Instantiate(augment, FindObjectOfType<INAcombat>().transform);
        tempAugment.transform.position -= new Vector3(0, .5f);

        int[] randomNumbers = { 0, 1, 2, 3, 4, 5, 6, 7, 8 };

        for (int i = 0; i < randomNumbers.Length; i++)
        {
            int temp = randomNumbers[i];
            int random = Random.Range(i, randomNumbers.Length);
            randomNumbers[i] = randomNumbers[random];
            randomNumbers[random] = temp;
        }

        SpriteRenderer[] children = tempAugment.GetComponentsInChildren<SpriteRenderer>();
        for(int i = 0; i < AugmentSO.CurrentStacks; i++)
        {
            children[randomNumbers[i]].enabled = true;
        }
    }

    public override void StopEffect()
    {
        Destroy(tempAugment);
        Destroy(gameObject);
    }
}
