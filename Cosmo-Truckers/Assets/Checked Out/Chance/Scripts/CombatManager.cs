using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    //Multiplayer Stuffs
    public Material[] playerMaterials;

    [HideInInspector] public static CombatManager Instance;
    [SerializeField] private Sprite blankBG;
    public GameObject CurrentBG;

    GameObject miniGame;
    public AttackDescription AttackDescription;
    public GameObject GetMiniGame { get => miniGame; }
    List<GameObject> characters;   //Currently only one 
                            //Needs to be made into list for enemy multi target skills
    [SerializeField] TMP_Text Timer;
    public Targeting MyTargeting;
    [SerializeField] float trashAttackDelay = .25f;

    [SerializeField] public List<Character> CharactersSelected;
    public List<PlayerCharacter> ActivePlayers;

    public List<Character> GetCharactersSelected { get => CharactersSelected; }

    PlayerCharacter CurrentPlayer;
    Enemy CurrentEnemy;
    public Character CurrentCharacter;
    public PlayerCharacter GetCurrentPlayer { get => CurrentPlayer; }

    public BaseAttackSO CurrentAttack;
    public AttackDisplay AttackDisplay;
    public Enemy GetCurrentEnemy { get => CurrentEnemy; }

    public Character GetCurrentCharacter { get => CurrentCharacter; }

    public bool PauseManager = false;
    public bool PauseAttack = false;

    public GameObject IntentionChange;
    public Vector3 LastCameraPosition;
    public Transform DungeonCharacterInstance;
    public DNode CurrentNode;
    public bool InCombat;

    [Space(20)]
    [Header("Combat Visual Effects")]
    public GameObject BaseCombatStar;
    public GameObject UiCombatStar;
    public Material DamageStarMaterial;
    public Material HealingStarMaterial; 
    public Material ShieldStarMaterial; 
    public float CombatStarSpawnWaitTime = 0.25f; 
    public float CombatStarMaxOffset = 0.3f;
    public float CombatStarSpawnCheckRadius = 0.05f;

    bool inTrashEndMove = false;
    INAcombat INA;

    private void Awake() => Instance = this;
    [HideInInspector] public bool TargetsSelected = true;
    [HideInInspector] public int CommandsExecuting;

    private void Start()
    {
        INA = FindObjectOfType<INAcombat>();
        Instantiate(CurrentBG, INA.transform);
    }

    public void StartCombat(BaseAttackSO attack, PlayerCharacter currentPlayer)
    {
        CurrentAttack = attack;
        CharactersSelected.Clear();
        ActivePlayers = new List<PlayerCharacter>();
        characters = new List<GameObject>();
        //To ensure this is cleared on player turn
        CurrentEnemy = null;
        CurrentCharacter = currentPlayer;
        CurrentPlayer = currentPlayer;
        ActivePlayers.Add(currentPlayer);
        TargetsSelected = false;
        MyTargeting.StartTargeting(attack);

        StartCoroutine(StartMiniGame(attack, ActivePlayers));
    }

    public void StartTurnEnemy(BaseAttackSO attack, Enemy enemy)
    {
        CurrentAttack = attack;
        CurrentEnemy = enemy;
        //EnemyTarget(attack, enemy);
        CharactersSelected.Clear();
        ActivePlayers = new List<PlayerCharacter>();
        characters = new List<GameObject>();
        CurrentPlayer = null;
        CurrentCharacter = enemy;

        //I dont think this code is necessary but i am also gay
        //if (!enemy.SpecialTargetConditions)
        //    EnemyTarget(attack, enemy);

        foreach (Character character in enemy.CurrentTargets)
        {
            if (character.GetComponent<PlayerCharacter>() && !character.GetComponent<PlayerCharacterSummon>() && !ActivePlayers.Contains(character))
                ActivePlayers.Add(character.GetComponent<PlayerCharacter>());
            else if (character.GetComponent<PlayerCharacterSummon>() && character.GetComponent<PlayerCharacterSummon>().Summoner)
                ActivePlayers.Add(character.GetComponent<PlayerCharacterSummon>().Summoner);
            else if (character.GetComponent<PlayerCharacterSummon>())
                ActivePlayers.Add(EnemyManager.Instance.PlayerCombatSpots[character.CombatSpot - EnemyManager.Instance.playerSummonIndexAdder].GetComponent<PlayerCharacter>());
            CharactersSelected.Add(character);
        }

        foreach (Character character in CharactersSelected)
            if (character.Dead && !attack.TargetsDead)
                CharactersSelected.Remove(character);

        //Plays minigame if there is a reason to
        if(CharactersSelected.Count > 0)
        {
            StartCoroutine(DisplayAttack(attack, ActivePlayers));
            MyTargeting.EnemyTargeting(attack);
        }
        //if all targets are dead and this minigame doesn't target dead characters, trigger end of turn
        else
        {
            HandleEndOfTurn();
        }
    }

    public void EnemyTarget(BaseAttackSO attack, Enemy enemy)
    {
        switch (attack.TargetingType)
        {
            #region No Target
            case EnumManager.TargetingType.No_Target:
                Debug.Log($"Doing Combat Stuff for {attack.AttackName}, no target. . .");
                break;
            #endregion
            #region Self Target
            case EnumManager.TargetingType.Self_Target:
                enemy.CurrentTargets.Add(enemy);
                Debug.Log($"Doing Combat Stuff for {attack.AttackName}, self target. . .");
                break;
            #endregion
            #region Single Target
            case EnumManager.TargetingType.Single_Target:
                SingleTargetEnemy(attack, enemy);
                break;
            #endregion
            #region Multi Target Cone
            case EnumManager.TargetingType.Multi_Target_Cone:
                ConeTargetEnemy(attack, enemy);
                Debug.Log($"Doing Combat Stuff for {attack.AttackName}, Cone attack. . .");
                break;
            #endregion
            #region Multi Target Choice
            case EnumManager.TargetingType.Multi_Target_Choice:
                MultiTargetChoiceEnemy(attack, enemy);
                break;
            #endregion
            #region AOE
            case EnumManager.TargetingType.AOE:
                foreach (var obj in EnemyManager.Instance.GetAlivePlayerCharacters())
                {
                    enemy.CurrentTargets.Add(obj);
                }
                Debug.Log($"Doing Combat Stuff for {attack.AttackName}, AOE. . .");
                break;
            #endregion
            #region All Target
            case EnumManager.TargetingType.All_Target:
                AllTargetEnemy(enemy, attack);
                break;
                #endregion
        }
    }

    public void StartTurnPlayerSummon(BaseAttackSO attack, PlayerCharacterSummon summon)
    {
        CurrentAttack = attack;
        PlayerSummonTarget(attack, summon);
    }

    private void PlayerSummonTarget(BaseAttackSO attack, PlayerCharacterSummon summon)
    {
        CharactersSelected.Clear();
        ActivePlayers = new List<PlayerCharacter>();
        characters = new List<GameObject>();
        CurrentPlayer = null;
        CurrentEnemy = null;
        CurrentCharacter = summon;

        if (summon.SpecialTargetConditions)
        {
            summon.TargetConditions(attack);

            foreach (Character character in CharactersSelected)
                if (character.GetComponent<PlayerCharacter>() && !character.GetComponent<PlayerCharacterSummon>() && !ActivePlayers.Contains(character) && !CurrentAttack.AutoCast)
                    ActivePlayers.Add(character.GetComponent<PlayerCharacter>());
                else if (character.GetComponent<PlayerCharacterSummon>() && !character.GetComponent<PlayerCharacterSummon>().Summoner)
                    ActivePlayers.Add(character.GetComponent<PlayerCharacterSummon>().Summoner);
                else if (character.GetComponent<PlayerCharacterSummon>())
                    ActivePlayers.Add(EnemyManager.Instance.PlayerCombatSpots[character.CombatSpot - EnemyManager.Instance.playerSummonIndexAdder].GetComponent<PlayerCharacter>());
        }
        else
        {
            switch (attack.TargetingType)
            {
                #region No Target
                case EnumManager.TargetingType.No_Target:
                    break;
                #endregion
                #region Self Target
                case EnumManager.TargetingType.Self_Target:
                    CharactersSelected.Add(summon);
                    break;
                #endregion
                #region Single Target
                case EnumManager.TargetingType.Single_Target:
                    SingleTargetPlayer(CurrentAttack, summon);
                    break;
                #endregion
                #region Multi Target Cone
                case EnumManager.TargetingType.Multi_Target_Cone:
                    break;
                #endregion
                #region Multi Target Choice
                case EnumManager.TargetingType.Multi_Target_Choice:
                    break;
                #endregion
                #region AOE
                case EnumManager.TargetingType.AOE:
                    break;
                #endregion
                #region All Target
                case EnumManager.TargetingType.All_Target:
                    break;
                    #endregion
            }
        }

        StartCoroutine(DisplayAttack(attack, ActivePlayers));
        MyTargeting.EnemyTargeting(attack);
    }

    public PlayerCharacter FindDPSCharacter()
    {
        foreach (PlayerCharacter player in EnemyManager.Instance.GetAlivePlayerCharacters())
            if (player.IsDPS)
                return player;

        return null;
    }

    public PlayerCharacter FindUtilityCharacter()
    {
        foreach (PlayerCharacter player in EnemyManager.Instance.GetAlivePlayerCharacters())
            if (player.IsUtility)
                return player;

        return null;
    }
    public PlayerCharacter FindTankCharacter()
    {
        foreach (PlayerCharacter player in EnemyManager.Instance.GetAlivePlayerCharacters())
            if (player.IsTank)
                return player;

        return null;
    }
    public PlayerCharacter FindSupportCharacter()
    {
        foreach (PlayerCharacter player in EnemyManager.Instance.GetAlivePlayerCharacters())
            if (player.IsSupport)
                return player;

        return null;
    }

    public void SingleTargetEnemy(BaseAttackSO attack, Enemy enemy, PlayerCharacter player = null)
    {
        //enemy is taunted
        if (enemy.TauntedBy != null && !enemy.TauntedBy.Dead && !enemy.CurrentTargets.Contains(enemy.TauntedBy))
        {
            if (CheckPlayerSummonLayer(EnemyManager.Instance.PlayerCombatSpots[enemy.TauntedBy.CombatSpot + EnemyManager.Instance.playerSummonIndexAdder]))
            {
                //CharactersSelected.Add(EnemyManager.Instance.PlayerCombatSpots[enemy.TauntedBy.CombatSpot + EnemyManager.Instance.playerSummonIndexAdder]);
                enemy.CurrentTargets.Add(EnemyManager.Instance.PlayerCombatSpots[enemy.TauntedBy.CombatSpot + EnemyManager.Instance.playerSummonIndexAdder]);
            }
            else
            {
                enemy.CurrentTargets.Add(enemy.TauntedBy);
                //ActivePlayers.Add(enemy.TauntedBy);
            }
        }
        //player selected already
        else if(player != null && !player.Dead)
        {
            if (!player.GetComponent<PlayerCharacterSummon>() && CheckPlayerSummonLayer(EnemyManager.Instance.PlayerCombatSpots[player.CombatSpot + EnemyManager.Instance.playerSummonIndexAdder]))
            {
                enemy.CurrentTargets.Add(EnemyManager.Instance.PlayerCombatSpots[player.CombatSpot + EnemyManager.Instance.playerSummonIndexAdder]);
                //TODO CHECK IF COMBAT SPOT IS OF TYPE PLAYERCHARACTERSUMMON THEN ADD SUMMONER REFERENCE TO ACTIVEPLAYERS
            }
            else
            {
                enemy.CurrentTargets.Add(player);
            }
        }
        //enemy is not taunted
        else
        {
            SelectRandomPlayerCharacterEnemy(enemy);
        }
    }

    public void SingleTargetPlayer(BaseAttackSO attack, PlayerCharacterSummon summon)
    {
        //Get Alive Enemies
        System.Random random = new System.Random();
        List<Enemy> aliveEnemies = EnemyManager.Instance.GetAliveEnemies();
        foreach(Enemy enemySummon in EnemyManager.Instance.GetAliveEnemySummons())
            aliveEnemies.Add(enemySummon);

        for (int i = 0; i < aliveEnemies.Count - 1; i++)
        {
            int j = random.Next(i, aliveEnemies.Count);
            Enemy temp = aliveEnemies[i];
            aliveEnemies[i] = aliveEnemies[j];
            aliveEnemies[j] = temp;
        }

        foreach (Enemy obj in aliveEnemies)
        {
            CharactersSelected.Add(obj);
            break;
        }
    }

    private void SelectRandomPlayerCharacterEnemy(Enemy enemy, bool ignoreSummons = false)
    {
        System.Random random = new System.Random();
        List<PlayerCharacter> alivePlayers = EnemyManager.Instance.GetAlivePlayerCharacters();

        for (int i = 0; i < alivePlayers.Count - 1; i++)
        {
            int j = random.Next(i, alivePlayers.Count);
            PlayerCharacter temp = alivePlayers[i];
            alivePlayers[i] = alivePlayers[j];
            alivePlayers[j] = temp;
        }

        foreach (PlayerCharacter obj in alivePlayers)
        {
            if (!ignoreSummons && !obj.GetComponent<PlayerCharacterSummon>() && CheckPlayerSummonLayer(EnemyManager.Instance.PlayerCombatSpots[obj.CombatSpot + EnemyManager.Instance.playerSummonIndexAdder]))
            {
                enemy.CurrentTargets.Add(EnemyManager.Instance.PlayerCombatSpots[obj.CombatSpot + EnemyManager.Instance.playerSummonIndexAdder]);
                break;
            }
            else if (!enemy.CurrentTargets.Contains(obj))
            {
                enemy.CurrentTargets.Add(obj);
                break;
            }
        }
    }

    public void SelectRandomPlayerCharacter(bool ignoreSummons = false)
    {
        System.Random random = new System.Random();
        List<PlayerCharacter> alivePlayers = EnemyManager.Instance.GetAlivePlayerCharacters();

        for (int i = 0; i < alivePlayers.Count - 1; i++)
        {
            int j = random.Next(i, alivePlayers.Count);
            PlayerCharacter temp = alivePlayers[i];
            alivePlayers[i] = alivePlayers[j];
            alivePlayers[j] = temp;
        }

        foreach (PlayerCharacter obj in alivePlayers)
        {
            if (!ignoreSummons && !obj.GetComponent<PlayerCharacterSummon>() && CheckPlayerSummonLayer(EnemyManager.Instance.PlayerCombatSpots[obj.CombatSpot + EnemyManager.Instance.playerSummonIndexAdder]))
            {
                CharactersSelected.Add(EnemyManager.Instance.PlayerCombatSpots[obj.CombatSpot + EnemyManager.Instance.playerSummonIndexAdder]);
                //TODO CHECK IF COMBAT SPOT IS OF TYPE PLAYERCHARACTERSUMMON THEN ADD SUMMONER REFERENCE TO ACTIVEPLAYERS
                if (!CurrentAttack.AutoCast)
                    ActivePlayers.Add(obj);

                break;
            }
            else if (!CharactersSelected.Contains(obj))
            {
                CharactersSelected.Add(obj);
                if(!CurrentAttack.AutoCast)
                    ActivePlayers.Add(obj);
                break;
            }
        }
    }

    public void IgnoreTauntSingleTarget(bool ignoreSummons = false)
    {
        System.Random random = new System.Random();

        for (int i = 0; i < EnemyManager.Instance.GetAlivePlayerCharacters().Count - 1; i++)
        {
            int j = random.Next(i, EnemyManager.Instance.GetAlivePlayerCharacters().Count);
            PlayerCharacter temp = EnemyManager.Instance.GetAlivePlayerCharacters()[i];
            EnemyManager.Instance.GetAlivePlayerCharacters()[i] = EnemyManager.Instance.GetAlivePlayerCharacters()[j];
            EnemyManager.Instance.GetAlivePlayerCharacters()[j] = temp;
        }

        foreach (PlayerCharacter obj in EnemyManager.Instance.GetAlivePlayerCharacters())
        {
            if (!obj.GetComponent<PlayerCharacterSummon>() && CheckPlayerSummonLayer(EnemyManager.Instance.PlayerCombatSpots[obj.CombatSpot + EnemyManager.Instance.playerSummonIndexAdder]) && !ignoreSummons)
            {
                CharactersSelected.Add(EnemyManager.Instance.PlayerCombatSpots[obj.CombatSpot + EnemyManager.Instance.playerSummonIndexAdder]);
                //TODO CHECK IF COMBAT SPOT IS OF TYPE PLAYERCHARACTERSUMMON THEN ADD SUMMONER REFERENCE TO ACTIVEPLAYERS
                ActivePlayers.Add(obj);

                break;
            }
            else
            {
                CharactersSelected.Add(obj);
                //Think this was the issue
                ActivePlayers.Add(obj);
                break;
            }
        }
    }

    public void ConeTargetEnemy(BaseAttackSO attack, Enemy enemy, Character character = null)
    {
        if (character == null)
        {
            SingleTargetEnemy(attack, enemy);
            character = enemy.CurrentTargets[0];
        }

        int minVal = 0;
        int maxVal = 3;

        if (character.GetComponent<PlayerCharacterSummon>())
        {
            minVal = 4;
            maxVal = 7;

            if (character.CombatSpot - 1 >= minVal && EnemyManager.Instance.EnemyCombatSpots[character.CombatSpot - 1] != null && !EnemyManager.Instance.EnemyCombatSpots[character.CombatSpot - 1].Dead)
                enemy.CurrentTargets.Add(EnemyManager.Instance.EnemyCombatSpots[character.CombatSpot - 1]);
            if (character.CombatSpot + 1 < maxVal && EnemyManager.Instance.EnemyCombatSpots[character.CombatSpot + 1] != null && !EnemyManager.Instance.EnemyCombatSpots[character.CombatSpot + 1].Dead)
                enemy.CurrentTargets.Add(EnemyManager.Instance.EnemyCombatSpots[character.CombatSpot + 1]);
        }
        else if(character.GetComponent<PlayerCharacter>())
        {
            if (character.CombatSpot - 1 >= minVal && EnemyManager.Instance.PlayerCombatSpots[character.CombatSpot - 1] != null && !EnemyManager.Instance.PlayerCombatSpots[character.CombatSpot - 1].Dead)
                enemy.CurrentTargets.Add(EnemyManager.Instance.PlayerCombatSpots[character.CombatSpot - 1]);
            if(character.CombatSpot + 1 <= maxVal && EnemyManager.Instance.PlayerCombatSpots[character.CombatSpot + 1] != null && !EnemyManager.Instance.PlayerCombatSpots[character.CombatSpot + 1].Dead)
                enemy.CurrentTargets.Add(EnemyManager.Instance.PlayerCombatSpots[character.CombatSpot + 1]);
        }
    }

    private void MultiTargetChoiceEnemy(BaseAttackSO attack, Enemy enemy)
    {
        int tempNumberOfTargets = attack.NumberOfTargets;

        if (tempNumberOfTargets > EnemyManager.Instance.GetAlivePlayerCharacters().Count)
            tempNumberOfTargets = EnemyManager.Instance.GetAlivePlayerCharacters().Count;

        for(int i = 0; i < tempNumberOfTargets; i++)
            SingleTargetEnemy(attack, enemy);
    }

    public void AOETargetPlayers(Enemy enemy, BaseAttackSO attack)
    {
        foreach (PlayerCharacter obj in EnemyManager.Instance.GetAlivePlayerCharacters())
        {
            enemy.CurrentTargets.Add(obj);
        }
        foreach (PlayerCharacterSummon obj in EnemyManager.Instance.GetAlivePlayerSummons())
        {
            enemy.CurrentTargets.Add(obj);
        }
    }
    public void AOETargetEnemies(Enemy enemy, BaseAttackSO attack)
    {
        //foreach (PlayerCharacter player in activePlayers)
        //    ActivePlayers.Add(player);

        foreach (Enemy obj in EnemyManager.Instance.GetAliveEnemies())
        {
            enemy.CurrentTargets.Add(obj);
        }
        foreach (EnemySummon obj in EnemyManager.Instance.GetAliveEnemySummons())
        {
            enemy.CurrentTargets.Add(obj);
        }
    }

    public void AllTargetEnemy(Enemy enemy, BaseAttackSO attack)
    {
        foreach (PlayerCharacter obj in EnemyManager.Instance.GetAlivePlayerCharacters())
        {
            enemy.CurrentTargets.Add(obj);
        }
        foreach(PlayerCharacterSummon obj in EnemyManager.Instance.GetAlivePlayerSummons())
        {
            enemy.CurrentTargets.Add(obj);
        }
        foreach (Enemy obj in EnemyManager.Instance.GetAliveEnemies())
        {
            enemy.CurrentTargets.Add(obj);
        }
        foreach (EnemySummon obj in EnemyManager.Instance.GetAliveEnemySummons())
        {
            enemy.CurrentTargets.Add(obj);
        }
    }

    public void DetermineTauntedTarget(Enemy enemy)
    {
        //enemy is taunted
        if (enemy.TauntedBy != null && !enemy.TauntedBy.Dead)
        {
            if (CheckPlayerSummonLayer(EnemyManager.Instance.PlayerCombatSpots[enemy.TauntedBy.CombatSpot + EnemyManager.Instance.playerSummonIndexAdder]))
                enemy.CurrentTargets.Add(EnemyManager.Instance.PlayerCombatSpots[enemy.TauntedBy.CombatSpot + EnemyManager.Instance.playerSummonIndexAdder]);
            else
                enemy.CurrentTargets.Add(enemy.TauntedBy);
        }
    }

    IEnumerator DisplayAttack(BaseAttackSO attack, List<PlayerCharacter> charactersToSpawn)
    {
        PauseAttack = true;

        if (!AttackDisplay)
            AttackDisplay = GetComponentInChildren<AttackDisplay>();
        AttackDisplay.SetAttack(attack.AttackName, ActivePlayers);

        while(PauseAttack)
            yield return null;

        StartCoroutine(StartMiniGame(attack, ActivePlayers));
    }

    IEnumerator StartMiniGame(BaseAttackSO attack, List<PlayerCharacter> charactersToSpawn)
    {
        while (!TargetsSelected)
            yield return null;

        if (!attack.AutoCast)
        {
            PauseManager = true;
            miniGame = Instantiate(attack.CombatPrefab, INA.transform);

            float miniGameTime = miniGame.GetComponent<CombatMove>().MinigameDuration; 

            if (attack.BossMove)
                Timer.text = "";
            else
                Timer.text = miniGameTime.ToString();

            StartCoroutine(INA.MoveINACombat(true));

            if (ActivePlayers.Count > 0)
            {
                foreach (PlayerCharacter character in ActivePlayers) //PlayerCharacter invalid cast
                {
                    for (int i = 0; i < character.GetAUGS.Count; i++)
                    {
                        if (character.GetAUGS[i].InCombat)
                            character.GetAUGS[i].AugmentEffect();
                    }
                }
            }

            if (CurrentCharacter.GetComponent<Enemy>())
            {
                if(CurrentCharacter.GetComponent<Enemy>().IsTrash)
                {
                    List<string> augmentsAlreadyUsed = new List<string>();

                    foreach (Enemy enemy in EnemyManager.Instance.TrashMobCollection[GetCurrentCharacter.GetComponent<Enemy>().CharacterName])
                    {
                        foreach (AugmentStackSO aug in enemy.GetAUGS)
                        {
                            if (aug.InCombat && !augmentsAlreadyUsed.Contains(aug.AugmentName))
                            {
                                aug.AugmentEffect();
                                augmentsAlreadyUsed.Add(aug.AugmentName);
                            }
                        }
                    }
                }
                else
                {
                    foreach (AugmentStackSO augment in CurrentCharacter.GetAUGS)
                    {
                        if (augment.InCombat)
                            augment.AugmentEffect();
                    }
                }
            }

            while (PauseManager)
                yield return null;

            //Attack SO Start
            foreach (Player player in FindObjectsOfType<Player>())
            {
                player.enabled = true;
            }

            attack.StartCombat();

            //Boss move handler. Does not track time. Tracks Fight won and players dead
            if(attack.BossMove)
            {
                while (!miniGame.GetComponentInChildren<CombatMove>().FightWon && !miniGame.GetComponentInChildren<CombatMove>().PlayerDead && !miniGame.GetComponentInChildren<CombatMove>().MoveEnded)
                {
                    yield return null;
                }
            }

            //Timer and end move handler for non-boss moves
            else
            {
                while (miniGameTime >= 0 && !miniGame.GetComponentInChildren<CombatMove>().PlayerDead && !miniGame.GetComponentInChildren<CombatMove>().MoveEnded)
                {
                    miniGameTime -= Time.deltaTime;
                    Timer.text = ((int)miniGameTime).ToString();

                    yield return null;
                }
            }

            StopAllCoroutines();

            PauseManager = true;

            StartCoroutine(INA.MoveINACombat(false));
        }
        else
        {
            miniGame = Instantiate(attack.CombatPrefab, INA.transform);
            StartCoroutine(EndCombat());
        }

    }

    public void SpawnPlayers()
    {
        foreach (PlayerCharacter player in ActivePlayers)
        {
            GameObject character = Instantiate(player.GetCharacterController, FindObjectOfType<CombatMove>().transform);
            characters.Add(character);
            character.GetComponent<Player>().MoveSpeed += character.GetComponent<Player>().MoveSpeed * player.GetComponent<CharacterStats>().Speed * .01f; //adjusts speed
            character.GetComponent<Player>().enabled = false;
            character.GetComponent<Player>().MyCharacter = player;
        }
    }

    public IEnumerator EndCombat()
    {
        //If this is the only place combat end move is called this should work, if it gets called from multiple sources it may cause some strange issues
        if(CurrentEnemy != null && CurrentEnemy.IsTrash)
        {
            StartCoroutine(DelayEndMove());
            while(inTrashEndMove)
            {
                yield return null;
            }
        }
        else
        {
            miniGame.GetComponentInChildren<CombatMove>().EndMove();
        }

        miniGame.gameObject.SetActive(true);
        Destroy(miniGame);
        HandleEndOfTurn();
    }

    IEnumerator DelayEndMove()
    {
        inTrashEndMove = true;

        //Loop over all the trash enemys
        foreach (var enemy in EnemyManager.Instance.TrashMobCollection)
        {
            if (enemy.Key == CurrentEnemy.CharacterName)
            {
                //Found the enemy type and now loop over them all to deal damage independently
                for (int i = 0; i < enemy.Value.Count; i++)
                {
                    CurrentEnemy = enemy.Value[i];

                    if (!CurrentEnemy.Stunned)
                        miniGame.GetComponentInChildren<CombatMove>().EndMove();
                    else
                        CurrentEnemy.Stun(false);
                    yield return new WaitForSeconds(trashAttackDelay);
                }
            }
        }

        inTrashEndMove = false;
    }

    public void HandleEndOfTurn()
    {
        CharactersSelected.Clear();

        //Handle EoT augments (store this data and display visuals in EndCombat()?
        AugmentStackSO[] allAugments = FindObjectsOfType<AugmentStackSO>();

        foreach (AugmentStackSO augment in allAugments)
        {
            if (augment.EveryTurnEnd)
                augment.AugmentEffect();
            if (augment.InCombat && augment.GetAugment() != null)
                augment.GetAugment().StopEffect();
        }

        TurnOrder.Instance.EndTurn();
    }

    public void CleanupMinigame()
    {
        //Clean up INA
        foreach (GameObject character in characters)
        {
            Destroy(character);
        }

        miniGame.gameObject.SetActive(false);
    }

    private void OnDisable() => Instance = null;

    public bool CheckPlayerSummonLayer(Character summon)
    {
        if(summon != null && !summon.Dead)
            return true;
        return false;
    }

    [ContextMenu("Combat start effects")]
    public void StartCharacterCombatEffects()
    {
        //Call Start combat effect for all characters in scene
        foreach (Character character in FindObjectsOfType<Character>())
            character.StartCombatEffect();
    }

    [ContextMenu("Combat end effects")]
    public void EndCharacterCombatEffects()
    {
        //Call Start combat effect for all characters in scene
        foreach (Character character in FindObjectsOfType<Character>())
            character.EndCombatEffect();
    }

    #region Combat Star

    public float SpawnCombatStar(EnumManager.CombatOutcome outcome, string text, int sortingLayer, Transform combatStarSpawn)
    {
        // Set combat star material to damage by default
        Material combatStarMaterial = DamageStarMaterial;

        // Choose what material to use
        switch (outcome)
        {
            case EnumManager.CombatOutcome.MultiHealing:
            case EnumManager.CombatOutcome.Resurrect:
            case EnumManager.CombatOutcome.Healing:
                combatStarMaterial = HealingStarMaterial;
                break;
            case EnumManager.CombatOutcome.Shield:
                combatStarMaterial = ShieldStarMaterial;
                break;
            default:
                break;
        }

        // Choose either the base combat star or the UI Combat Star
        GameObject starToSpawn = InCombat ? BaseCombatStar : UiCombatStar; 

        // Create the Combat Star at the star spwan position
        GameObject star = Instantiate(starToSpawn, combatStarSpawn.position, Quaternion.identity, GameObject.Find("DungeonCombat").transform);
        star.name = outcome.ToString() + star.name; 

        Vector3 offset = new Vector3(UnityEngine.Random.Range(-CombatStarMaxOffset, CombatStarMaxOffset),
            UnityEngine.Random.Range(-CombatStarMaxOffset, CombatStarMaxOffset), 0);
        Vector3 newPosition = star.transform.position + offset;

        int triesPerStar = 0;

        // Determine if the Combat Star can be created at this position or if it is overlapping with another star
        while (!CanSpawnCombatStarAtLocation((Vector2)newPosition, triesPerStar, star))
        {
            triesPerStar++;
            Debug.Log("Star couldn't be created at previous spot! Creating new position!");
            offset = new Vector3(UnityEngine.Random.Range(-CombatStarMaxOffset, CombatStarMaxOffset),
                UnityEngine.Random.Range(-CombatStarMaxOffset, CombatStarMaxOffset), 0);
            newPosition = star.transform.position + offset;
        }

        star.transform.position = newPosition;
        return star.GetComponent<CombatStar>().SetupStar(text, combatStarMaterial, sortingLayer);
    }

    private bool CanSpawnCombatStarAtLocation(Vector2 spawnPoint, int triesPerStar, GameObject star)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(spawnPoint, CombatStarSpawnCheckRadius);

        // If it's the first star or it's tried to spawn over 10 times, just spawn it
        if (colliders.Length > 1 && triesPerStar < 10)
        {
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject.GetComponent<CombatStar>() != null && collider.gameObject != star)
                    return false;
            }
        }

        return true;
    }
    #endregion
}
