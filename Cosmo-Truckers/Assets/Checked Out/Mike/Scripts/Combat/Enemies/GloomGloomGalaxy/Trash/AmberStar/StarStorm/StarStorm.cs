using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarStorm : CombatMove
{
    [SerializeField] StarStormBlock[] blocks;
    [SerializeField] StarStormStar[] stars;
    StarStormLayout layoutToGenerate;

    private void Start()
    {
        BuildLayout();
        StartMove();
    }

    protected void BuildLayout()
    {
        int random;

        if (layouts.Length > 0)
        {
            random = UnityEngine.Random.Range(0, layouts.Length);
            layoutToGenerate = layouts[random].GetComponent<StarStormLayout>();

            for(int i = 0; i < blocks.Length; i++)
            {
                if(layoutToGenerate.Layout[i] == 0)
                {
                    blocks[i].gameObject.SetActive(false);
                }
                else if(layoutToGenerate.Layout[i] == 1)
                {
                    blocks[i].gameObject.SetActive(true);
                }
                else if(layoutToGenerate.Layout[i] == 2)
                {
                    blocks[i].gameObject.SetActive(true);
                    blocks[i].ActivateMe();
                }
            }

            for(int i = 0; i < stars.Length; i++)
            {
                if(layoutToGenerate.ActiveStars[i] == true)
                {
                    stars[i].gameObject.SetActive(true);
                }
            }
        }
    }

    public override void EndMove()
    {
        base.EndMove(); //for now
    }
}
