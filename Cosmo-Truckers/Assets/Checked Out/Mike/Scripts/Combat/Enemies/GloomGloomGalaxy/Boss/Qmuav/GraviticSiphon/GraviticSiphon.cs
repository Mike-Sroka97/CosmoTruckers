using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraviticSiphon : CombatMove
{
    [SerializeField] int damagePerHit = 10;

    private void Start()
    {
        GenerateLayout();
    }

    public override void StartMove()
    {
        foreach (Graviton graviton in GetComponentsInChildren<Graviton>())
            graviton.enabled = true;

        GetComponentInChildren<GraviticSiphonProjectilePool>().enabled = true;

        base.StartMove();
    }

    public override void EndMove()
    {
        if (FindObjectOfType<InaPractice>())
            return;

        QmuavAI qmuav = FindObjectOfType<QmuavAI>();

        int damage = CalculateScore();
        damage += damagePerHit * qmuav.NumberOfTimesHitLastRound;

        //1 being base damage, pre-calcs damage after adjustments so that the healing is the same
        float DamageAdj = 1;
        DamageAdj = (float)CombatManager.Instance.GetCurrentCharacter.Stats.Damage / 100;
        int healing = CombatManager.Instance.GetCharactersSelected[0].AdjustAttackDamage((int)(damage * DamageAdj + CombatManager.Instance.GetCurrentCharacter.FlatDamageAdjustment));

        //Player Damage
        DealDamageOrHealing(CombatManager.Instance.GetCharactersSelected[0], damage, 1);

        //Qmuav Healing
        DealDamageOrHealing(CombatManager.Instance.GetCharactersSelected[1], healing, 2);
    }
}
