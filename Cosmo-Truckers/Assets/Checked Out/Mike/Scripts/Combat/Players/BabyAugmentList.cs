using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyAugmentList : MonoBehaviour
{
    Character myCharacter;
    BabyAugment[] babyAugments;

    private void Start()
    {
        myCharacter = GetComponentInParent<Character>();
        babyAugments = GetComponentsInChildren<BabyAugment>();
        myCharacter.AugmentCountChangeEvent.AddListener(UpdateBabies);
        UpdateBabies();
    }

    private void UpdateBabies()
    {
        int numberOfAugments = myCharacter.GetAUGS.Count;

        for(int i = 0; i < babyAugments.Length; i++)
        {
            if (i < numberOfAugments)
                babyAugments[i].UpdateSlot(myCharacter.GetAUGS[i]);
            else
                babyAugments[i].gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        myCharacter.AugmentCountChangeEvent.RemoveListener(UpdateBabies);
    }
}
