using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupermassiveAmplifier : CombatMove
{
    private SupermassiveAmplifierBlackHole[] blackHoles; 

    private void Start()
    {
        GenerateLayout();
        blackHoles = FindObjectsOfType<SupermassiveAmplifierBlackHole>();

        // Disable 2 of the black holes from the start
        int disabledBh = Random.Range(0, blackHoles.Length);
        blackHoles[disabledBh].SetActiveState(false);

        int disabledBh2 = Random.Range(0, blackHoles.Length);

        while (disabledBh2 == disabledBh)
            disabledBh2 = Random.Range(0, blackHoles.Length);

        blackHoles[disabledBh2].SetActiveState(false);
    }

    public override void StartMove()
    {
        GetComponentInChildren<GravityManager>().Initialize();

        SupermassiveAmplifierEye[] eyes = GetComponentsInChildren<SupermassiveAmplifierEye>(); 
        for (int i = 0; i < eyes.Length; i++)
        {
            eyes[i].SetBlackHole(blackHoles[i]);
            eyes[i].enabled = true;
        }

        SetupMultiplayer();

        base.StartMove();
    }
}
