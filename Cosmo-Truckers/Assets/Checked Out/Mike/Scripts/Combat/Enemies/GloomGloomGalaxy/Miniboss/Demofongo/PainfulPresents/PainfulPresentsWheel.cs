using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PainfulPresentsWheel : MonoBehaviour
{
    [SerializeField] int numberOfBad = 3;
    [SerializeField] int numberOfGood = 2;
    [SerializeField] int numberOfHeal = 1;

    PainfulPresentsPresent[] myPresents;
    int currentNumberOfBad = 0;
    int currentNumberOfGood = 0;
    int currentNumberOfHeal = 0;

    private void Start()
    {
        myPresents = GetComponentsInChildren<PainfulPresentsPresent>();

        GeneratePresents();
    }

    private void GeneratePresents()
    {
        for(int i = 0; i < myPresents.Length; i++)
        {
            int type = Random.Range(0, 3);

            while((type == 0 && currentNumberOfBad == 3) ||
                    (type == 1 && currentNumberOfGood == 2) ||
                    (type == 2 && currentNumberOfHeal == 1))
            {
                type = Random.Range(0, 3);
            }

            if(type == 0)
            {
                currentNumberOfBad++;
            }
            else if(type == 1)
            {
                currentNumberOfGood++;
            }
            else
            {
                currentNumberOfHeal++;
            }

            myPresents[i].SetPresent(type);
        }
    }
}
