using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class EnemyMultiHitTest : CombatMove
{
    [SerializeField] StarStormBlock[] blocks;
    [SerializeField] StarStormStar[] stars;
    StarStormLayout layoutToGenerate;

    public int NumberOfHits = 3;
    public int TotalDamage = 20;

    int random;

    private void Start()
    {
        BuildLayout();
    }

    public override void StartMove()
    {
        for (int i = 0; i < stars.Length; i++)
        {
            if (layoutToGenerate.ActiveStars[i] == true)
            {
                stars[i].gameObject.SetActive(true);
            }
        }

        base.StartMove();
    }

    protected void BuildLayout()
    {
        if (layouts.Length > 0)
        {
            layoutToGenerate = layouts[1].GetComponent<StarStormLayout>();
        }
    }

    public override void EndMove()
    {
        if (FindObjectOfType<InaPractice>())
            return;

        MoveEnded = true;

        CombatManager.Instance.GetCharactersSelected[0].TakeMultiHitDamage(TotalDamage, NumberOfHits);
    }
}
