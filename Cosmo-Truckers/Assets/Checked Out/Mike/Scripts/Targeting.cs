using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : MonoBehaviour
{
    [Header("Targeting Materials")]
    [SerializeField] Material positiveTargetMaterial;
    [SerializeField] Material negativeTargetMaterial;
    [SerializeField] Material notTargetedMaterial;
    [SerializeField] Material enemyTargetingMaterial;

    bool isTargeting = false;
    EnumManager.TargetingType currentTargetingType;
    BaseAttackSO currentAttack;
    bool initialSetup = false;
    bool targetingEnemies = true;
    List<Character> currentlySelectedTargets;
    int column = 0;

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
        initialSetup = false;
        currentTargetingType = attack.TargetingType;
        currentAttack = attack;
        currentlySelectedTargets.Clear();
    }

    public void EnemyTargeting(BaseAttackSO attack, float waitTime)
    {
        CombatManager.Instance.TargetsSelected = false;
        isTargeting = true;
        currentAttack = attack;
        currentlySelectedTargets.Clear();

        foreach (Character character in CombatManager.Instance.CharactersSelected)
        {
            currentlySelectedTargets.Add(character);
            Target(character);
        }

        foreach(SpriteRenderer sprite in CombatManager.Instance.GetCurrentEnemy.TargetingSprites)
        {
            sprite.material = enemyTargetingMaterial;
        }

        StartCoroutine(TargetingWait(waitTime));
    }

    IEnumerator TargetingWait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        ReactivateCombatManager();
        foreach (SpriteRenderer sprite in CombatManager.Instance.GetCurrentEnemy.TargetingSprites)
        {
            sprite.material = notTargetedMaterial;
        }
    }

    private void TrackPlayerInput()
    {
        if (!isTargeting)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
            CancelAttack();

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
        ClearTargets();
        currentlySelectedTargets.Clear();
    }

    private void CancelAttack()
    {
        CombatManager.Instance.GetCurrentPlayer.SetupAttackWheel();
        CombatManager.Instance.StopAllCoroutines();
        isTargeting = false;
        ClearTargets();
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

    private void TrackNoTargetInput()
    {
        //set initial target
        if (!initialSetup)
        {
            initialSetup = true;
            List<Character> noTargetCharacters = currentAttack.CombatPrefab.GetComponentInChildren<CombatMove>().NoTargetTargeting();
            foreach(Character character in noTargetCharacters)
            {
                currentlySelectedTargets.Add(character);
                Target(character);
            }
        }

        //track choice
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ReactivateCombatManager();
        }
    }

    private void TrackSelfTargetInput()
    {
        //set initial target
        if (!initialSetup)
        {
            initialSetup = true;
            currentlySelectedTargets.Add(CombatManager.Instance.GetCurrentPlayer);
            Target(CombatManager.Instance.GetCurrentPlayer);
        }

        //track choice
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ReactivateCombatManager();
        }
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
                        if (enemy != null && !enemy.Dead)
                        {
                            ClearTargets();
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
                        if (player != null && !player.Dead)
                        {
                            ClearTargets();
                            currentlySelectedTargets.Add(player);
                            Target(player);
                            break;
                        }
                    }
                }
            }

            if(targetingEnemies)
            {
                //track choice
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    ReactivateCombatManager();
                }
                //track up input
                if (Input.GetKeyDown(KeyCode.W))
                {
                    TrackEnemyUpTargeting();
                }
                //track down input
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    TrackEnemyDownTargeting();
                }
                //track left input
                else if (Input.GetKeyDown(KeyCode.A))
                {
                    TrackEnemyLeftTargeting();
                }
                //track right input
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    TrackEnemyRightTargeting();
                }
            }
            else
            {
                //track choice
                if (Input.GetKeyDown(KeyCode.W))
                {
                    TrackPlayerUpTargeting();
                }
                //TODO
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    TrackPlayerDownTargeting();
                }
                //TODO
                else if (Input.GetKeyDown(KeyCode.A))
                {
                    TrackPlayerLeftTargeting();
                }
                //TODO
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    TrackPlayerRightTargeting();
                }
            }

        }
        else if (currentAttack.canTargetEnemies)
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
            //track up input
            if (Input.GetKeyDown(KeyCode.W))
            {
                TrackEnemyUpTargeting();
            }
            //track down input
            else if (Input.GetKeyDown(KeyCode.S))
            {
                TrackEnemyDownTargeting();
            }
            //track left input
            else if (Input.GetKeyDown(KeyCode.A))
            {
                TrackEnemyLeftTargeting();
            }
            //track right input
            else if (Input.GetKeyDown(KeyCode.D))
            {
                TrackEnemyRightTargeting();
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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ReactivateCombatManager();
            }

            //track up input
            if (Input.GetKeyDown(KeyCode.W))
            {
                TrackPlayerUpTargeting();
            }
            //track down input
            else if (Input.GetKeyDown(KeyCode.S))
            {
                TrackPlayerDownTargeting();
            }
            //track left input
            else if (Input.GetKeyDown(KeyCode.A))
            {
                TrackPlayerLeftTargeting();
            }
            //track right input
            else if (Input.GetKeyDown(KeyCode.D))
            {
                TrackPlayerRightTargeting();
            }
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
        //set initial target
        if (!initialSetup)
        {
            initialSetup = true;

            //Target enemies
            if (currentAttack.canTargetEnemies)
            {
                foreach (Character enemy in EnemyManager.Instance.EnemyCombatSpots)
                {
                    if (enemy != null && !enemy.Dead)
                    {
                        currentlySelectedTargets.Add(enemy);
                        Target(enemy);
                    }
                }
            }

            //Target players
            else if (currentAttack.canTargetFriendly)
            {
                foreach (Character player in EnemyManager.Instance.PlayerCombatSpots)
                {
                    if (player != null && !player.Dead)
                    {
                        currentlySelectedTargets.Add(player);
                        Target(player);
                    }
                }
            }
        }

        //track choice
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ReactivateCombatManager();
        }
    }

    private void TrackAllTargetInput()
    {

    }

    private void TrackEnemyUpTargeting()
    {
        //top of enemy column back
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
        //top of enemy column front
        else if (currentlySelectedTargets[0].CombatSpot == 4)
        {
            column = 1;
            for (int i = 7; i > 3; i--)
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
        //top of enemy summon column
        else if (currentlySelectedTargets[0].CombatSpot == 8)
        {
            column = 2;
            for (int i = 11; i > 7; i--)
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
        //anywhere but the top of a column
        else
        {
            if (currentlySelectedTargets[0].CombatSpot <= 3)
                column = 0;
            else if (currentlySelectedTargets[0].CombatSpot <= 7)
                column = 1;
            else
                column = 2;

            for (int i = currentlySelectedTargets[0].CombatSpot - 1; i > (4 * column) - 1; i--) // 4 * column helps with staying in a specific column
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
    }

    private void TrackEnemyDownTargeting()
    {
        //bottom of enemy column back
        if (currentlySelectedTargets[0].CombatSpot == 3)
        {
            column = 0;
            for (int i = 0; i < 3; i++)
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
        //bottom of enemy column front
        else if (currentlySelectedTargets[0].CombatSpot == 7)
        {
            column = 1;
            for (int i = 4; i < 7; i++)
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
        //bottom of enemy summon column
        else if (currentlySelectedTargets[0].CombatSpot == 11)
        {
            column = 2;
            for (int i = 8; i < 11; i++)
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
        //anywhere but the top of a column
        else
        {
            if (currentlySelectedTargets[0].CombatSpot <= 3)
                column = 0;
            else if (currentlySelectedTargets[0].CombatSpot <= 7)
                column = 1;
            else
                column = 2;

            for (int i = currentlySelectedTargets[0].CombatSpot + 1; i < (4 * column) + 4; i++) // 4 * column helps with staying in a specific column
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
    }

    private void TrackEnemyLeftTargeting()
    {
        //enemy summon column
        if (currentlySelectedTargets[0].CombatSpot >= 8)
        {
            column = 2;

            if (EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[0].CombatSpot - 8] != null && !EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[0].CombatSpot - 8].Dead)
            {
                Character tempCharacter = EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[0].CombatSpot - 8];
                ClearTargets();
                currentlySelectedTargets.Add(tempCharacter);
                Target(currentlySelectedTargets[0]);
            }
            else
            {
                int tempColumn = column;

                for (int i = 0; i < 3; i++)
                {
                    if (tempColumn == 2)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if (EnemyManager.Instance.EnemyCombatSpots[j] != null && !EnemyManager.Instance.EnemyCombatSpots[j].Dead)
                            {
                                ClearTargets();
                                currentlySelectedTargets.Add(EnemyManager.Instance.EnemyCombatSpots[j]);
                                Target(currentlySelectedTargets[0]);
                                return;
                            }
                        }
                        tempColumn++;
                    }
                    else if (tempColumn == 1)
                    {
                        for (int j = 8; j < 11; j++)
                        {
                            if (EnemyManager.Instance.EnemyCombatSpots[j] != null && !EnemyManager.Instance.EnemyCombatSpots[j].Dead)
                            {
                                ClearTargets();
                                currentlySelectedTargets.Add(EnemyManager.Instance.EnemyCombatSpots[j]);
                                Target(currentlySelectedTargets[0]);
                                return;
                            }
                        }
                        tempColumn++;
                    }
                    else
                    {
                        for (int j = 4; j < 7; j++)
                        {
                            if (EnemyManager.Instance.EnemyCombatSpots[j] != null && !EnemyManager.Instance.EnemyCombatSpots[j].Dead)
                            {
                                ClearTargets();
                                currentlySelectedTargets.Add(EnemyManager.Instance.EnemyCombatSpots[j]);
                                Target(currentlySelectedTargets[0]);
                                return;
                            }
                        }
                        tempColumn -= 2;
                    }
                }
            }
        }
        //anywhere but the enemy summon column
        else
        {
            if (currentlySelectedTargets[0].CombatSpot <= 3)
                column = 0;
            else
                column = 1;

            if (EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[0].CombatSpot + 4] != null && !EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[0].CombatSpot + 4].Dead)
            {
                Character tempCharacter = EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[0].CombatSpot + 4];
                ClearTargets();
                currentlySelectedTargets.Add(tempCharacter);
                Target(currentlySelectedTargets[0]);
            }
            else
            {
                int tempColumn = column;

                for (int i = 0; i < 3; i++)
                {
                    if (tempColumn == 2)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if (EnemyManager.Instance.EnemyCombatSpots[j] != null && !EnemyManager.Instance.EnemyCombatSpots[j].Dead)
                            {
                                ClearTargets();
                                currentlySelectedTargets.Add(EnemyManager.Instance.EnemyCombatSpots[j]);
                                Target(currentlySelectedTargets[0]);
                                return;
                            }
                        }
                        tempColumn++;
                    }
                    else if (tempColumn == 1)
                    {
                        for (int j = 8; j < 11; j++)
                        {
                            if (EnemyManager.Instance.EnemyCombatSpots[j] != null && !EnemyManager.Instance.EnemyCombatSpots[j].Dead)
                            {
                                ClearTargets();
                                currentlySelectedTargets.Add(EnemyManager.Instance.EnemyCombatSpots[j]);
                                Target(currentlySelectedTargets[0]);
                                return;
                            }
                        }
                        tempColumn++;
                    }
                    else
                    {
                        for (int j = 4; j < 7; j++)
                        {
                            if (EnemyManager.Instance.EnemyCombatSpots[j] != null && !EnemyManager.Instance.EnemyCombatSpots[j].Dead)
                            {
                                ClearTargets();
                                currentlySelectedTargets.Add(EnemyManager.Instance.EnemyCombatSpots[j]);
                                Target(currentlySelectedTargets[0]);
                                return;
                            }
                        }
                        tempColumn -= 2;
                    }
                }
            }
        }
    }

    private void TrackEnemyRightTargeting()
    {
        //enemy summon column
        if (currentlySelectedTargets[0].CombatSpot <= 3)
        {
            column = 0;

            if (EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[0].CombatSpot + 8] != null && !EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[0].CombatSpot + 8].Dead)
            {
                Character tempCharacter = EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[0].CombatSpot + 8];
                ClearTargets();
                currentlySelectedTargets.Add(tempCharacter);
                Target(currentlySelectedTargets[0]);
            }
            else
            {
                int tempColumn = column;

                for (int i = 0; i < 3; i++)
                {
                    if (tempColumn == 2)
                    {
                        for (int j = 4; j < 7; j++)
                        {
                            if (EnemyManager.Instance.EnemyCombatSpots[j] != null && !EnemyManager.Instance.EnemyCombatSpots[j].Dead)
                            {
                                ClearTargets();
                                currentlySelectedTargets.Add(EnemyManager.Instance.EnemyCombatSpots[j]);
                                Target(currentlySelectedTargets[0]);
                                return;
                            }
                        }
                        tempColumn--;
                    }
                    else if (tempColumn == 1)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if (EnemyManager.Instance.EnemyCombatSpots[j] != null && !EnemyManager.Instance.EnemyCombatSpots[j].Dead)
                            {
                                ClearTargets();
                                currentlySelectedTargets.Add(EnemyManager.Instance.EnemyCombatSpots[j]);
                                Target(currentlySelectedTargets[0]);
                                return;
                            }
                        }
                        tempColumn--;
                    }
                    else
                    {
                        for (int j = 8; j < 11; j++)
                        {
                            if (EnemyManager.Instance.EnemyCombatSpots[j] != null && !EnemyManager.Instance.EnemyCombatSpots[j].Dead)
                            {
                                ClearTargets();
                                currentlySelectedTargets.Add(EnemyManager.Instance.EnemyCombatSpots[j]);
                                Target(currentlySelectedTargets[0]);
                                return;
                            }
                        }
                        tempColumn += 2;
                    }
                }
            }
        }
        //anywhere but the enemy summon column
        else
        {
            if (currentlySelectedTargets[0].CombatSpot <= 3)
                column = 0;
            else
                column = 1;

            if (EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[0].CombatSpot - 4] != null && !EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[0].CombatSpot - 4].Dead)
            {
                Character tempCharacter = EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[0].CombatSpot - 4];
                ClearTargets();
                currentlySelectedTargets.Add(tempCharacter);
                Target(currentlySelectedTargets[0]);
            }
            else
            {
                int tempColumn = column;

                for (int i = 0; i < 3; i++)
                {
                    if (tempColumn == 2)
                    {
                        for (int j = 4; j < 7; j++)
                        {
                            if (EnemyManager.Instance.EnemyCombatSpots[j] != null && !EnemyManager.Instance.EnemyCombatSpots[j].Dead)
                            {
                                ClearTargets();
                                currentlySelectedTargets.Add(EnemyManager.Instance.EnemyCombatSpots[j]);
                                Target(currentlySelectedTargets[0]);
                                return;
                            }
                        }
                        tempColumn++;
                    }
                    else if (tempColumn == 1)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if (EnemyManager.Instance.EnemyCombatSpots[j] != null && !EnemyManager.Instance.EnemyCombatSpots[j].Dead)
                            {
                                ClearTargets();
                                currentlySelectedTargets.Add(EnemyManager.Instance.EnemyCombatSpots[j]);
                                Target(currentlySelectedTargets[0]);
                                return;
                            }
                        }
                        tempColumn++;
                    }
                    else
                    {
                        for (int j = 8; j < 11; j++)
                        {
                            if (EnemyManager.Instance.EnemyCombatSpots[j] != null && !EnemyManager.Instance.EnemyCombatSpots[j].Dead)
                            {
                                ClearTargets();
                                currentlySelectedTargets.Add(EnemyManager.Instance.EnemyCombatSpots[j]);
                                Target(currentlySelectedTargets[0]);
                                return;
                            }
                        }
                        tempColumn -= 2;
                    }
                }
            }
        }
    }

    private void TrackPlayerUpTargeting()
    {
        //top of player column
        if (currentlySelectedTargets[0].CombatSpot == 0)
        {
            column = 0;
            for (int i = 3; i > 0; i--)
            {
                if (EnemyManager.Instance.PlayerCombatSpots[i] != null && !EnemyManager.Instance.PlayerCombatSpots[i].Dead)
                {
                    ClearTargets();
                    currentlySelectedTargets.Add(EnemyManager.Instance.PlayerCombatSpots[i]);
                    Target(currentlySelectedTargets[0]);
                    break;
                }
            }
        }
        //top of player summon column
        else if (currentlySelectedTargets[0].CombatSpot == 4)
        {
            column = 1;
            for (int i = 7; i > 3; i--)
            {
                if (EnemyManager.Instance.PlayerCombatSpots[i] != null && !EnemyManager.Instance.PlayerCombatSpots[i].Dead)
                {
                    ClearTargets();
                    currentlySelectedTargets.Add(EnemyManager.Instance.PlayerCombatSpots[i]);
                    Target(currentlySelectedTargets[0]);
                    break;
                }
            }
        }
        else
        {
            if (currentlySelectedTargets[0].CombatSpot <= 3)
                column = 0;
            else
                column = 1;

            for (int i = currentlySelectedTargets[0].CombatSpot - 1; i > (4 * column) - 1; i--) // 4 * column helps with staying in a specific column
            {
                if (EnemyManager.Instance.PlayerCombatSpots[i] != null && !EnemyManager.Instance.PlayerCombatSpots[i].Dead)
                {
                    ClearTargets();
                    currentlySelectedTargets.Add(EnemyManager.Instance.PlayerCombatSpots[i]);
                    Target(currentlySelectedTargets[0]);
                    break;
                }
            }
        }
    }

    private void TrackPlayerDownTargeting()
    {
        //top of player column
        if (currentlySelectedTargets[0].CombatSpot == 3)
        {
            column = 0;
            for (int i = 0; i < 3; i--)
            {
                if (EnemyManager.Instance.PlayerCombatSpots[i] != null && !EnemyManager.Instance.PlayerCombatSpots[i].Dead)
                {
                    ClearTargets();
                    currentlySelectedTargets.Add(EnemyManager.Instance.PlayerCombatSpots[i]);
                    Target(currentlySelectedTargets[0]);
                    break;
                }
            }
        }
        //top of player summon column
        else if (currentlySelectedTargets[0].CombatSpot == 7)
        {
            column = 1;
            for (int i = 4; i < 7; i--)
            {
                if (EnemyManager.Instance.PlayerCombatSpots[i] != null && !EnemyManager.Instance.PlayerCombatSpots[i].Dead)
                {
                    ClearTargets();
                    currentlySelectedTargets.Add(EnemyManager.Instance.PlayerCombatSpots[i]);
                    Target(currentlySelectedTargets[0]);
                    break;
                }
            }
        }
        else
        {
            if (currentlySelectedTargets[0].CombatSpot <= 3)
                column = 0;
            else
                column = 1;

            for (int i = currentlySelectedTargets[0].CombatSpot + 1; i < (4 * column) + 4; i++) // 4 * column helps with staying in a specific column
            {
                if (EnemyManager.Instance.PlayerCombatSpots[i] != null && !EnemyManager.Instance.PlayerCombatSpots[i].Dead)
                {
                    ClearTargets();
                    currentlySelectedTargets.Add(EnemyManager.Instance.PlayerCombatSpots[i]);
                    Target(currentlySelectedTargets[0]);
                    break;
                }
            }
        }
    }

    private void TrackPlayerLeftTargeting()
    {
        //player column
        if (currentlySelectedTargets[0].CombatSpot <= 3)
        {
            column = 0;

            if (EnemyManager.Instance.PlayerCombatSpots[currentlySelectedTargets[0].CombatSpot + 4] != null && !EnemyManager.Instance.PlayerCombatSpots[currentlySelectedTargets[0].CombatSpot + 4].Dead)
            {
                Character tempCharacter = EnemyManager.Instance.PlayerCombatSpots[currentlySelectedTargets[0].CombatSpot + 4];
                ClearTargets();
                currentlySelectedTargets.Add(tempCharacter);
                Target(currentlySelectedTargets[0]);
            }
            else
            {
                int tempColumn = column;

                for (int i = 0; i < 2; i++)
                {
                    if (tempColumn == 0)
                    {
                        for (int j = 3; j < 7; j++)
                        {
                            if (EnemyManager.Instance.PlayerCombatSpots[j] != null && !EnemyManager.Instance.PlayerCombatSpots[j].Dead)
                            {
                                ClearTargets();
                                currentlySelectedTargets.Add(EnemyManager.Instance.PlayerCombatSpots[j]);
                                Target(currentlySelectedTargets[0]);
                                return;
                            }
                        }
                        tempColumn++;
                    }
                    else if (tempColumn == 1)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if (EnemyManager.Instance.PlayerCombatSpots[j] != null && !EnemyManager.Instance.PlayerCombatSpots[j].Dead)
                            {
                                ClearTargets();
                                currentlySelectedTargets.Add(EnemyManager.Instance.PlayerCombatSpots[j]);
                                Target(currentlySelectedTargets[0]);
                                return;
                            }
                        }
                        tempColumn++;
                    }
                }
            }
        }
        //player summon column
        else
        {
            column = 1;

            if (EnemyManager.Instance.PlayerCombatSpots[currentlySelectedTargets[0].CombatSpot - 4] != null && !EnemyManager.Instance.PlayerCombatSpots[currentlySelectedTargets[0].CombatSpot - 4].Dead)
            {
                Character tempCharacter = EnemyManager.Instance.PlayerCombatSpots[currentlySelectedTargets[0].CombatSpot - 4];
                ClearTargets();
                currentlySelectedTargets.Add(tempCharacter);
                Target(currentlySelectedTargets[0]);
            }
            else
            {
                int tempColumn = column;

                for (int i = 0; i < 2; i++)
                {
                    if (tempColumn == 0)
                    {
                        for (int j = 3; j < 7; j++)
                        {
                            if (EnemyManager.Instance.PlayerCombatSpots[j] != null && !EnemyManager.Instance.PlayerCombatSpots[j].Dead)
                            {
                                ClearTargets();
                                currentlySelectedTargets.Add(EnemyManager.Instance.PlayerCombatSpots[j]);
                                Target(currentlySelectedTargets[0]);
                                return;
                            }
                        }
                        tempColumn++;
                    }
                    else if (tempColumn == 1)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if (EnemyManager.Instance.PlayerCombatSpots[j] != null && !EnemyManager.Instance.PlayerCombatSpots[j].Dead)
                            {
                                ClearTargets();
                                currentlySelectedTargets.Add(EnemyManager.Instance.PlayerCombatSpots[j]);
                                Target(currentlySelectedTargets[0]);
                                return;
                            }
                        }
                        tempColumn++;
                    }
                }
            }
        }
    }

    private void TrackPlayerRightTargeting()
    {
        //player summon column
        if (currentlySelectedTargets[0].CombatSpot > 3)
        {
            column = 0;

            if (EnemyManager.Instance.PlayerCombatSpots[currentlySelectedTargets[0].CombatSpot - 4] != null && !EnemyManager.Instance.PlayerCombatSpots[currentlySelectedTargets[0].CombatSpot - 4].Dead)
            {
                Character tempCharacter = EnemyManager.Instance.PlayerCombatSpots[currentlySelectedTargets[0].CombatSpot - 4];
                ClearTargets();
                currentlySelectedTargets.Add(tempCharacter);
                Target(currentlySelectedTargets[0]);
            }
            else
            {
                int tempColumn = column;

                for (int i = 0; i < 2; i++)
                {
                    if (tempColumn == 0)
                    {
                        for (int j = 3; j < 7; j++)
                        {
                            if (EnemyManager.Instance.PlayerCombatSpots[j] != null && !EnemyManager.Instance.PlayerCombatSpots[j].Dead)
                            {
                                ClearTargets();
                                currentlySelectedTargets.Add(EnemyManager.Instance.PlayerCombatSpots[j]);
                                Target(currentlySelectedTargets[0]);
                                return;
                            }
                        }
                        tempColumn++;
                    }
                    else if (tempColumn == 1)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if (EnemyManager.Instance.PlayerCombatSpots[j] != null && !EnemyManager.Instance.PlayerCombatSpots[j].Dead)
                            {
                                ClearTargets();
                                currentlySelectedTargets.Add(EnemyManager.Instance.PlayerCombatSpots[j]);
                                Target(currentlySelectedTargets[0]);
                                return;
                            }
                        }
                        tempColumn++;
                    }
                }
            }
        }
        //player column
        else
        {
            column = 1;

            if (EnemyManager.Instance.PlayerCombatSpots[currentlySelectedTargets[0].CombatSpot + 4] != null && !EnemyManager.Instance.PlayerCombatSpots[currentlySelectedTargets[0].CombatSpot + 4].Dead)
            {
                Character tempCharacter = EnemyManager.Instance.PlayerCombatSpots[currentlySelectedTargets[0].CombatSpot + 4];
                ClearTargets();
                currentlySelectedTargets.Add(tempCharacter);
                Target(currentlySelectedTargets[0]);
            }
            else
            {
                int tempColumn = column;

                for (int i = 0; i < 2; i++)
                {
                    if (tempColumn == 0)
                    {
                        for (int j = 3; j < 7; j++)
                        {
                            if (EnemyManager.Instance.PlayerCombatSpots[j] != null && !EnemyManager.Instance.PlayerCombatSpots[j].Dead)
                            {
                                ClearTargets();
                                currentlySelectedTargets.Add(EnemyManager.Instance.PlayerCombatSpots[j]);
                                Target(currentlySelectedTargets[0]);
                                return;
                            }
                        }
                        tempColumn++;
                    }
                    else if (tempColumn == 1)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if (EnemyManager.Instance.PlayerCombatSpots[j] != null && !EnemyManager.Instance.PlayerCombatSpots[j].Dead)
                            {
                                ClearTargets();
                                currentlySelectedTargets.Add(EnemyManager.Instance.PlayerCombatSpots[j]);
                                Target(currentlySelectedTargets[0]);
                                return;
                            }
                        }
                        tempColumn++;
                    }
                }
            }
        }
    }
}
