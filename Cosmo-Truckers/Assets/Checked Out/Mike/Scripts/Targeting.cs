using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Targeting : MonoBehaviour
{
    [Header("Targeting Materials")]
    [SerializeField] Material positiveTargetMaterial; //Flashing green
    [SerializeField] Material negativeTargetMaterial; //Flashing red
    [SerializeField] Material notTargetedMaterial; //default
    [SerializeField] Material enemyTargetingMaterial; //Flashing white
    [SerializeField] Material negativeSelectedMaterial; //Solid red
    [SerializeField] Material positiveSelectedMaterial; //Solid green

    [Space(20)]
    [Header("Attack Description")]
    [SerializeField] float staticTime = .1f;

    bool isTargeting = false;
    public EnumManager.TargetingType CurrentTargetingType;
    [HideInInspector] public Character ForcedTarget;
    BaseAttackSO currentAttack;
    public bool InitialSetup = false;
    bool targetingEnemies = true;
    bool targetingDead = false;
    bool enemyAttacking = false;
    bool checkingEnemyItentions = false;
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
        InitialSetup = false;
        CurrentTargetingType = attack.TargetingType;
        currentAttack = attack;
        currentlySelectedTargets.Clear();
        currentNumberOfTargets = 0;
        targetingDead = attack.TargetsDead;
        if (CurrentTargetingType == EnumManager.TargetingType.Multi_Target_Choice)
             targets = MaxNumberOfTargets(attack);
        else
            targets = 0;
    }

    public void StartCheckingEnemyIntentions()
    {
        isTargeting = true;
        checkingEnemyItentions = true;
        InitialSetup = false;
        currentlySelectedTargets.Clear();
        CurrentTargetingType = EnumManager.TargetingType.Enemy_Intentions;
    }

    private static int MaxNumberOfTargets(BaseAttackSO attack)
    {
        int maxTargetableCharacters = 0;

        if (attack.CanTargetEnemies && attack.CanTargetFriendly)
        {
            foreach (Enemy enemy in EnemyManager.Instance.Enemies)
                if (!enemy.Dead)
                {
                    if (maxTargetableCharacters >= attack.NumberOfTargets)
                        break;

                    maxTargetableCharacters++;
                }
            foreach (PlayerCharacter playerCharacter in EnemyManager.Instance.Players)
                if (!playerCharacter.Dead)
                {
                    if (maxTargetableCharacters >= attack.NumberOfTargets)
                        break;

                    maxTargetableCharacters++;
                }
            foreach (EnemySummon enemySummon in EnemyManager.Instance.EnemySummons)
                if (!enemySummon.Dead)
                {
                    if (maxTargetableCharacters >= attack.NumberOfTargets)
                        break;

                    maxTargetableCharacters++;
                }
            foreach (PlayerCharacterSummon playerCharacterSummon in EnemyManager.Instance.PlayerSummons)
                if (!playerCharacterSummon.Dead)
                {
                    if (maxTargetableCharacters >= attack.NumberOfTargets)
                        break;

                    maxTargetableCharacters++;
                }
        }
        else if (attack.CanTargetEnemies)
        {
            foreach (Enemy enemy in EnemyManager.Instance.Enemies)
                if (!enemy.Dead)
                {
                    if (maxTargetableCharacters >= attack.NumberOfTargets)
                        break;

                    maxTargetableCharacters++;
                }
            foreach (EnemySummon enemySummon in EnemyManager.Instance.EnemySummons)
                if (!enemySummon.Dead)
                {
                    if (maxTargetableCharacters >= attack.NumberOfTargets)
                        break;

                    maxTargetableCharacters++;
                }
        }
        else if (attack.CanTargetFriendly)
        {
            foreach (PlayerCharacter playerCharacter in EnemyManager.Instance.Players)
                if (!playerCharacter.Dead)
                {
                    if (maxTargetableCharacters >= attack.NumberOfTargets)
                        break;

                    maxTargetableCharacters++;
                }
            foreach (PlayerCharacterSummon playerCharacterSummon in EnemyManager.Instance.PlayerSummons)
                if (!playerCharacterSummon.Dead)
                {
                    if (maxTargetableCharacters >= attack.NumberOfTargets)
                        break;

                    maxTargetableCharacters++;
                }
        }

        return maxTargetableCharacters;
    }

    public void EnemyTargeting(BaseAttackSO attack)
    {
        CombatManager.Instance.TargetsSelected = false;
        enemyAttacking = true;
        isTargeting = true;
        currentAttack = attack;
        currentlySelectedTargets.Clear();

        foreach (Character character in CombatManager.Instance.CharactersSelected)
        {
            currentlySelectedTargets.Add(character);
            Target(character);
        }

        //handles trash targeting
        if(CombatManager.Instance.GetCurrentEnemy != null && CombatManager.Instance.GetCurrentEnemy.IsTrash)
        {
            foreach(Enemy trashEnemy in EnemyManager.Instance.TrashMobCollection[CombatManager.Instance.GetCurrentEnemy.CharacterName])
            {
                foreach (SpriteRenderer renderer in trashEnemy.TargetingSprites)
                {
                    renderer.material = enemyTargetingMaterial;
                }
            }
        }
        //handles nontrash targeting
        else
        {
            foreach (SpriteRenderer renderer in CombatManager.Instance.GetCurrentCharacter.TargetingSprites)
            {
                renderer.material = enemyTargetingMaterial;
            }
        }

        StartCoroutine(TargetingWait());
    }

    IEnumerator TargetingWait()
    {
        while (CombatManager.Instance.PauseAttack)
            yield return null;

        ReactivateCombatManager(true);

        if (CombatManager.Instance.GetCurrentEnemy != null && CombatManager.Instance.GetCurrentEnemy.IsTrash)
        {
            foreach (Enemy trashEnemy in EnemyManager.Instance.TrashMobCollection[CombatManager.Instance.GetCurrentEnemy.CharacterName])
            {
                foreach (SpriteRenderer renderer in trashEnemy.TargetingSprites)
                {
                    renderer.material = notTargetedMaterial;
                }
            }
        }
        else
        {
            foreach (SpriteRenderer renderer in CombatManager.Instance.GetCurrentCharacter.TargetingSprites)
            {
                renderer.material = notTargetedMaterial;
            }
        }
    }

    private void TrackPlayerInput()
    {
        if (!isTargeting || enemyAttacking || CombatManager.Instance.GetCurrentCharacter.GetComponent<PlayerCharacter>().RevokeControls)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
            CancelAttack();

        switch (CurrentTargetingType)
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
            case EnumManager.TargetingType.Enemy_Intentions:
                TrackEnemyIntentionsInput();
                break;
            default: break;
        }
    }

    private void ReactivateCombatManager(bool enemyTargeting = false)
    {
        if (ForcedTarget != null && currentlySelectedTargets[0] != ForcedTarget)
            return;
        else if (ForcedTarget != null)
            ForcedTarget = null;

        //actually adds cone targets to the list
        if(currentAttack.TargetingType == EnumManager.TargetingType.Multi_Target_Cone)
        {
            if (currentlySelectedTargets[0].GetComponent<Player>())
                ConeSelect(currentlySelectedTargets[0].CombatSpot, true);
            else
                ConeSelect(currentlySelectedTargets[0].CombatSpot, false);
        }

        currentNumberOfTargets++;

        if(currentNumberOfTargets < targets)
        {
            ClearTargets();
            TargetReset();
        }
        else
        {
            if(!enemyTargeting)
            {
                foreach (Character combatSpot in currentlySelectedTargets)
                {
                    CombatManager.Instance.CharactersSelected.Add(combatSpot);
                }
            }

            if(enemyTargeting && CombatManager.Instance.GetCurrentEnemy != null && CombatManager.Instance.GetCurrentCharacter.GetComponent<Enemy>().IsTrash)
            {
                foreach(Enemy enemy in EnemyManager.Instance.TrashMobCollection[CombatManager.Instance.GetCurrentCharacter.GetComponent<Enemy>().CharacterName])
                {
                    foreach (AugmentStackSO aug in enemy.GetAUGS)
                    {
                        if (aug.OnSpellCast)
                            aug.AugmentEffect();
                    }
                }
            }
            else
            {
                foreach (AugmentStackSO aug in CombatManager.Instance.GetCurrentCharacter.GetAUGS)
                {
                    if (aug.OnSpellCast)
                        aug.AugmentEffect();
                }
            }

            if (CombatManager.Instance.GetCurrentPlayer)
                CombatManager.Instance.GetCurrentPlayer.PlayerAttackUI.HandleMana();
            CombatManager.Instance.TargetsSelected = true;
            isTargeting = false;
            enemyAttacking = false;
            currentNumberOfTargets = 0;
            ClearTargets();
            ClearEmptySlots();
            currentlySelectedTargets.Clear();
        }
    }

    private void CancelAttack()
    {
        if(checkingEnemyItentions)
        {
            CombatManager.Instance.AttackDescription.gameObject.SetActive(false);
            checkingEnemyItentions = false;
        }
        else
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

    private void EnemyIntentionTargets(Character target)
    {
        CombatManager.Instance.AttackDisplay.SetEnemyIntentions(target.GetComponent<Enemy>().ChosenAttack.AttackName);

        foreach (Character character in target.GetComponent<Enemy>().CurrentTargets)
        {
            currentlySelectedTargets.Add(character);

            foreach (SpriteRenderer renderer in character.TargetingSprites)
            {
                if (character.GetComponent<Enemy>())
                {
                    if (target.GetComponent<Enemy>().ChosenAttack.EnemyPositiveEffect)
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
                    if (target.GetComponent<Enemy>().ChosenAttack.FriendlyPositiveEffect)
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

        foreach (SpriteRenderer renderer in target.TargetingSprites)
        {
            renderer.material = enemyTargetingMaterial;
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
        //clears material for all characters in list
        foreach (Character character in currentlySelectedTargets)
            foreach (SpriteRenderer renderer in character.TargetingSprites)
                renderer.material = notTargetedMaterial;

        //cone clearing since it doesn't target until selection
        if (currentAttack != null && currentAttack.TargetingType == EnumManager.TargetingType.Multi_Target_Cone)
        {
            if (currentlySelectedTargets[0].CombatSpot != 3 && currentlySelectedTargets[0].CombatSpot != 7 && currentlySelectedTargets[0].CombatSpot != 11)
            {
                if (currentlySelectedTargets[0].GetComponent<PlayerCharacter>() && EnemyManager.Instance.PlayerCombatSpots[currentlySelectedTargets[0].CombatSpot + 1] != null)
                    foreach (SpriteRenderer renderer in EnemyManager.Instance.PlayerCombatSpots[currentlySelectedTargets[0].CombatSpot + 1].TargetingSprites)
                        renderer.material = notTargetedMaterial;
                else if (EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[0].CombatSpot + 1] != null)
                    foreach (SpriteRenderer renderer in EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[0].CombatSpot + 1].TargetingSprites)
                        renderer.material = notTargetedMaterial;
            }
            if (currentlySelectedTargets[0].CombatSpot != 0 && currentlySelectedTargets[0].CombatSpot != 4 && currentlySelectedTargets[0].CombatSpot != 8)
            {
                if (currentlySelectedTargets[0].GetComponent<PlayerCharacter>() && EnemyManager.Instance.PlayerCombatSpots[currentlySelectedTargets[0].CombatSpot - 1] != null)
                    foreach (SpriteRenderer renderer in EnemyManager.Instance.PlayerCombatSpots[currentlySelectedTargets[0].CombatSpot - 1].TargetingSprites)
                        renderer.material = notTargetedMaterial;
                else if (EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[0].CombatSpot - 1] != null)
                    foreach (SpriteRenderer renderer in EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[0].CombatSpot - 1].TargetingSprites)
                        renderer.material = notTargetedMaterial;
            }
        }

        //resets targeting list
        while (currentlySelectedTargets.Count > currentNumberOfTargets)
            currentlySelectedTargets.Remove(currentlySelectedTargets[currentlySelectedTargets.Count - 1]);

        //multi-targeting
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
        if (!InitialSetup)
        {
            InitialSetup = true;
            List<Character> noTargetCharacters = currentAttack.CombatPrefab.GetComponentInChildren<CombatMove>().NoTargetTargeting();
            if(noTargetCharacters != null)
                foreach (Character character in noTargetCharacters)
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
        if (!InitialSetup)
        {
            InitialSetup = true;
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
            if (!InitialSetup)
            {
                InitialSetup = true;
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

            if (Input.GetKeyDown(KeyCode.E))
            {
                targetingEnemies = !targetingEnemies;

                TargetReset();
            }

            if (targetingEnemies)
            {
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
        else if (currentAttack.CanTargetEnemies)
        {
            //set initial target (only enemy side)
            if (!InitialSetup)
            {
                InitialSetup = true;
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
            if (!InitialSetup)
            {
                InitialSetup = true;
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
        //Get primary target
        TrackSingleTargetInput();

        if (!isTargeting)
            return;

        //Get the cone
        if(currentlySelectedTargets[0].GetComponent<PlayerCharacter>())
        {
            ConeTarget(currentlySelectedTargets[0].CombatSpot, true);
        }
        else if(currentlySelectedTargets[0].GetComponent<Enemy>())
        {
            ConeTarget(currentlySelectedTargets[0].CombatSpot, false);
        }
    }

    private void ConeTarget(int index, bool playerSide)
    {
        if(playerSide)
        {
            if (currentlySelectedTargets[0].CombatSpot != 3 && currentlySelectedTargets[0].CombatSpot != 7 && EnemyManager.Instance.PlayerCombatSpots[index + 1] != null && !EnemyManager.Instance.PlayerCombatSpots[index + 1].Dead)
            {
                Target(EnemyManager.Instance.PlayerCombatSpots[index + 1]);
            }
            if (currentlySelectedTargets[0].CombatSpot != 0 && currentlySelectedTargets[0].CombatSpot != 4 && EnemyManager.Instance.PlayerCombatSpots[index - 1] != null && !EnemyManager.Instance.PlayerCombatSpots[index - 1].Dead)
            {
                Target(EnemyManager.Instance.PlayerCombatSpots[index - 1]);
            }
        }
        else
        {
            if (currentlySelectedTargets[0].CombatSpot != 3 && currentlySelectedTargets[0].CombatSpot != 7 && currentlySelectedTargets[0].CombatSpot != 11 && EnemyManager.Instance.EnemyCombatSpots[index + 1] != null && !EnemyManager.Instance.EnemyCombatSpots[index + 1].Dead)
            {
                Target(EnemyManager.Instance.EnemyCombatSpots[index + 1]);
            }
            if (currentlySelectedTargets[0].CombatSpot != 0 && currentlySelectedTargets[0].CombatSpot != 4 && currentlySelectedTargets[0].CombatSpot != 8 && EnemyManager.Instance.EnemyCombatSpots[index - 1] != null && !EnemyManager.Instance.EnemyCombatSpots[index - 1].Dead)
            {
                Target(EnemyManager.Instance.EnemyCombatSpots[index - 1]);
            }
        }
    }

    private void ConeSelect(int index, bool playerSide)
    {
        if (playerSide)
        {
            if (currentlySelectedTargets[0].CombatSpot != 3 && currentlySelectedTargets[0].CombatSpot != 7 && EnemyManager.Instance.PlayerCombatSpots[index + 1] != null && !EnemyManager.Instance.PlayerCombatSpots[index + 1].Dead)
            {
                currentlySelectedTargets.Add(EnemyManager.Instance.PlayerCombatSpots[index + 1]);
            }
            if (currentlySelectedTargets[0].CombatSpot != 0 && currentlySelectedTargets[0].CombatSpot != 4 && EnemyManager.Instance.PlayerCombatSpots[index - 1] != null && !EnemyManager.Instance.PlayerCombatSpots[index - 1].Dead)
            {
                currentlySelectedTargets.Add(EnemyManager.Instance.PlayerCombatSpots[index - 1]);
            }
        }
        else
        {
            if (currentlySelectedTargets[0].CombatSpot != 3 && currentlySelectedTargets[0].CombatSpot != 7 && currentlySelectedTargets[0].CombatSpot != 11 && EnemyManager.Instance.EnemyCombatSpots[index + 1] != null && !EnemyManager.Instance.EnemyCombatSpots[index + 1].Dead)
            {
                currentlySelectedTargets.Add(EnemyManager.Instance.EnemyCombatSpots[index + 1]);
            }
            if (currentlySelectedTargets[0].CombatSpot != 0 && currentlySelectedTargets[0].CombatSpot != 4 && currentlySelectedTargets[0].CombatSpot != 8 && EnemyManager.Instance.EnemyCombatSpots[index - 1] != null && !EnemyManager.Instance.EnemyCombatSpots[index - 1].Dead)
            {
                currentlySelectedTargets.Add(EnemyManager.Instance.EnemyCombatSpots[index - 1]);
            }
        }
    }

    private void TrackMultiTargetChoice()
    {
        TrackSingleTargetInput();
    }

    private void TrackAEOTargetInput()
    {
        //set initial target
        if (!InitialSetup)
        {
            InitialSetup = true;

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
        if (!InitialSetup)
        {
            InitialSetup = true;

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

    private void TrackEnemyIntentionsInput()
    {
        //set initial target (only enemy side)
        if (!InitialSetup)
        {
            InitialSetup = true;
            targetingEnemies = true;
            foreach (Character enemy in EnemyManager.Instance.EnemyCombatSpots)
            {
                if (enemy != null && ((!enemy.Dead && !targetingDead) || (enemy.Dead && targetingDead)) && !currentlySelectedTargets.Contains(enemy))
                {
                    if (!CombatManager.Instance.AttackDisplay)
                        CombatManager.Instance.AttackDisplay = CombatManager.Instance.GetComponentInChildren<AttackDisplay>();

                    currentlySelectedTargets.Add(enemy);
                    EnemyIntentionTargets(enemy);
                    break;
                }
            }
        }

        //track up input
        if (Input.GetKeyDown(KeyCode.W))
        {
            TrackEnemyIntentionsUpTargeting();

            if (CombatManager.Instance.AttackDescription.gameObject.activeInHierarchy)
                UpdateAttackDescription();
        }
        //track down input
        else if (Input.GetKeyDown(KeyCode.S))
        {
            TrackEnemyIntentionsDownTargeting();

            if (CombatManager.Instance.AttackDescription.gameObject.activeInHierarchy)
                UpdateAttackDescription();
        }
        //track left input
        else if (Input.GetKeyDown(KeyCode.A))
        {
            TrackEnemyIntentionsLeftTargeting();

            if (CombatManager.Instance.AttackDescription.gameObject.activeInHierarchy)
                UpdateAttackDescription();
        }
        //track right input
        else if (Input.GetKeyDown(KeyCode.D))
        {
            TrackEnemyIntentionsRightTargeting();

            if (CombatManager.Instance.AttackDescription.gameObject.activeInHierarchy)
                UpdateAttackDescription();
        }

        //enable/disable attack description
        if(Input.GetKeyDown(KeyCode.E))
        {
            if (CombatManager.Instance.AttackDescription.gameObject.activeInHierarchy)
                CombatManager.Instance.AttackDescription.gameObject.SetActive(false);
            else
            {
                CombatManager.Instance.AttackDescription.gameObject.SetActive(true);
                UpdateAttackDescription();
            }
        }
    }

    private void UpdateAttackDescription()
    {
        StartCoroutine(StaticEffect());

        CombatManager.Instance.AttackDescription.MyAttackName.text = currentlySelectedTargets[0].GetComponent<Enemy>().ChosenAttack.AttackName;
        CombatManager.Instance.AttackDescription.MyAttackDescription.text = currentlySelectedTargets[0].GetComponent<Enemy>().ChosenAttack.AttackDescription;
        CombatManager.Instance.AttackDescription.UpdateCost(currentlySelectedTargets[0].GetComponent<Enemy>().ChosenAttack.CostTitle, 
            currentlySelectedTargets[0].GetComponent<Enemy>().ChosenAttack.CostDescription);
        CombatManager.Instance.AttackDescription.UpdateTargetType(currentlySelectedTargets[0].GetComponent<Enemy>().ChosenAttack.TargetingType.ToString());
        CombatManager.Instance.AttackDescription.MyVideoPlayer.clip = currentlySelectedTargets[0].GetComponent<Enemy>().ChosenAttack.MinigameDemo;
        CombatManager.Instance.AttackDescription.MyVideoPlayer.frame = 0;
    }

    IEnumerator StaticEffect()
    {
        CombatManager.Instance.AttackDescription.Static.SetActive(true);
        CombatManager.Instance.AttackDescription.Screen.GetComponent<RawImage>().color = Color.clear;

        yield return new WaitForSeconds(staticTime);

        CombatManager.Instance.AttackDescription.Static.SetActive(false);
        CombatManager.Instance.AttackDescription.Screen.GetComponent<RawImage>().color = Color.white;
    }

    private void TrackEnemyUpTargeting()
    {
        if (currentlySelectedTargets[currentNumberOfTargets].CombatSpot <= 3)
            column = 0;
        else if (currentlySelectedTargets[currentNumberOfTargets].CombatSpot <= 7)
            column = 1;
        else
            column = 2;

        //top of enemy column back
        if (currentlySelectedTargets[currentNumberOfTargets].CombatSpot == FindMinSlot(column, false) && column == 0)
        {
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
        else if (currentlySelectedTargets[currentNumberOfTargets].CombatSpot == FindMinSlot(column, false) && column == 1)
        {
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
        else if (currentlySelectedTargets[0].CombatSpot == FindMinSlot(column, false) && column == 2)
        {
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
        if (currentlySelectedTargets[currentNumberOfTargets].CombatSpot <= 3)
            column = 0;
        else if (currentlySelectedTargets[currentNumberOfTargets].CombatSpot <= 7)
            column = 1;
        else
            column = 2;

        //bottom of enemy column back
        if (currentlySelectedTargets[currentNumberOfTargets].CombatSpot == FindMaxSlot(column, false) && column == 0)
        {
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
        else if (currentlySelectedTargets[currentNumberOfTargets].CombatSpot == FindMaxSlot(column, false) && column == 1)
        {
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
        else if (currentlySelectedTargets[currentNumberOfTargets].CombatSpot == FindMaxSlot(column, false) && column == 2)
        {
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
            else if (EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[currentNumberOfTargets].CombatSpot - 4] != null && ((!EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[currentNumberOfTargets].CombatSpot - 4].Dead && !targetingDead) || (EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[currentNumberOfTargets].CombatSpot - 4].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[currentNumberOfTargets].CombatSpot - 4]))
            {
                Character tempCharacter = EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[currentNumberOfTargets].CombatSpot - 4];
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
            else if(column == 2 && EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[0].CombatSpot - 8] != null && ((!EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[0].CombatSpot - 8].Dead && !targetingDead) || (EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[0].CombatSpot - 8].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[0].CombatSpot - 8]))
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
    }

    private void TrackPlayerUpTargeting()
    {
        if (currentlySelectedTargets[currentNumberOfTargets].CombatSpot <= 3)
            column = 0;
        else
            column = 1;

        //top of player column
        if (currentlySelectedTargets[currentNumberOfTargets].CombatSpot == FindMinSlot(column, true) && column == 0)
        {
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
        else if (currentlySelectedTargets[currentNumberOfTargets].CombatSpot == FindMinSlot(column, true) && column == 1)
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
        if (currentlySelectedTargets[currentNumberOfTargets].CombatSpot == FindMaxSlot(column, true) && column == 0)
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
        else if (currentlySelectedTargets[currentNumberOfTargets].CombatSpot == FindMaxSlot(column, true) && column == 1)
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

    private void TrackEnemyIntentionsUpTargeting()
    {
        if (currentlySelectedTargets[currentNumberOfTargets].CombatSpot <= 3)
            column = 0;
        else if (currentlySelectedTargets[currentNumberOfTargets].CombatSpot <= 7)
            column = 1;
        else
            column = 2;

        //top of enemy column back
        if (currentlySelectedTargets[currentNumberOfTargets].CombatSpot == FindMinSlot(column, false) && column == 0)
        {
            for (int i = 3; i > 0; i--)
            {
                if (EnemyManager.Instance.EnemyCombatSpots[i] != null && ((!EnemyManager.Instance.EnemyCombatSpots[i].Dead && !targetingDead) || (EnemyManager.Instance.EnemyCombatSpots[i].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.EnemyCombatSpots[i]))
                {
                    ClearTargets();
                    currentlySelectedTargets.Add(EnemyManager.Instance.EnemyCombatSpots[i]);
                    EnemyIntentionTargets(currentlySelectedTargets[currentNumberOfTargets]);
                    break;
                }
            }
        }
        //top of enemy column front
        else if (currentlySelectedTargets[currentNumberOfTargets].CombatSpot == FindMinSlot(column, false) && column == 1)
        {
            for (int i = 7; i > 3; i--)
            {
                if (EnemyManager.Instance.EnemyCombatSpots[i] != null && ((!EnemyManager.Instance.EnemyCombatSpots[i].Dead && !targetingDead) || (EnemyManager.Instance.EnemyCombatSpots[i].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.EnemyCombatSpots[i]))
                {
                    ClearTargets();
                    currentlySelectedTargets.Add(EnemyManager.Instance.EnemyCombatSpots[i]);
                    EnemyIntentionTargets(currentlySelectedTargets[currentNumberOfTargets]);
                    break;
                }
            }
        }
        //top of enemy summon column
        else if (currentlySelectedTargets[0].CombatSpot == FindMinSlot(column, false) && column == 2)
        {
            for (int i = 11; i > 7; i--)
            {
                if (EnemyManager.Instance.EnemyCombatSpots[i] != null && ((!EnemyManager.Instance.EnemyCombatSpots[i].Dead && !targetingDead) || (EnemyManager.Instance.EnemyCombatSpots[i].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.EnemyCombatSpots[i]))
                {
                    ClearTargets();
                    currentlySelectedTargets.Add(EnemyManager.Instance.EnemyCombatSpots[i]);
                    EnemyIntentionTargets(currentlySelectedTargets[currentNumberOfTargets]);
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
                    EnemyIntentionTargets(currentlySelectedTargets[currentNumberOfTargets]);
                    break;
                }
            }
        }
    }

    private void TrackEnemyIntentionsDownTargeting()
    {
        if (currentlySelectedTargets[currentNumberOfTargets].CombatSpot <= 3)
            column = 0;
        else if (currentlySelectedTargets[currentNumberOfTargets].CombatSpot <= 7)
            column = 1;
        else
            column = 2;

        //bottom of enemy column back
        if (currentlySelectedTargets[currentNumberOfTargets].CombatSpot == FindMaxSlot(column, false) && column == 0)
        {
            for (int i = 0; i < 3; i++)
            {
                if (EnemyManager.Instance.EnemyCombatSpots[i] != null && ((!EnemyManager.Instance.EnemyCombatSpots[i].Dead && !targetingDead) || (EnemyManager.Instance.EnemyCombatSpots[i].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.EnemyCombatSpots[i]))
                {
                    ClearTargets();
                    currentlySelectedTargets.Add(EnemyManager.Instance.EnemyCombatSpots[i]);
                    EnemyIntentionTargets(currentlySelectedTargets[currentNumberOfTargets]);
                    break;
                }
            }
        }
        //bottom of enemy column front
        else if (currentlySelectedTargets[currentNumberOfTargets].CombatSpot == FindMaxSlot(column, false) && column == 1)
        {
            for (int i = 4; i < 7; i++)
            {
                if (EnemyManager.Instance.EnemyCombatSpots[i] != null && ((!EnemyManager.Instance.EnemyCombatSpots[i].Dead && !targetingDead) || (EnemyManager.Instance.EnemyCombatSpots[i].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.EnemyCombatSpots[i]))
                {
                    ClearTargets();
                    currentlySelectedTargets.Add(EnemyManager.Instance.EnemyCombatSpots[i]);
                    EnemyIntentionTargets(currentlySelectedTargets[currentNumberOfTargets]);
                    break;
                }
            }
        }
        //bottom of enemy summon column
        else if (currentlySelectedTargets[currentNumberOfTargets].CombatSpot == FindMaxSlot(column, false) && column == 2)
        {
            for (int i = 8; i < 11; i++)
            {
                if (EnemyManager.Instance.EnemyCombatSpots[i] != null && ((!EnemyManager.Instance.EnemyCombatSpots[i].Dead && !targetingDead) || (EnemyManager.Instance.EnemyCombatSpots[i].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.EnemyCombatSpots[i]))
                {
                    ClearTargets();
                    currentlySelectedTargets.Add(EnemyManager.Instance.EnemyCombatSpots[i]);
                    EnemyIntentionTargets(currentlySelectedTargets[currentNumberOfTargets]);
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
                    EnemyIntentionTargets(currentlySelectedTargets[currentNumberOfTargets]);
                    break;
                }
            }
        }
    }

    private void TrackEnemyIntentionsLeftTargeting()
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
                EnemyIntentionTargets(currentlySelectedTargets[currentNumberOfTargets]);
            }
            else if (EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[currentNumberOfTargets].CombatSpot - 4] != null && ((!EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[currentNumberOfTargets].CombatSpot - 4].Dead && !targetingDead) || (EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[currentNumberOfTargets].CombatSpot - 4].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[currentNumberOfTargets].CombatSpot - 4]))
            {
                Character tempCharacter = EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[currentNumberOfTargets].CombatSpot - 4];
                ClearTargets();
                currentlySelectedTargets.Add(tempCharacter);
                EnemyIntentionTargets(currentlySelectedTargets[currentNumberOfTargets]);
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
                                EnemyIntentionTargets(currentlySelectedTargets[currentNumberOfTargets]);
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
                                EnemyIntentionTargets(currentlySelectedTargets[currentNumberOfTargets]);
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
                                EnemyIntentionTargets(currentlySelectedTargets[currentNumberOfTargets]);
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
                EnemyIntentionTargets(currentlySelectedTargets[currentNumberOfTargets]);
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
                                EnemyIntentionTargets(currentlySelectedTargets[currentNumberOfTargets]);
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
                                EnemyIntentionTargets(currentlySelectedTargets[currentNumberOfTargets]);
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
                                EnemyIntentionTargets(currentlySelectedTargets[currentNumberOfTargets]);
                                return;
                            }
                        }
                        tempColumn++;
                    }
                }
            }
        }
    }

    private void TrackEnemyIntentionsRightTargeting()
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
                EnemyIntentionTargets(currentlySelectedTargets[0]);
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
                                EnemyIntentionTargets(currentlySelectedTargets[0]);
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
                                EnemyIntentionTargets(currentlySelectedTargets[0]);
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
                                EnemyIntentionTargets(currentlySelectedTargets[0]);
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
                EnemyIntentionTargets(currentlySelectedTargets[0]);
            }
            else if (column == 2 && EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[0].CombatSpot - 8] != null && ((!EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[0].CombatSpot - 8].Dead && !targetingDead) || (EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[0].CombatSpot - 8].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[0].CombatSpot - 8]))
            {
                Character tempCharacter = EnemyManager.Instance.EnemyCombatSpots[currentlySelectedTargets[0].CombatSpot - 8];
                ClearTargets();
                currentlySelectedTargets.Add(tempCharacter);
                EnemyIntentionTargets(currentlySelectedTargets[0]);
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
                                EnemyIntentionTargets(currentlySelectedTargets[0]);
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
                                EnemyIntentionTargets(currentlySelectedTargets[0]);
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
                                EnemyIntentionTargets(currentlySelectedTargets[0]);
                                return;
                            }
                        }
                        tempColumn += 2;
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
                if (EnemyManager.Instance.EnemyCombatSpots[i] != null && ((!EnemyManager.Instance.EnemyCombatSpots[i].Dead && !targetingDead) || (EnemyManager.Instance.EnemyCombatSpots[i].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.EnemyCombatSpots[i]))
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
                if (EnemyManager.Instance.EnemyCombatSpots[i] != null && ((!EnemyManager.Instance.EnemyCombatSpots[i].Dead && !targetingDead) || (EnemyManager.Instance.EnemyCombatSpots[i].Dead && targetingDead)) && !currentlySelectedTargets.Contains(EnemyManager.Instance.EnemyCombatSpots[i]))
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
