using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : MonoBehaviour
{
    [Header("Targeting Materials")]
    [SerializeField] Material positiveTargetMaterial;
    [SerializeField] Material negativeTargetMaterial;
    [SerializeField] Material notTargetedMaterial;

    bool isTargeting = false;
    EnumManager.TargetingType currentTargetingType;
    BaseAttackSO currentAttack;
    bool initialSetup = false;
    bool targetingEnemies = true;
    List<Character> currentlySelectedTargets;

    private void Start()
    {
        currentlySelectedTargets = new List<Character>();
    }

    private void Update()
    {
        TrackPlayerInput();
    }

    public void StartTargeting(BaseAttackSO attack)
    {
        isTargeting = true;
        currentTargetingType = attack.TargetingType;
        currentAttack = attack;
        currentlySelectedTargets.Clear();
    }

    private void TrackPlayerInput()
    {
        if (!isTargeting)
            return;

        switch (currentTargetingType)
        {
            case EnumManager.TargetingType.No_Target:
                TrackNoTargetInput();
                break;
            case EnumManager.TargetingType.Self_Target:
                TrackSelfTargetInput();
                break;
            case EnumManager.TargetingType.Single_Target:
                TrackSingleTargetInput();
                break;
            case EnumManager.TargetingType.Multi_Target_Cone:
                TrackMultiTargetConeInput();
                break;
            case EnumManager.TargetingType.Multi_Target_Choice:
                TrackMultiTargetChoice();
                break;
            case EnumManager.TargetingType.AOE:
                TrackAEOTargetInput();
                break;
            case EnumManager.TargetingType.All_Target:
                TrackAllTargetInput();
                break;

            default: break;
        }
    }

    private void ReactivateCombatManager()
    {
        foreach(Character combatSpot in currentlySelectedTargets)
        {
            CombatManager.Instance.CharactersSelected.Add(combatSpot);
        }

        CombatManager.Instance.TargetsSelected = true;
        isTargeting = false;
        currentlySelectedTargets.Clear();
    }

    private void Target(Character target)
    {
        foreach(SpriteRenderer renderer in target.TargetingSprites)
        {
            if(target.GetComponent<Enemy>())
            {
                if(currentAttack.enemyPositiveEffect)
                {
                    renderer.material = positiveTargetMaterial;
                }
                else
                {
                    renderer.material = negativeTargetMaterial;
                }
            }
            else
            {
                if(currentAttack.friendlyPositiveEffect)
                {
                    renderer.material = positiveTargetMaterial;
                }
                else
                {
                    renderer.material = negativeTargetMaterial;
                }
            }
        }
    }

    private void ClearTargets()
    {
        foreach (Character character in currentlySelectedTargets)
        {
            foreach (SpriteRenderer renderer in character.TargetingSprites)
            {
                renderer.material = notTargetedMaterial;
            }
        }

        currentlySelectedTargets.Clear();
    }

    private int FindNextSpot(bool horizontal, bool positiveDirection, bool enemy, int column)
    {
        return 0;
    }

    private void TrackNoTargetInput()
    {
        //kill long dog
    }

    private void TrackSelfTargetInput()
    {

    }

    private void TrackSingleTargetInput()
    {
        if (currentAttack.canTargetEnemies && currentAttack.canTargetFriendly)
        {
            //set initial target (start enemy side)
            if (!initialSetup)
            {
                initialSetup = true;
                targetingEnemies = true;
                foreach(Character enemy in EnemyManager.Instance.EnemyCombatSpots)
                {
                    if(!enemy.Dead)
                    {
                        currentlySelectedTargets.Add(enemy);
                        Target(enemy);
                        break;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                ReactivateCombatManager();
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                targetingEnemies = !targetingEnemies;

                if(targetingEnemies)
                {
                    foreach (Character enemy in EnemyManager.Instance.EnemyCombatSpots)
                    {
                        if (!enemy.Dead)
                        {
                            currentlySelectedTargets.Add(enemy);
                            Target(enemy);
                            break;
                        }
                    }
                }
                else
                {
                    foreach (Character player in EnemyManager.Instance.PlayerCombatSpots)
                    {
                        if (!player.Dead)
                        {
                            currentlySelectedTargets.Add(player);
                            Target(player);
                            break;
                        }
                    }
                }
            }

            //track choice
            if(Input.GetKeyDown(KeyCode.W))
            {
                if (targetingEnemies)
                {
                    int column;

                    if (currentlySelectedTargets[0].CombatSpot == 0)
                    {
                        column = 0;
                        for(int i = 3; i > 0; i--)
                        {
                            if(EnemyManager.Instance.EnemyCombatSpots[i] != null && !EnemyManager.Instance.EnemyCombatSpots[i].Dead)
                            {
                                ClearTargets();
                                currentlySelectedTargets.Add(EnemyManager.Instance.EnemyCombatSpots[i]);
                                Target(currentlySelectedTargets[0]);
                                break;
                            }
                        }
                    }
                    else if (currentlySelectedTargets[0].CombatSpot == 4)
                    {
                        column = 1;
                        FindNextSpot(false, false, true, column);
                    }
                    else if(currentlySelectedTargets[0].CombatSpot == 8)
                    {
                        column = 2;
                        FindNextSpot(false, false, true, column);
                    }
                    else
                    {
                        column = 0;
                        if (currentlySelectedTargets[0].CombatSpot <= 3)
                            column = 0;
                        else if (currentlySelectedTargets[0].CombatSpot <= 7)
                            column = 1;
                        else
                            column = 2;

                        ClearTargets();
                        int currentSpot = FindNextSpot(false, false, true, column);
                        currentlySelectedTargets.Add(EnemyManager.Instance.EnemyCombatSpots[currentSpot]);
                    }
                }
                else
                {
                    if (currentlySelectedTargets[0].CombatSpot == 0)
                    {

                    }
                    else if (currentlySelectedTargets[0].CombatSpot == 4)
                    {

                    }
                    else
                    {

                    }
                }
            }
            else if(Input.GetKeyDown(KeyCode.S))
            {
                if (targetingEnemies)
                {
                    if(currentlySelectedTargets[0].CombatSpot == 3)
                    {

                    }
                    else if (currentlySelectedTargets[0].CombatSpot == 7)
                    {

                    }
                    else
                    {

                    }
                }
                else
                {
                    if (currentlySelectedTargets[0].CombatSpot == 3)
                    {

                    }
                    else if (currentlySelectedTargets[0].CombatSpot == 7)
                    {

                    }
                    else
                    {

                    }
                }
            }
            else if(Input.GetKeyDown(KeyCode.A))
            {
                if (targetingEnemies)
                {

                }
                else
                {

                }
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                if (targetingEnemies)
                {

                }
                else
                {

                }
            }
        }
        if (currentAttack.canTargetEnemies)
        {
            //set initial target (only enemy side)
            if (!initialSetup)
            {
                initialSetup = true;
                targetingEnemies = true;
                foreach (Character enemy in EnemyManager.Instance.EnemyCombatSpots)
                {
                    if (enemy != null && !enemy.Dead)
                    {
                        currentlySelectedTargets.Add(enemy);
                        Target(enemy);
                        break;
                    }
                }
            }

            //track choice
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ReactivateCombatManager();
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                if (targetingEnemies)
                {
                    int column;

                    if (currentlySelectedTargets[0].CombatSpot == 0)
                    {
                        column = 0;
                        for (int i = 3; i > 0; i--)
                        {
                            if (EnemyManager.Instance.EnemyCombatSpots[i] != null && !EnemyManager.Instance.EnemyCombatSpots[i].Dead)
                            {
                                ClearTargets();
                                currentlySelectedTargets.Add(EnemyManager.Instance.EnemyCombatSpots[i]);
                                Target(currentlySelectedTargets[0]);
                                break;
                            }
                        }
                    }
                    else if (currentlySelectedTargets[0].CombatSpot == 4)
                    {
                        column = 1;
                        FindNextSpot(false, false, true, column);
                    }
                    else if (currentlySelectedTargets[0].CombatSpot == 8)
                    {
                        column = 2;
                        FindNextSpot(false, false, true, column);
                    }
                    else
                    {
                        column = 0;
                        if (currentlySelectedTargets[0].CombatSpot <= 3)
                            column = 0;
                        else if (currentlySelectedTargets[0].CombatSpot <= 7)
                            column = 1;
                        else
                            column = 2;

                        ClearTargets();
                        currentlySelectedTargets.Add(EnemyManager.Instance.EnemyCombatSpots[4]);
                        Target(EnemyManager.Instance.EnemyCombatSpots[4]);
                    }
                }
                else
                {
                    if (currentlySelectedTargets[0].CombatSpot == 0)
                    {

                    }
                    else if (currentlySelectedTargets[0].CombatSpot == 4)
                    {

                    }
                    else
                    {

                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                if (targetingEnemies)
                {
                    if (currentlySelectedTargets[0].CombatSpot == 3)
                    {

                    }
                    else if (currentlySelectedTargets[0].CombatSpot == 7)
                    {

                    }
                    else
                    {

                    }
                }
                else
                {
                    if (currentlySelectedTargets[0].CombatSpot == 3)
                    {

                    }
                    else if (currentlySelectedTargets[0].CombatSpot == 7)
                    {

                    }
                    else
                    {

                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                if (targetingEnemies)
                {

                }
                else
                {

                }
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                if (targetingEnemies)
                {

                }
                else
                {

                }
            }
        }
        else if (currentAttack.canTargetFriendly)
        {
            //set initial target (only player side)
            if (!initialSetup)
            {
                initialSetup = true;
                targetingEnemies = false;
                foreach (Character player in EnemyManager.Instance.PlayerCombatSpots)
                {
                    if (!player.Dead)
                    {
                        currentlySelectedTargets.Add(player);
                        Target(player);
                        break;
                    }
                }
            }

            //track choice
        }
    }

    private void TrackMultiTargetConeInput()
    {

    }

    private void TrackMultiTargetChoice()
    {

    }

    private void TrackAEOTargetInput()
    {

    }

    private void TrackAllTargetInput()
    {

    }
}
