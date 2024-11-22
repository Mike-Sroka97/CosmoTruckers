using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShakeTheStarlightSoda : EventNodeBase
{
    [SerializeField] int maxShakes;
    [SerializeField] int minShakes;
    [SerializeField] int shakeDamage;

    int randomShake;
    int currentShakes = 0;

    protected override void Start()
    {
        base.Start();
        randomShake = Random.Range(minShakes, maxShakes + 1);
    }

    public void Shake()
    {
        currentShakes++;

        if(currentShakes >= randomShake)
        {
            if (currentCharacter.CurrentHealth <= shakeDamage)
                shakeDamage = currentCharacter.CurrentHealth - 1;

            myButtons[0].GetComponentInChildren<TextMeshProUGUI>().text = $"The soda has exploded.\n<color=red> [Take {shakeDamage}]";

            currentCharacter.TakeDamage(shakeDamage, true);

            IteratePlayerReference();
            StartCoroutine(SelectionChosen());
        }
        else
        {
            AddAugmentToPlayer(augmentsToAdd[0]);
        }
    }

    public override void HandleButtonSelect(int buttonId)
    {
        if (buttonId == 0)
        {
            PopupOne.gameObject.SetActive(true);

            SetButtonWithAugInfo(augmentsToAdd[buttonId]);
        }
        else
            HandleButtonDeselect();
    }
}
