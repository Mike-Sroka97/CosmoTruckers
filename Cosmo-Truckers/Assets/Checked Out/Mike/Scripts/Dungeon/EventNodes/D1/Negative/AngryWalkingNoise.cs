using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
public class AngryWalkingNoise : EventNodeBase
{
    [SerializeField] GameObject Timmy;
    [SerializeField] string responseText;
    [SerializeField] string popupText = "Timothy is a Walking Noise enemy with (25) health who spawns in the enemy summon layer\n next combat.";

    protected override void Start()
    {
        base.Start();
    }

    public void NoOption()
    {
        EnemyManager.Instance.EnemySummonsToSpawn.Add(Timmy);
        descriptionText.text = responseText;
        IteratePlayerReference();
        StartCoroutine(SelectionChosen());
    }

    public override void HandleButtonSelect(int buttonId)
    {
        if(buttonId == 0)
        {
            PopupOne.gameObject.SetActive(true);

            PopupOne.PopupText.text = popupText;
        }
    }
}
