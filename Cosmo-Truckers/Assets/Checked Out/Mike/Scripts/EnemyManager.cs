using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [HideInInspector] public static EnemyManager Instance;

    [Header("For testing placement")]
    public List<GameObject> EnemiesToSpawn;
    public List<GameObject> EnemySummonsToSpawn;
    public List<GameObject> PlayersToSpawn;
    public List<GameObject> PlayerSummonsToSpawn;
    public Transform[] EnemyLocations;
    public Transform[] EnemySummonLocations;
    public Transform[] PlayerLocations;
    public Transform[] PlayerSummonLocations;
    public Character[] PlayerCombatSpots; //0-3 players, 4-7 player summons
    public Character[] EnemyCombatSpots; //0-7 enemies, 8-11 enemy summons
    [SerializeField] Transform EnemyPrefabLocation;
    [SerializeField] Transform EnemySummonPrefabLocation;
    [SerializeField] Transform PlayerPrefabLocation;
    [SerializeField] Transform PlayerSummonPrefabLocation;

    //TEST
    [SerializeField] GameObject testMockup;

    [Space(10)]
    [HideInInspector] public List<Enemy> Enemies;
    [HideInInspector] public Dictionary<string, List<Enemy>> TrashMobCollection; //Test for now
    [HideInInspector] public List<PlayerCharacter> Players;
    [HideInInspector] public List<PlayerCharacterSummon> PlayerSummons;
    [HideInInspector] public List<EnemySummon> EnemySummons;

    const int charactersPerColumn = 4;
    const int enemySummonIndexAdder = 8;
    public int playerSummonIndexAdder = 4;

    private void Awake() 
    { 
        Instance = this; 
        if(CombatData.Instance != null)
        {
            EnemiesToSpawn.Clear();
            testMockup = CombatData.Instance.EnemysToSpawn;

            PlayersToSpawn.Clear();
            foreach (var player in CombatData.Instance.PlayersToSpawn)
                PlayersToSpawn.Add(player.GetPlayer.CombatPlayerSpawn);
        }
    }

    void Start()
    {
        if (testMockup != null)
        {
            GameObject mockUp = Instantiate(testMockup);

            Enemy[] spawnedEnemys = FindObjectsOfType<Enemy>();
            foreach (Enemy enemy in spawnedEnemys)
                if (!enemy.GetComponent<EnemySummon>())
                    EnemiesToSpawn.Add(enemy.gameObject);

            PlayerCombatSpots = new Character[8];
            EnemyCombatSpots = new Character[12];

            SetSpawns();

            //Not great on overhead, but waiting till EOF will cause a null enemy to be in the list
            DestroyImmediate(mockUp);
        }
        else
        {
            PlayerCombatSpots = new Character[8];
            EnemyCombatSpots = new Character[12];

            SetSpawns();
        }

        Enemy[] foundEnemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in foundEnemies)
            if (!enemy.GetComponent<EnemySummon>())
                Enemies.Add(enemy);
            else
                EnemySummons.Add(enemy.GetComponent<EnemySummon>());

        int currentPlayerNumber = 1;
        PlayerCharacter[] foundPlayers = FindObjectsOfType<PlayerCharacter>();
        foreach (PlayerCharacter player in foundPlayers)
            if(!player.GetComponent<PlayerCharacterSummon>())
            {
                player.PlayerNumber = currentPlayerNumber;
                currentPlayerNumber++;
                Players.Add(player);
            }
            else
            {
                PlayerSummons.Add(player.GetComponent<PlayerCharacterSummon>());
            }


        PlayerVesselManager.Instance.Initialize();

        UpdateTrashMobList();
    }

    private void SetSpawns()
    {
        int enemyCount = 0;
        int enemySummonCount = 0;
        int playerCount = 0;
        int playerSummonCount = 0;

        foreach (GameObject enemy in EnemiesToSpawn)
        {
            if (enemyCount < EnemyLocations.Length)
            {
                GameObject prefab = Instantiate(enemy, EnemyPrefabLocation);
                prefab.transform.position = EnemyLocations[enemyCount].position;
                SpriteRenderer[] spriteRenderers = prefab.GetComponentsInChildren<SpriteRenderer>();
                foreach (SpriteRenderer renderer in spriteRenderers)
                    renderer.sortingOrder += enemyCount;

                //TODO Figure out enemy spacing
                for (int i = enemyCount; i < prefab.GetComponent<Character>().GetSpaceTaken + enemyCount; i++)
                {
                    EnemyCombatSpots[i] = prefab.GetComponent<Character>();
                    prefab.GetComponent<Character>().CombatSpot = i;
                    prefab.name = $"{prefab.name}{i}";
                }

                enemyCount += prefab.GetComponent<Character>().GetSpaceTaken;
            }
        }

        foreach (GameObject enemySummon in EnemySummonsToSpawn)
        {
            if (enemySummonCount < EnemySummonLocations.Length)
            {
                GameObject prefab = Instantiate(enemySummon, EnemySummonPrefabLocation);
                prefab.transform.position = EnemySummonLocations[enemySummonCount].position;
                SpriteRenderer[] spriteRenderers = prefab.GetComponentsInChildren<SpriteRenderer>();
                foreach (SpriteRenderer renderer in spriteRenderers)
                    renderer.sortingOrder += enemySummonCount + 8;
                EnemyCombatSpots[enemySummonCount + enemySummonIndexAdder] = prefab.GetComponent<Character>();
                prefab.GetComponent<Character>().CombatSpot = enemySummonCount + enemySummonIndexAdder;
                enemySummonCount += prefab.GetComponent<Character>().GetSpaceTaken;
            }
        }

        foreach (GameObject player in PlayersToSpawn)
        {
            if (playerCount < PlayerLocations.Length)
            {
                GameObject prefab = Instantiate(player, PlayerPrefabLocation);
                prefab.transform.position = PlayerLocations[playerCount].position;
                SpriteRenderer[] spriteRenderers = prefab.GetComponentsInChildren<SpriteRenderer>();
                foreach (SpriteRenderer renderer in spriteRenderers)
                    renderer.sortingOrder += playerCount;
                PlayerCombatSpots[playerCount] = prefab.GetComponent<Character>();
                prefab.GetComponent<Character>().CombatSpot = playerCount;
                playerCount += prefab.GetComponent<Character>().GetSpaceTaken;

                SetPlayerSaveData(prefab.GetComponent<PlayerCharacter>());
            }
        }

        foreach (GameObject playerSummon in PlayerSummonsToSpawn)
        {
            if (playerSummonCount < PlayerSummonLocations.Length)
            {
                GameObject prefab = Instantiate(playerSummon, PlayerSummonPrefabLocation);
                prefab.transform.position = PlayerSummonLocations[playerSummonCount].position;
                SpriteRenderer[] spriteRenderers = prefab.GetComponentsInChildren<SpriteRenderer>();
                foreach (SpriteRenderer renderer in spriteRenderers)
                    renderer.sortingOrder += playerSummonCount + 4;
                PlayerCombatSpots[playerSummonCount + playerSummonIndexAdder] = prefab.GetComponent<Character>();
                prefab.GetComponent<Character>().CombatSpot = playerSummonCount + playerSummonIndexAdder;
                playerSummonCount += prefab.GetComponent<Character>().GetSpaceTaken;
            }
        }
    }

    public void UpdateEnemySummons(GameObject summon)
    {        
        for(int i = 8; i < 12; i++)
        {
            if(EnemyCombatSpots[i] == null)
            {
                GameObject newSummon = Instantiate(summon, EnemySummonPrefabLocation);
                EnemyCombatSpots[i] = newSummon.GetComponent<EnemySummon>();
                newSummon.transform.position = EnemySummonLocations[i - 8].position;
                foreach(SpriteRenderer spriteRenderer in newSummon.GetComponentsInChildren<SpriteRenderer>())
                    spriteRenderer.sortingOrder += i;
                EnemyCombatSpots[i] = newSummon.GetComponent<Character>();
                EnemyCombatSpots[i].CombatSpot = i;
                CharacterStats stats = newSummon.GetComponent<CharacterStats>();
                TurnOrder.Instance.AddToSpeedList(stats);
                break;
            }
        }

        TurnOrder.Instance.DetermineTurnOrder();
    }

    public List<PlayerCharacter> GetAlivePlayerCharacters()
    {
        List<PlayerCharacter> alivePlayers = new List<PlayerCharacter>();

        foreach (PlayerCharacter player in Players)
            if (!player.Dead && !player.GetComponent<PlayerCharacterSummon>())
                alivePlayers.Add(player);

        return alivePlayers;
    }

    public List<Enemy> GetAliveEnemies()
    {
        List<Enemy> aliveEnemies = new List<Enemy>();

        foreach (Enemy enemy in Enemies)
            if (!enemy.Dead && !enemy.GetComponent<EnemySummon>())
                aliveEnemies.Add(enemy);

        return aliveEnemies;
    }

    public List<PlayerCharacterSummon> GetAlivePlayerSummons()
    {
        List<PlayerCharacterSummon> aliveSummons = new List<PlayerCharacterSummon>();

        foreach (PlayerCharacterSummon summon in PlayerSummons)
            if (!summon.Dead)
                aliveSummons.Add(summon.GetComponent<PlayerCharacterSummon>());

        return aliveSummons;
    }

    public List<EnemySummon> GetAliveEnemySummons()
    {
        List<EnemySummon> aliveSummons = new List<EnemySummon>();

        foreach (EnemySummon enemy in EnemySummons)
            if (!enemy.Dead)
                aliveSummons.Add(enemy.GetComponent<EnemySummon>());

        return aliveSummons;
    }

    //Can be called to reset the trash collection if a mob dies or is added to collection
    public void UpdateTrashMobList()
    {
        TrashMobCollection = new Dictionary<string, List<Enemy>>();

        foreach (Enemy mob in Enemies)
        {
            if (mob.IsTrash && !mob.Dead)
            {
                if (!TrashMobCollection.ContainsKey(mob.CharacterName))
                    TrashMobCollection.Add(mob.CharacterName, new List<Enemy>());

                TrashMobCollection[mob.CharacterName].Add(mob);
            }
        }
    }

    void SetPlayerSaveData(PlayerCharacter character)
    {
        if (!CombatData.Instance) return;

        foreach(var player in CombatData.Instance.PlayersToSpawn)
        {
            if(player.GetPlayer.CombatPlayerSpawn.GetComponent<PlayerCharacter>().CharacterName == character.CharacterName)
            {
                SaveData playerData = SaveManager.Load(player.GetPlayer.PlayerID);

                //If some how you made it here with out the manager saving any data
                if (playerData == null) break;

                foreach (var aug in playerData.PlayerCurrentDebuffs)
                {
                    character.AddDebuffStack(aug);
                }

                //If the players health has changed from last combat
                character.Health = playerData.PlayerCurrentHP == -1 ? character.Health : playerData.PlayerCurrentHP;
            }
        }
    }

    /// <summary>
    /// Save the current players combat data
    /// </summary>
    /// <param name="character">The character to save</param>
    public void SavePlayerData(PlayerCharacter character)
    {
        if (!CombatData.Instance) return;

        foreach (var player in CombatData.Instance.PlayersToSpawn)
        {
            if (player.GetPlayer.CombatPlayerSpawn.GetComponent<PlayerCharacter>().CharacterName == character.CharacterName)
            {
                player.GetPlayerData.PlayerCurrentHP = character.CurrentHealth;
                player.GetPlayerData.PlayerCurrentDebuffs.Clear();

                foreach (var aug in character.GetAUGS)
                    player.GetPlayerData.PlayerCurrentDebuffs.Add(aug);

                SaveManager.Save(player.GetPlayerData, player.GetPlayer.PlayerID);
            }
        }
    }
}
