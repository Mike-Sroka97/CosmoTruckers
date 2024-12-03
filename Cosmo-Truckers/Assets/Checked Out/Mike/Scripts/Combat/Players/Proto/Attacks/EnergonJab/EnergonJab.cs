using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergonJab : CombatMove
{
    [SerializeField] float timeBetweenShockAreaActivations;

    EnergonJabShockArea[] shockAreas;
    int lastRandom = -1;
    int lastlastRandom = -1;

    private void Start()
    {
        GenerateLayout();
        currentTime = 2.5f;
        shockAreas = FindObjectsOfType<EnergonJabShockArea>();
    }

    protected override void TrackTime()
    {
        if (!trackTime)
            return;

        base.TrackTime();
        
        if(currentTime >= timeBetweenShockAreaActivations)
        {
            currentTime = 0;
            int random = UnityEngine.Random.Range(0, shockAreas.Length);

            while(random == lastRandom || random == lastlastRandom)
            {
                random = UnityEngine.Random.Range(0, shockAreas.Length);
            }

            lastlastRandom = lastRandom;
            lastRandom = random;

            shockAreas[random].ActivateMe();
        }
    }

    public override void EndMove()
    {
        if (FindObjectOfType<InaPractice>())
            return;

        base.EndMove();

        ProtoMana mana = CombatManager.Instance.GetCurrentPlayer.GetComponent<ProtoMana>();

        if (mana.CurrentBattery == 0)
        {
            if (Score >= maxScore)
                mana.UpdateMana(2);
            else if (Score >= maxScore / 2)
                mana.UpdateMana(1);
        }
    }
}
