using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StretchySpine : CombatMove
{
    [SerializeField] int startingScore = 10;
    [SerializeField] Transform[] ground;
    [SerializeField] float[] groundHeights;

    public override void EndMove()
    {
        throw new System.NotImplementedException();
    }

    private void Start()
    {
        StartMove();
        Score = startingScore;

        List<float> alreadyRolledValues = new List<float>();
        int groundHeight = -1;

        foreach (Transform transform in ground)
        {
            if(groundHeight == -1)
            {
                groundHeight = Random.Range(0, groundHeights.Length);
            }
            else
            {
                while (alreadyRolledValues.Contains(groundHeights[groundHeight]))
                {
                    groundHeight = Random.Range(0, groundHeights.Length);
                }
            }

            alreadyRolledValues.Add(groundHeights[groundHeight]);

            transform.position = new Vector3(transform.position.x, groundHeights[groundHeight] , transform.position.z);
        }
    }
}
