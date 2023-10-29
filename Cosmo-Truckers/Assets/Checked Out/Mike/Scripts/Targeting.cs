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
    [SerializeField] Material negativeSelectedMaterial;
    [SerializeField] Material positiveSelectedMaterial;

    bool isTargeting = false;
    EnumManager.TargetingType currentTargetingType;
    BaseAttackSO currentAttack;
    bool initialSetup = false;
    bool targetingEnemies = true;
    bool targetingDead = false;
    List<Character> currentlySelectedTargets;
    int column = 0;
    int currentNumberOfTargets;
    int targets;

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
        currentNumberOfTargets = 0;
        targetingDead = attack.TargetsDead;
        if (currentTargetingType == EnumManager.TargetingType.Multi_Target_Choice)
            targets = attack.NumberOFTargets;
        else
            targets = 0;
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
        currentNumberOfTargets++;

        if(currentNumberOfTargets < targets)
        {
            ClearTargets();
            TargetReset();
        }
        else
        {
            foreach (Character combatSpot in currentlySelectedTargets)
            {
                CombatManager.Instance.CharactersSelected.Add(combatSpot);
            }

            if(CombatManager.Instance.GetCurrentPlayer)
                CombatManager.Instance.GetCurrentPlayer.PlayerAttackUI.HandleMana();
            CombatManager.Instance.TargetsSelected = true;
            isTargeting = false;
            currentNumberOfTargets = 0;
            ClearTargets();
            ClearEmptySlots();
            currentlySelectedTargets.Clear();
        }
    }

    private void CancelAttack()
    {
        CombatManager.Instance.GetCurrentPlayer.SetupAttackWheel();
        CombatManager.Instance.StopAllCoroutines();
        isTargeting = false;
        currentNumberOfTargets = 0;
        ClearTargets();
        ClearEmptySlots();
        currentlySelectedTargets.Clear();
    }

    private void Target(Character target)
    {
        foreach(SpriteRenderer renderer in target.TargetingSprites)
        {
            if(target.GetComponent<Enemy>())
            {
                if(currentAttack.EnemyPositiveEffect)
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
                if(currentAttack.FriendlyPositiveEffect)
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

    public void TargetEmptySlot(bool playerSide, int combatSpotIndex)
    {
        if(playerSide)
        {
            if(combatSpotIndex > 3) //in the summon layer
            {
                combatSpotIndex -= 4; // adjusts number down to be in range of summon indices

                if (currentAttack.FriendlyPositiveEffect)
                {
                    EnemyManager.Instance.PlayerSummonLocations[combatSpotIndex].GetComponent<SpriteRenderer>().color = new Color(EnemyManager.Instance.PlayerSummonLocations[combatSpotIndex].GetComponent<SpriteRenderer>().color.r, EnemyManager.Instance.PlayerSummonLocations[combatSpotIndex].GetComponent<SpriteRenderer>().color.g, EnemyManager.Instance.PlayerSummonLocations[combatSpotIndex].GetComponent<SpriteRenderer>().color.b, 1);
                    EnemyManager.Instance.PlayerSummonLocations[combatSpotIndex].GetComponent<SpriteRenderer>().material = positiveTargetMaterial;
                }
                else
                {
                    EnemyManager.Instance.PlayerSummonLocations[combatSpotIndex].GetComponent<SpriteRenderer>().color = new Color(EnemyManager.Instance.PlayerSummonLocations[combatSpotIndex].GetComponent<SpriteRenderer>().color.r, EnemyManager.Instance.PlayerSummonLocations[combatSpotIndex].GetComponent<SpriteRenderer>().color.g, EnemyManager.Instance.PlayerSummonLocations[combatSpotIndex].GetComponent<SpriteRenderer>().color.b, 1);
                    EnemyManager.Instance.PlayerSummonLocations[combatSpotIndex].GetComponent<SpriteRenderer>().material = negativeTargetMaterial;
                }
            }
            else
            {
                if (currentAttack.FriendlyPositiveEffect)
                {
                    EnemyManager.Instance.PlayerLocations[combatSpotIndex].GetComponent<SpriteRenderer>().color = new Color(EnemyManager.Instance.PlayerLocations[combatSpotIndex].GetComponent<SpriteRenderer>().color.r, EnemyManager.Instance.PlayerLocations[combatSpotIndex].GetComponent<SpriteRenderer>().color.g, EnemyManager.Instance.PlayerLocations[combatSpotIndex].GetComponent<SpriteRenderer>().color.b, 1);
                    EnemyManager.Instance.PlayerLocations[combatSpotIndex].GetComponent<SpriteRenderer>().material = positiveTargetMaterial;
                }
                else
                {
                    EnemyManager.Instance.PlayerLocations[combatSpotIndex].GetComponent<SpriteRenderer>().color = new Color(EnemyManager.Instance.PlayerLocations[combatSpotIndex].GetComponent<SpriteRenderer>().color.r, EnemyManager.Instance.PlayerLocations[combatSpotIndex].GetComponent<SpriteRenderer>().color.g, EnemyManager.Instance.PlayerLocations[combatSpotIndex].GetComponent<SpriteRenderer>().color.b, 1);
                    EnemyManager.Instance.PlayerLocations[combatSpotIndex].GetComponent<SpriteRenderer>().material = negativeTargetMaterial;
                }
            }
        }
        else
        {
            if(combatSpotIndex > 7) //in the summon layer
            {
                combatSpotIndex -= 8; // adjusts number down to be in range of summon indices

                if (currentAttack.EnemyPositiveEffect)
                {
                    EnemyManager.Instance.EnemySummonLocations[combatSpotIndex].GetComponent<SpriteRenderer>().color = new Color(EnemyManager.Instance.EnemySummonLocations[combatSpotIndex].GetComponent<SpriteRenderer>().color.r, EnemyManager.Instance.EnemySummonLocations[combatSpotIndex].GetComponent<SpriteRenderer>().color.g, EnemyManager.Instance.EnemySummonLocations[combatSpotIndex].GetComponent<SpriteRenderer>().color.b, 1);
                    EnemyManager.Instance.EnemySummonLocations[combatSpotIndex].GetComponent<SpriteRenderer>().material = positiveTargetMaterial;
                }
                else
                {
                    EnemyManager.Instance.EnemySummonLocations[combatSpotIndex].GetComponent<SpriteRenderer>().color = new Color(EnemyManager.Instance.EnemySummonLocations[combatSpotIndex].GetComponent<SpriteRenderer>().color.r, EnemyManager.Instance.EnemySummonLocations[combatSpotIndex].GetComponent<SpriteRenderer>().color.g, EnemyManager.Instance.EnemySummonLocations[combatSpotIndex].GetComponent<SpriteRenderer>().color.b, 1);
                    EnemyManager.Instance.EnemySummonLocations[combatSpotIndex].GetComponent<SpriteRenderer>().material = negativeTargetMaterial;
                }
            }
            else
            {
                if (currentAttack.EnemyPositiveEffect)
                {
                    EnemyManager.Instance.EnemyLocations[combatSpotIndex].GetComponent<SpriteRenderer>().color = new Color(EnemyManager.Instance.EnemyLocations[combatSpotIndex].GetComponent<SpriteRenderer>().color.r, EnemyManager.Instance.EnemyLocations[combatSpotIndex].GetComponent<SpriteRenderer>().color.g, EnemyManager.Instance.EnemyLocations[combatSpotIndex].GetComponent<SpriteRenderer>().color.b, 1);
                    EnemyManager.Instance.EnemyLocations[combatSpotIndex].GetComponent<SpriteRenderer>().material = positiveTargetMaterial;
                }
                else
                {
                    EnemyManager.Instance.EnemyLocations[combatSpotIndex].GetComponent<SpriteRenderer>().color = new Color(EnemyManager.Instance.EnemyLocations[combatSpotIndex].GetComponent<SpriteRenderer>().color.r, EnemyManager.Instance.EnemyLocations[combatSpotIndex].GetComponent<SpriteRenderer>().color.g, EnemyManager.Instance.EnemyLocations[combatSpotIndex].GetComponent<SpriteRenderer>().color.b, 1);
                    EnemyManager.Instance.EnemyLocations[combatSpotIndex].GetComponent<SpriteRenderer>().material = negativeTargetMaterial;
                }
            }
        }
    }

    private void MultiTarget(Character target)
    {
        foreach (SpriteRenderer renderer in target.TargetingSprites)
        {
            if (target.GetComponent<Enemy>())
            {
                if (currentAttack.EnemyPositiveEffect)
                {
                    renderer.material = positiveSelectedMaterial;
                }
                else
                {
                    renderer.material = negativeSelectedMaterial;
                }
            }
            else
            {
                if (currentAttack.FriendlyPositiveEffect)
                {
                    renderer.material = positiveSelectedMaterial;
                }
                else
                {
                    renderer.material = negativeSelectedMaterial;
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

        while(currentlySelectedTargets.Count > currentNumberOfTargets)
        {
            currentlySelectedTargets.Remove(currentlySelectedTargets[currentlySelectedTargets.Count - 1]);
        }

        foreach (Character character in currentlySelectedTargets)
            MultiTarget(character);
    }

    private void ClearEmptySlots()
    {
        foreach (Transform spot in EnemyManager.Instance.PlayerSummonLocations)
        {
            spot.GetComponent<SpriteRenderer>().color = new Color(spot.GetComponent<SpriteRenderer>().color.r, spot.GetComponent<SpriteRenderer>().color.g, spot.GetComponent<SpriteRenderer>().color.b, 0);
            spot.GetComponent<SpriteRenderer>().material = notTargetedMaterial;
        }

        foreach (Transform spot in EnemyManager.Instance.EnemySummonLocations)
        {
            spot.GetComponent<SpriteRenderer>().color = new Color(spot.GetComponent<SpriteRenderer>().color.r, spot.GetComponent<SpriteRenderer>().color.g, spot.GetComponent<SpriteRenderer>().color.b, 0);

            spot.GetComponent<SpriteRenderer>().material = notTargetedMaterial;
        }

        foreach (Transform spot in EnemyManager.Instance.PlayerLocations)
        {
            spot.GetComponent<SpriteRenderer>().color = new Color(spot.GetComponent<SpriteRenderer>().color.r, spot.GetComponent<SpriteRenderer>().color.g, spot.GetComponent<SpriteRenderer>().color.b, 0);
            spot.GetComponent<SpriteRenderer>().material = notTargetedMaterial;
        }


        foreach (Transform spot in EnemyManager.Instance.EnemyLocations)
        {
            spot.GetComponent<SpriteRenderer>().color = new Color(spot.GetComponent<SpriteRenderer>().color.r, spot.GetComponent<SpriteRenderer>().color.g, spot.GetComponent<SpriteRenderer>().color.b, 0);
            spot.GetComponent<SpriteRenderer>().material = notTargetedMaterial;
        }
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
        if (currentAttack.CanTargetEnemies && currentAttack.CanTargetFriendly)
        {
            //set initial target (start enemy side)
            if (!initialSetup)
            {
                initialSetup = true;
                targetingEnemies = true;
                foreach(Character enemy in EnemyManager.Instance.EnemyCombatSpots)
                {
                    if(((!enemy.Dead && !targetingDead) || (enemy.Dead && targetingDead)))
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

                TargetReset();
            }

            if (targetingEnemies)
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
        else if (currentAttack.CanTargetEnemies)
        {
            //set initial target (only enemy side)
            if (!initialSetup)
            {
                initialSetup = true;
                targetingEnemies = true;
                foreach (Character enemy in EnemyManager.Instance.EnemyCombatSpots)
                {
                    if (enemy != null && ((!enemy.Dead && !targetingDead) || (enemy.Dead && targetingDead)) && !currentlySelectedTargets.Contains(enemy))
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
        else if (currentAttack.CanTargetFriendly)
        {
            //set initial target (only player side)
            if (!initialSetup)
            {
                initialSetup = true;
                targetingEnemies = false;
                foreach (Character player in EnemyManager.Instance.PlayerCombatSpots)
                {
                    if ((!player.Dead || ((!player.Dead && !targetingDead) || (player.Dead && targetingDead)) && !currentlySelectedTargets.Contains(player)))
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

    private void TargetReset()
    {
        if (targetingEnemies)
        {
            foreach (Character enemy in EnemyManager.Instance.EnemyCombatSpots)
            {
                if (enemy != null && ((!enemy.Dead && !targetingDead) || (enemy.Dead && targetingDead)) && !currentlySelectedTargets.Contains(enemy))
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
                if (player != null && ((!player.Dead && !targetingDead) || (player.Dead && targetingDead)) && !currentlySelectedTargets.Contains(player))
                {
                    ClearTargets();
                    currentlySelectedTargets.Add(player);
                    Target(player);
                    break;
                }
            }
        }
    }

    private void TrackMultiTargetConeInput()
    {

    }

    private void TrackMultiTargetChoice()
    {
        TrackSingleTargetInput();
    }

    private void TrackAEOTargetInput()
    {
        //set initial target
        if (!initialSetup)
        {
            initialSetup = true;

            //Target enemies
            if (currentAttack.CanTargetEnemies)
            {
                foreach (Character enemy in EnemyManager.Instance.EnemyCombatSpots)
                {
                    if (enemy != null && ((!enemy.Dead && !targetingDead) || (enemy.Dead && targetingDead)))
                    {
                        currentlySelectedTargets.Add(enemy);
                        Target(enemy);
                    }
                }
            }

            //Target players
            else if (currentAttack.CanTargetFriendly)
            {
                foreach (Character player in EnemyManager.Instance.PlayerCombatSpots)
                {
                    if (player != null && ((!player.Dead && !targetingDead) || (player.Dead && targetingDead)))
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
        //set initial target
        if (!initialSetup)
        {
            initialSetup = true;

            //Target enemies
            foreach (Character enemy in EnemyManager.Instance.EnemyCombatSpots)
            {
                if (enemy != null && ((!enemy.Dead && !targetingDead) || (enemy.Dead && targetingDead)))
                {
                    currentlySelectedTargets.Add(enemy);
                    Target(enemy);
                }
            }

            foreach (Character player in EnemyManager.Instance.PlayerCombatSpots)
            {
                if (player != null && ((!player.Dead && !targetingDead) || (player.Dead && targetingDead)))
                {
                    currentlySelectedTargets.Add(player);
                    Target(player);
                }
            }
        }

        //track choice
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ReactivateCombatManager();
        }
    }

    private void TrackEnemyUpTargeting()
    {
        //top of enemy column back
        if (currentlySelectedTargets[currentNumberOfTargets].CombatSpot == FindMinSlot(column, false))
        {
            column = 0;
            for (int i = 3; i > 0; i--)
            {
                if (EnemyManager.Instance.EnemyCombatSpots[i] != null && ((!EnemyManager.Instance.EnemyCombatSpots[i].Dead && !targetingDead) || (EnemyManager.Instance.EnemyCombatSpots[i].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.EnemyCombatSpots[i]))
                {
                    ClearTargets();
                    currentlySelectedTargets.Add(EnemyManager.Instance.EnemyCombatSpots[i]);
                    Target(currentlySelectedTargets[currentNumberOfTargets]);
                    break;
                }
            }
        }
        //top of enemy column front
        else if (currentlySelectedTargets[currentNumberOfTargets].CombatSpot == FindMinSlot(column, false))
        {
            column = 1;
            for (int i = 7; i > 3; i--)
            {
                if (EnemyManager.Instance.EnemyCombatSpots[i] != null && ((!EnemyManager.Instance.EnemyCombatSpots[i].Dead && !targetingDead) || (EnemyManager.Instance.EnemyCombatSpots[i].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.EnemyCombatSpots[i]))
                {
                    ClearTargets();
                    currentlySelectedTargets.Add(EnemyManager.Instance.EnemyCombatSpots[i]);
                    Target(currentlySelectedTargets[currentNumberOfTargets]);
                    break;
                }
            }
        }
        //top of enemy summon column
        else if (currentlySelectedTargets[0].CombatSpot == FindMinSlot(column, false))
        {
            column = 2;
            for (int i = 11; i > 7; i--)
            {
                if (EnemyManager.Instance.EnemyCombatSpots[i] != null && ((!EnemyManager.Instance.EnemyCombatSpots[i].Dead && !targetingDead) || (EnemyManager.Instance.EnemyCombatSpots[i].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.EnemyCombatSpots[i]))
                {
                    ClearTargets();
                    currentlySelectedTargets.Add(EnemyManager.Instance.EnemyCombatSpots[i]);
                    Target(currentlySelectedTargets[currentNumberOfTargets]);
                    break;
                }
            }
        }
        //anywhere but the top of a column
        else
        {
            if (currentlySelectedTargets[currentNumberOfTargets].CombatSpot <= 3)
                column = 0;
            else if (currentlySelectedTargets[currentNumberOfTargets].CombatSpot <= 7)
                column = 1;
            else
                column = 2;

            for (int i = currentlySelectedTargets[currentNumberOfTargets].CombatSpot - 1; i > (4 * column) - 1; i--) // 4 * column helps with staying in a specific column
            {
                if (EnemyManager.Instance.EnemyCombatSpots[i] != null && ((!EnemyManager.Instance.EnemyCombatSpots[i].Dead && !targetingDead) || (EnemyManager.Instance.EnemyCombatSpots[i].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.EnemyCombatSpots[i]))
                {
                    ClearTargets();
                    currentlySelectedTargets.Add(EnemyManager.Instance.EnemyCombatSpots[i]);
                    Target(currentlySelectedTargets[currentNumberOfTargets]);
                    break;
                }
            }
        }
    }

    private void TrackEnemyDownTargeting()
    {
        //bottom of enemy column back
        if (currentlySelectedTargets[currentNumberOfTargets].CombatSpot == FindMaxSlot(column, false))
        {
            column = 0;
            for (int i = 0; i < 3; i++)
            {
                if (EnemyManager.Instance.EnemyCombatSpots[i] != null && ((!EnemyManager.Instance.EnemyCombatSpots[i].Dead && !targetingDead) || (EnemyManager.Instance.EnemyCombatSpots[i].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.EnemyCombatSpots[i]))
                {
                    ClearTargets();
                    currentlySelectedTargets.Add(EnemyManager.Instance.EnemyCombatSpots[i]);
                    Target(currentlySelectedTargets[currentNumberOfTargets]);
                    break;
                }
            }
        }
        //bottom of enemy column front
        else if (currentlySelectedTargets[currentNumberOfTargets].CombatSpot == FindMaxSlot(column, false))
        {
            column = 1;
            for (int i = 4; i < 7; i++)
            {
                if (EnemyManager.Instance.EnemyCombatSpots[i] != null && ((!EnemyManager.Instance.EnemyCombatSpots[i].Dead && !targetingDead) || (EnemyManager.Instance.EnemyCombatSpots[i].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.EnemyCombatSpots[i]))
                {
                    ClearTargets();
                    currentlySelectedTargets.Add(EnemyManager.Instance.EnemyCombatSpots[i]);
                    Target(currentlySelectedTargets[currentNumberOfTargets]);
                    break;
                }
            }
        }
        //bottom of enemy summon column
        else if (currentlySelectedTargets[currentNumberOfTargets].CombatSpot == FindMaxSlot(column, false))
        {
            column = 2;
            for (int i = 8; i < 11; i++)
            {
                if (EnemyManager.Instance.EnemyCombatSpots[i] != null && ((!EnemyManager.Instance.EnemyCombatSpots[i].Dead && !targetingDead) || (EnemyManager.Instance.EnemyCombatSpots[i].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.EnemyCombatSpots[i]))
                {
                    ClearTargets();
                    currentlySelectedTargets.Add(EnemyManager.Instance.EnemyCombatSpots[i]);
                    Target(currentlySelectedTargets[currentNumberOfTargets]);
                    break;
                }
            }
        }
        //anywhere but the top of a column
        else
        {
            if (currentlySelectedTargets[currentNumberOfTargets].CombatSpot <= 3)
                column = 0;
            else if (currentlySelectedTargets[currentNumberOfTargets].CombatSpot <= 7)
                column = 1;
            else
                column = 2;

            for (int i = currentlySelectedTargets[currentNumberOfTargets].CombatSpot + 1; i < (4 * column) + 4; i++) // 4 * column helps with staying in a specific column
            {
                if (EnemyManager.Instance.EnemyCombatSpots[i] != null && ((!EnemyManager.Instance.EnemyCombatSpots[i].Dead && !targetingDead) || (EnemyManager.Instance.EnemyCombatSpots[i].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.EnemyCombatSpots[i]))
                {
                    ClearTargets();
                    currentlySelectedTargets.Add(EnemyManager.Instance.EnemyCombatSpots[i]);
                    Target(currentlySelectedTargets[currentNumberOfTargets]);
                    break;
                }
            }
        }
    }

    private void TrackEnemyLeftTargeting()
    {
        //enemy summon column
        if (currentlySelectedTargets[currentNumberOfTargets].CombatSpot >= 8)
        {
            column = 2;

            if (EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[currentNumberOfTargets].CombatSpot - 8] != null && ((!EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[currentNumberOfTargets].CombatSpot - 8].Dead && !targetingDead) || (EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[currentNumberOfTargets].CombatSpot - 8].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[currentNumberOfTargets].CombatSpot - 8]))
            {
                Character tempCharacter = EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[currentNumberOfTargets].CombatSpot - 8];
                ClearTargets();
                currentlySelectedTargets.Add(tempCharacter);
                Target(currentlySelectedTargets[currentNumberOfTargets]);
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
                            if (EnemyManager.Instance.EnemyCombatSpots[j] != null && ((!EnemyManager.Instance.EnemyCombatSpots[j].Dead && !targetingDead) || (EnemyManager.Instance.EnemyCombatSpots[j].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.EnemyCombatSpots[j]))
                            {
                                ClearTargets();
                                currentlySelectedTargets.Add(EnemyManager.Instance.EnemyCombatSpots[j]);
                                Target(currentlySelectedTargets[currentNumberOfTargets]);
                                return;
                            }
                        }
                        tempColumn++;
                    }
                    else if (tempColumn == 1)
                    {
                        for (int j = 8; j < 11; j++)
                        {
                            if (EnemyManager.Instance.EnemyCombatSpots[j] != null && ((!EnemyManager.Instance.EnemyCombatSpots[j].Dead && !targetingDead) || (EnemyManager.Instance.EnemyCombatSpots[j].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.EnemyCombatSpots[j]))
                            {
                                ClearTargets();
                                currentlySelectedTargets.Add(EnemyManager.Instance.EnemyCombatSpots[j]);
                                Target(currentlySelectedTargets[currentNumberOfTargets]);
                                return;
                            }
                        }
                        tempColumn++;
                    }
                    else
                    {
                        for (int j = 4; j < 7; j++)
                        {
                            if (EnemyManager.Instance.EnemyCombatSpots[j] != null && ((!EnemyManager.Instance.EnemyCombatSpots[j].Dead && !targetingDead) || (EnemyManager.Instance.EnemyCombatSpots[j].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.EnemyCombatSpots[j]))
                            {
                                ClearTargets();
                                currentlySelectedTargets.Add(EnemyManager.Instance.EnemyCombatSpots[j]);
                                Target(currentlySelectedTargets[currentNumberOfTargets]);
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
            if (currentlySelectedTargets[currentNumberOfTargets].CombatSpot <= 3)
                column = 0;
            else
                column = 1;

            if (EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[currentNumberOfTargets].CombatSpot + 4] != null && ((!EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[currentNumberOfTargets].CombatSpot + 4].Dead && !targetingDead) || (EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[currentNumberOfTargets].CombatSpot + 4].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[currentNumberOfTargets].CombatSpot + 4]))
            {
                Character tempCharacter = EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[currentNumberOfTargets].CombatSpot + 4];
                ClearTargets();
                currentlySelectedTargets.Add(tempCharacter);
                Target(currentlySelectedTargets[currentNumberOfTargets]);
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
                            if (EnemyManager.Instance.EnemyCombatSpots[j] != null && ((!EnemyManager.Instance.EnemyCombatSpots[j].Dead && !targetingDead) || (EnemyManager.Instance.EnemyCombatSpots[j].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.EnemyCombatSpots[j]))
                            {
                                ClearTargets();
                                currentlySelectedTargets.Add(EnemyManager.Instance.EnemyCombatSpots[j]);
                                Target(currentlySelectedTargets[currentNumberOfTargets]);
                                return;
                            }
                        }
                        tempColumn -= 2;
                    }
                    else if (tempColumn == 1)
                    {
                        for (int j = 8; j < 11; j++)
                        {
                            if (EnemyManager.Instance.EnemyCombatSpots[j] != null && ((!EnemyManager.Instance.EnemyCombatSpots[j].Dead && !targetingDead) || (EnemyManager.Instance.EnemyCombatSpots[j].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.EnemyCombatSpots[j]))
                            {
                                ClearTargets();
                                currentlySelectedTargets.Add(EnemyManager.Instance.EnemyCombatSpots[j]);
                                Target(currentlySelectedTargets[currentNumberOfTargets]);
                                return;
                            }
                        }
                        tempColumn++;
                    }
                    else
                    {
                        for (int j = 4; j < 7; j++)
                        {
                            if (EnemyManager.Instance.EnemyCombatSpots[j] != null && ((!EnemyManager.Instance.EnemyCombatSpots[j].Dead && !targetingDead) || (EnemyManager.Instance.EnemyCombatSpots[j].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.EnemyCombatSpots[j]))
                            {
                                ClearTargets();
                                currentlySelectedTargets.Add(EnemyManager.Instance.EnemyCombatSpots[j]);
                                Target(currentlySelectedTargets[currentNumberOfTargets]);
                                return;
                            }
                        }
                        tempColumn++;
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

            if (EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[0].CombatSpot + 8] != null && ((!EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[0].CombatSpot + 8].Dead && !targetingDead) || (EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[0].CombatSpot + 8].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[0].CombatSpot + 8]))
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
                            if (EnemyManager.Instance.EnemyCombatSpots[j] != null && ((!EnemyManager.Instance.EnemyCombatSpots[j].Dead && !targetingDead) || (EnemyManager.Instance.EnemyCombatSpots[j].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.EnemyCombatSpots[j]))
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
                            if (EnemyManager.Instance.EnemyCombatSpots[j] != null && ((!EnemyManager.Instance.EnemyCombatSpots[j].Dead && !targetingDead) || (EnemyManager.Instance.EnemyCombatSpots[j].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.EnemyCombatSpots[j]))
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
                            if (EnemyManager.Instance.EnemyCombatSpots[j] != null && ((!EnemyManager.Instance.EnemyCombatSpots[j].Dead && !targetingDead) || (EnemyManager.Instance.EnemyCombatSpots[j].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.EnemyCombatSpots[j]))
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
            if (currentlySelectedTargets[0].CombatSpot <= 7)
                column = 1;
            else
                column = 2;

            if (EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[0].CombatSpot - 4] != null && ((!EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[0].CombatSpot - 4].Dead && !targetingDead) || (EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[0].CombatSpot - 4].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[0].CombatSpot - 4]))
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
                            if (EnemyManager.Instance.EnemyCombatSpots[j] != null && ((!EnemyManager.Instance.EnemyCombatSpots[j].Dead && !targetingDead) || (EnemyManager.Instance.EnemyCombatSpots[j].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.EnemyCombatSpots[j]))
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
                            if (EnemyManager.Instance.EnemyCombatSpots[j] != null && ((!EnemyManager.Instance.EnemyCombatSpots[j].Dead && !targetingDead) || (EnemyManager.Instance.EnemyCombatSpots[j].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.EnemyCombatSpots[j]))
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
                            if (EnemyManager.Instance.EnemyCombatSpots[j] != null && ((!EnemyManager.Instance.EnemyCombatSpots[j].Dead && !targetingDead) || (EnemyManager.Instance.EnemyCombatSpots[j].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.EnemyCombatSpots[j]))
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
        if (currentlySelectedTargets[currentNumberOfTargets].CombatSpot == FindMinSlot(column, true))
        {
            column = 0;
            for (int i = 3; i > 0; i--)
            {
                if (EnemyManager.Instance.PlayerCombatSpots[i] != null && ((!EnemyManager.Instance.PlayerCombatSpots[i].Dead && !targetingDead) || (EnemyManager.Instance.PlayerCombatSpots[i].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.PlayerCombatSpots[i]))
                {
                    ClearTargets();
                    currentlySelectedTargets.Add(EnemyManager.Instance.PlayerCombatSpots[i]);
                    Target(currentlySelectedTargets[currentNumberOfTargets]);
                    break;
                }
            }
        }
        //top of player summon column
        else if (currentlySelectedTargets[currentNumberOfTargets].CombatSpot == FindMinSlot(column, true))
        {
            column = 1;
            for (int i = 7; i > 3; i--)
            {
                if (EnemyManager.Instance.PlayerCombatSpots[i] != null && ((!EnemyManager.Instance.PlayerCombatSpots[i].Dead && !targetingDead) || (EnemyManager.Instance.PlayerCombatSpots[i].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.PlayerCombatSpots[i]))
                {
                    ClearTargets();
                    currentlySelectedTargets.Add(EnemyManager.Instance.PlayerCombatSpots[i]);
                    Target(currentlySelectedTargets[currentNumberOfTargets]);
                    break;
                }
            }
        }
        else
        {
            if (currentlySelectedTargets[currentNumberOfTargets].CombatSpot <= 3)
                column = 0;
            else
                column = 1;

            for (int i = currentlySelectedTargets[currentNumberOfTargets].CombatSpot - 1; i > (4 * column) - 1; i--) // 4 * column helps with staying in a specific column
            {
                if (EnemyManager.Instance.PlayerCombatSpots[i] != null && ((!EnemyManager.Instance.PlayerCombatSpots[i].Dead && !targetingDead) || (EnemyManager.Instance.PlayerCombatSpots[i].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.PlayerCombatSpots[i]))
                {
                    ClearTargets();
                    currentlySelectedTargets.Add(EnemyManager.Instance.PlayerCombatSpots[i]);
                    Target(currentlySelectedTargets[currentNumberOfTargets]);
                    break;
                }
            }
        }
    }

    private void TrackPlayerDownTargeting()
    {
        if (currentlySelectedTargets[currentNumberOfTargets].CombatSpot <= 3)
            column = 0;
        else
            column = 1;

        //top of player column
        if (currentlySelectedTargets[currentNumberOfTargets].CombatSpot == FindMaxSlot(column, true))
        {
            for (int i = 0; i < 3; i++)
            {
                if (EnemyManager.Instance.PlayerCombatSpots[i] != null && ((!EnemyManager.Instance.PlayerCombatSpots[i].Dead && !targetingDead) || (EnemyManager.Instance.PlayerCombatSpots[i].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.PlayerCombatSpots[i]))
                {
                    ClearTargets();
                    currentlySelectedTargets.Add(EnemyManager.Instance.PlayerCombatSpots[i]);
                    Target(currentlySelectedTargets[currentNumberOfTargets]);
                    break;
                }
            }
        }
        //top of player summon column
        else if (currentlySelectedTargets[currentNumberOfTargets].CombatSpot == FindMaxSlot(column, true))
        {
            for (int i = 4; i < 7; i++)
            {
                if (EnemyManager.Instance.PlayerCombatSpots[i] != null && ((!EnemyManager.Instance.PlayerCombatSpots[i].Dead && !targetingDead) || (EnemyManager.Instance.PlayerCombatSpots[i].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.EnemyCombatSpots[i]))
                {
                    ClearTargets();
                    currentlySelectedTargets.Add(EnemyManager.Instance.PlayerCombatSpots[i]);
                    Target(currentlySelectedTargets[currentNumberOfTargets]);
                    break;
                }
            }
        }
        else
        {
            for (int i = currentlySelectedTargets[currentNumberOfTargets].CombatSpot + 1; i < (4 * column) + 4; i++) // 4 * column helps with staying in a specific column
            {
                if (EnemyManager.Instance.PlayerCombatSpots[i] != null && ((!EnemyManager.Instance.PlayerCombatSpots[i].Dead && !targetingDead) || (EnemyManager.Instance.PlayerCombatSpots[i].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.EnemyCombatSpots[i]))
                {
                    ClearTargets();
                    currentlySelectedTargets.Add(EnemyManager.Instance.PlayerCombatSpots[i]);
                    Target(currentlySelectedTargets[currentNumberOfTargets]);
                    break;
                }
            }
        }
    }

    private void TrackPlayerLeftTargeting()
    {
        //player column
        if (currentlySelectedTargets[currentNumberOfTargets].CombatSpot <= 3)
        {
            column = 0;

            if (EnemyManager.Instance.PlayerCombatSpots[currentlySelectedTargets[currentNumberOfTargets].CombatSpot + 4] != null && ((!EnemyManager.Instance.PlayerCombatSpots[currentlySelectedTargets[currentNumberOfTargets].CombatSpot + 4].Dead && !targetingDead) || (EnemyManager.Instance.PlayerCombatSpots[currentlySelectedTargets[currentNumberOfTargets].CombatSpot + 4].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.PlayerCombatSpots[currentlySelectedTargets[currentNumberOfTargets].CombatSpot + 4]))
            {
                Character tempCharacter = EnemyManager.Instance.PlayerCombatSpots[currentlySelectedTargets[currentNumberOfTargets].CombatSpot + 4];
                ClearTargets();
                currentlySelectedTargets.Add(tempCharacter);
                Target(currentlySelectedTargets[currentNumberOfTargets]);
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
                            if (EnemyManager.Instance.PlayerCombatSpots[j] != null && ((!EnemyManager.Instance.PlayerCombatSpots[j].Dead && !targetingDead) || (EnemyManager.Instance.PlayerCombatSpots[j].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.PlayerCombatSpots[j]))
                            {
                                ClearTargets();
                                currentlySelectedTargets.Add(EnemyManager.Instance.PlayerCombatSpots[j]);
                                Target(currentlySelectedTargets[currentNumberOfTargets]);
                                return;
                            }
                        }
                        tempColumn++;
                    }
                    else if (tempColumn == 1)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if (EnemyManager.Instance.PlayerCombatSpots[j] != null && ((!EnemyManager.Instance.PlayerCombatSpots[j].Dead && !targetingDead) || (EnemyManager.Instance.PlayerCombatSpots[j].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.PlayerCombatSpots[j]))
                            {
                                ClearTargets();
                                currentlySelectedTargets.Add(EnemyManager.Instance.PlayerCombatSpots[j]);
                                Target(currentlySelectedTargets[currentNumberOfTargets]);
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

            if (EnemyManager.Instance.PlayerCombatSpots[currentlySelectedTargets[currentNumberOfTargets].CombatSpot - 4] != null && ((!EnemyManager.Instance.PlayerCombatSpots[currentlySelectedTargets[currentNumberOfTargets].CombatSpot - 4].Dead && !targetingDead) || (EnemyManager.Instance.PlayerCombatSpots[currentlySelectedTargets[currentNumberOfTargets].CombatSpot - 4].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.PlayerCombatSpots[currentlySelectedTargets[currentNumberOfTargets].CombatSpot - 4]))
            {
                Character tempCharacter = EnemyManager.Instance.PlayerCombatSpots[currentlySelectedTargets[currentNumberOfTargets].CombatSpot - 4];
                ClearTargets();
                currentlySelectedTargets.Add(tempCharacter);
                Target(currentlySelectedTargets[currentNumberOfTargets]);
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
                            if (EnemyManager.Instance.PlayerCombatSpots[j] != null && ((!EnemyManager.Instance.PlayerCombatSpots[j].Dead && !targetingDead) || (EnemyManager.Instance.PlayerCombatSpots[j].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.PlayerCombatSpots[j]))
                            {
                                ClearTargets();
                                currentlySelectedTargets.Add(EnemyManager.Instance.PlayerCombatSpots[j]);
                                Target(currentlySelectedTargets[currentNumberOfTargets]);
                                return;
                            }
                        }
                        tempColumn++;
                    }
                    else if (tempColumn == 1)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if (EnemyManager.Instance.PlayerCombatSpots[j] != null && ((!EnemyManager.Instance.PlayerCombatSpots[j].Dead && !targetingDead) || (EnemyManager.Instance.PlayerCombatSpots[j].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.PlayerCombatSpots[j]))
                            {
                                ClearTargets();
                                currentlySelectedTargets.Add(EnemyManager.Instance.PlayerCombatSpots[j]);
                                Target(currentlySelectedTargets[currentNumberOfTargets]);
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
        if (currentlySelectedTargets[currentNumberOfTargets].CombatSpot > 3)
        {
            column = 0;

            if (EnemyManager.Instance.PlayerCombatSpots[currentlySelectedTargets[currentNumberOfTargets].CombatSpot - 4] != null && ((!EnemyManager.Instance.PlayerCombatSpots[currentlySelectedTargets[currentNumberOfTargets].CombatSpot - 4].Dead && !targetingDead) || (EnemyManager.Instance.PlayerCombatSpots[currentlySelectedTargets[currentNumberOfTargets].CombatSpot - 4].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.PlayerCombatSpots[currentlySelectedTargets[currentNumberOfTargets].CombatSpot - 4]))
            {
                Character tempCharacter = EnemyManager.Instance.PlayerCombatSpots[currentlySelectedTargets[currentNumberOfTargets].CombatSpot - 4];
                ClearTargets();
                currentlySelectedTargets.Add(tempCharacter);
                Target(currentlySelectedTargets[currentNumberOfTargets]);
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
                            if (EnemyManager.Instance.PlayerCombatSpots[j] != null && ((!EnemyManager.Instance.PlayerCombatSpots[j].Dead && !targetingDead) || (EnemyManager.Instance.PlayerCombatSpots[j].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.PlayerCombatSpots[j]))
                            {
                                ClearTargets();
                                currentlySelectedTargets.Add(EnemyManager.Instance.PlayerCombatSpots[j]);
                                Target(currentlySelectedTargets[currentNumberOfTargets]);
                                return;
                            }
                        }
                        tempColumn++;
                    }
                    else if (tempColumn == 1)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if (EnemyManager.Instance.PlayerCombatSpots[j] != null && ((!EnemyManager.Instance.PlayerCombatSpots[j].Dead && !targetingDead) || (EnemyManager.Instance.PlayerCombatSpots[j].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.PlayerCombatSpots[j]))
                            {
                                ClearTargets();
                                currentlySelectedTargets.Add(EnemyManager.Instance.PlayerCombatSpots[j]);
                                Target(currentlySelectedTargets[currentNumberOfTargets]);
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

            if (EnemyManager.Instance.PlayerCombatSpots[currentlySelectedTargets[currentNumberOfTargets].CombatSpot + 4] != null && ((!EnemyManager.Instance.PlayerCombatSpots[currentlySelectedTargets[currentNumberOfTargets].CombatSpot + 4].Dead && !targetingDead) || (EnemyManager.Instance.PlayerCombatSpots[currentlySelectedTargets[currentNumberOfTargets].CombatSpot + 4].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.PlayerCombatSpots[currentlySelectedTargets[currentNumberOfTargets].CombatSpot + 4]))
            {
                Character tempCharacter = EnemyManager.Instance.PlayerCombatSpots[currentlySelectedTargets[currentNumberOfTargets].CombatSpot + 4];
                ClearTargets();
                currentlySelectedTargets.Add(tempCharacter);
                Target(currentlySelectedTargets[currentNumberOfTargets]);
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
                            if (EnemyManager.Instance.PlayerCombatSpots[j] != null && ((!EnemyManager.Instance.PlayerCombatSpots[j].Dead && !targetingDead) || (EnemyManager.Instance.PlayerCombatSpots[j].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.PlayerCombatSpots[j]))
                            {
                                ClearTargets();
                                currentlySelectedTargets.Add(EnemyManager.Instance.PlayerCombatSpots[j]);
                                Target(currentlySelectedTargets[currentNumberOfTargets]);
                                return;
                            }
                        }
                        tempColumn++;
                    }
                    else if (tempColumn == 1)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if (EnemyManager.Instance.PlayerCombatSpots[j] != null && ((!EnemyManager.Instance.PlayerCombatSpots[j].Dead && !targetingDead) || (EnemyManager.Instance.PlayerCombatSpots[j].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.PlayerCombatSpots[j]))
                            {
                                ClearTargets();
                                currentlySelectedTargets.Add(EnemyManager.Instance.PlayerCombatSpots[j]);
                                Target(currentlySelectedTargets[currentNumberOfTargets]);
                                return;
                            }
                        }
                        tempColumn++;
                    }
                }
            }
        }
    }

    private int FindMinSlot(int column, bool player)
    {
        Character tempCharacter = currentlySelectedTargets[currentlySelectedTargets.Count - 1];
        currentlySelectedTargets.Remove(currentlySelectedTargets[currentlySelectedTargets.Count - 1]);

        for (int i = (4 * column); i < 3 + (4 * column); i++) //iterates through all characters in a give column
        {
            if(player)
            {
                if (EnemyManager.Instance.PlayerCombatSpots[i] != null && ((!EnemyManager.Instance.PlayerCombatSpots[i].Dead && !targetingDead) || (EnemyManager.Instance.PlayerCombatSpots[i].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.PlayerCombatSpots[i]))
                {
                    currentlySelectedTargets.Add(tempCharacter);
                    return EnemyManager.Instance.PlayerCombatSpots[i].CombatSpot;
                }
            }
            else
            {
                if (EnemyManager.Instance.EnemyCombatSpots[i] != null && ((!EnemyManager.Instance.PlayerCombatSpots[i].Dead && !targetingDead) || (EnemyManager.Instance.PlayerCombatSpots[i].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.EnemyCombatSpots[i]))
                {
                    currentlySelectedTargets.Add(tempCharacter);
                    return EnemyManager.Instance.EnemyCombatSpots[i].CombatSpot;
                }
            }
        }

        currentlySelectedTargets.Add(tempCharacter);
        return 0;
    }

    private int FindMaxSlot(int column, bool player)
    {
        Character tempCharacter = currentlySelectedTargets[currentlySelectedTargets.Count - 1];
        currentlySelectedTargets.Remove(currentlySelectedTargets[currentlySelectedTargets.Count - 1]);

        for (int i = 3 + (4 * column); i > (4 * column); i--) //iterates through all characters in a give column
        {
            if (player)
            {
                if (EnemyManager.Instance.PlayerCombatSpots[i] != null && ((!EnemyManager.Instance.PlayerCombatSpots[i].Dead && !targetingDead) || (EnemyManager.Instance.PlayerCombatSpots[i].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.PlayerCombatSpots[i]))
                {
                    currentlySelectedTargets.Add(tempCharacter);
                    return EnemyManager.Instance.PlayerCombatSpots[i].CombatSpot;
                }
            }
            else
            {
                if (EnemyManager.Instance.EnemyCombatSpots[i] != null && ((!EnemyManager.Instance.PlayerCombatSpots[i].Dead && !targetingDead) || (EnemyManager.Instance.PlayerCombatSpots[i].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.EnemyCombatSpots[i]))
                {
                    currentlySelectedTargets.Add(tempCharacter);
                    return EnemyManager.Instance.EnemyCombatSpots[i].CombatSpot;
                }
            }
        }

        currentlySelectedTargets.Add(tempCharacter);
        return 0;
    }
}
