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

    private void Awake() => Instance = this;

    void Start()
    {
        //Instantiate(testMockup)
        PlayerCombatSpots = new Character[12];
        EnemyCombatSpots = new Character[12];
        
        SetSpawns();

        Enemy[] foundEnemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in foundEnemies)
            if(!enemy.GetComponent<EnemySummon>())
                Enemies.Add(enemy);


        PlayerCharacter[] foundPlayers = FindObjectsOfType<PlayerCharacter>();
        foreach (PlayerCharacter player in foundPlayers)
            if(!player.GetComponent<PlayerCharacterSummon>())
                Players.Add(player);

        PlayerVesselManager.Instance.Initialize();

        FuckTrashMobs();
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
                prefab.GetComponentInChildren<SpriteRenderer>().sortingOrder = enemyCount;

                //TODO Figure out enemy spacing
                for (int i = enemyCount; i < prefab.GetComponent<Character>().GetSpaceTaken + enemyCount; i++)
                {
                    EnemyCombatSpots[i] = prefab.GetComponent<Character>();
                    prefab.GetComponent<Character>().CombatSpot = i;
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
                prefab.GetComponent<SpriteRenderer>().sortingOrder = enemySummonCount;
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
                prefab.GetComponent<SpriteRenderer>().sortingOrder = playerCount;
                PlayerCombatSpots[playerCount] = prefab.GetComponent<Character>();
                prefab.GetComponent<Character>().CombatSpot = playerCount;
                playerCount += prefab.GetComponent<Character>().GetSpaceTaken;
            }
        }

        foreach (GameObject playerSummon in PlayerSummonsToSpawn)
        {
            if (playerSummonCount < PlayerSummonLocations.Length)
            {
                GameObject prefab = Instantiate(playerSummon, PlayerSummonPrefabLocation);
                prefab.transform.position = PlayerSummonLocations[playerSummonCount].position;
                prefab.GetComponent<SpriteRenderer>().sortingOrder = playerSummonCount;
                PlayerCombatSpots[playerSummonCount + playerSummonIndexAdder] = prefab.GetComponent<Character>();
                prefab.GetComponent<Character>().CombatSpot = playerSummonCount + playerSummonIndexAdder;
                playerSummonCount += prefab.GetComponent<Character>().GetSpaceTaken;
            }
        }
    }

    //Can be called to reset the trash collection if a mob dies or is added to collection
    public void FuckTrashMobs()
    {
        TrashMobCollection = new();

        foreach (var mob in Enemies)
        {
            if (mob.isTrash && !mob.Dead)
            {
                if (!TrashMobCollection.ContainsKey(mob.CharacterName))
                    TrashMobCollection.Add(mob.CharacterName, new());

                TrashMobCollection[mob.CharacterName].Add(mob);
            }
        }
    }
}
