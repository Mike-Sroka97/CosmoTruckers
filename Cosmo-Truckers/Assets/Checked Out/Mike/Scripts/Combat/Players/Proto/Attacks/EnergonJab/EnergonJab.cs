using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergonJab : CombatMove
{
    [SerializeField] float timeBetweenShockAreaActivations;

    EnergonJabDVD[] dvds;
    int lastRandom = -1;

    private void Start()
    {
        GenerateLayout();
        currentTime = 2.5f;
        dvds = GetComponentsInChildren<EnergonJabDVD>();
        SetNextBall();
    }

    public override void EndMove()
    {
        if (FindObjectOfType<InaPractice>())
            return;

        base.EndMove();

        ProtoMana mana = CombatManager.Instance.GetCurrentPlayer.GetComponent<ProtoMana>();

        if (mana.CurrentBattery == 0)
            mana.UpdateMana(CalculateManaGain());
    }

    public void SetNextBall()
    {
        int random = Random.Range(0, dvds.Length);

        while (random == lastRandom)
            random = Random.Range(0, dvds.Length);

        dvds[random].ActivateMe();
    }

    private int CalculateManaGain()
    {
        if (Score >= maxScore)
            return 2;
        else if (Score >= maxScore / 2)
            return 1;
        else
            return 0;
    }

    public override string TrainingDisplayText => $"You scored {Score = (Score > maxScore ? maxScore : Score)}/{maxScore} dealing {baseDamage + Score * Damage} damage. You gained {CalculateManaGain()} battery charges.";
}
