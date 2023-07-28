using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Augment : MonoBehaviour
{
    [SerializeField] float baseStatusEffect;
    [SerializeField] float additionalStatusEffect;
    [HideInInspector] public int Stacks;
    [HideInInspector] public float StatusEffect;

    private void Start()
    {
        if(Stacks == 1)
        {
            StatusEffect = baseStatusEffect;
        }
        else if(Stacks > 1)
        {
            StatusEffect = baseStatusEffect;
            for(int i = 0; i < Stacks - 1; i++)
            {
                StatusEffect += additionalStatusEffect;
            }
        }
        else
        {
            StatusEffect = 0;
        }

        if(StatusEffect > 1)
        {
            StatusEffect = 1;
        }
    }
}
