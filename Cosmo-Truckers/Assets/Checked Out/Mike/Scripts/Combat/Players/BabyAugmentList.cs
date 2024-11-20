using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyAugmentList : MonoBehaviour
{
    Character myCharacter;
    BabyAugment[] babyAugments;
    BabyAugment[] vesselAugments;

    private void Start()
    {
        myCharacter = GetComponentInParent<Character>();
        babyAugments = GetComponentsInChildren<BabyAugment>();

        if (myCharacter.GetComponent<PlayerCharacter>())
            vesselAugments = myCharacter.GetComponent<PlayerCharacter>().MyVessel.GetComponentsInChildren<BabyAugment>();

        myCharacter.AugmentCountChangeEvent.AddListener(UpdateBabies);
        UpdateBabies();
    }

    private void UpdateBabies()
    {
        int numberOfAugments = myCharacter.GetAUGS.Count;

        for(int i = 0; i < babyAugments.Length; i++)
        {
            if (i < numberOfAugments)
            {
                babyAugments[i].UpdateSlot(myCharacter.GetAUGS[i]);

                if(vesselAugments.Length > 0)
                    vesselAugments[i].UpdateSlot(myCharacter.GetAUGS[i]);
            }
            else
            {
                babyAugments[i].gameObject.SetActive(false);

                if (vesselAugments.Length > 0)
                    vesselAugments[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnDestroy()
    {
        myCharacter.AugmentCountChangeEvent.RemoveListener(UpdateBabies);
    }
}
