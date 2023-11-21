using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StretchySpine : CombatMove
{
    [SerializeField] Transform[] ground;
    [SerializeField] float[] groundHeights;

    public override List<Character> NoTargetTargeting()
    {
        List<Character> characters = new List<Character>();

        for (int i = 4; i <= 7; i++) //checks all enemy summon spots
        {
            CombatManager.Instance.MyTargeting.TargetEmptySlot(true, i);
            if (EnemyManager.Instance.PlayerCombatSpots[i] != null && !EnemyManager.Instance.PlayerCombatSpots[i].Dead)
                characters.Add(EnemyManager.Instance.PlayerCombatSpots[i]);
        }
        
        return characters;
    }

    private void Start()
    {
        List<float> alreadyRolledValues = new List<float>();
        int groundHeight = -1;

        foreach (Transform transform in ground)
        {
            if (groundHeight == -1)
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

            transform.localPosition = new Vector3(transform.localPosition.x, groundHeights[groundHeight], transform.localPosition.z);
        }
    }

    public override void StartMove()
    {
        GetComponentInChildren<StretchySpineSpawner>().enabled = true;
    }

    public override void EndMove()
    {
        //kill summons
        //animate
        //set an animation to occur at ldgs next start turn
        //take up summon spots

        LongDogCharacter longDog = FindObjectOfType<LongDogCharacter>();

        //Calculate Shield
        if (Score < 0)
            Score = 0;
        if (Score >= maxScore)
            Score = maxScore;

        int currentShield = Score * Damage;

        longDog.TakeShielding(currentShield);
    }
}
