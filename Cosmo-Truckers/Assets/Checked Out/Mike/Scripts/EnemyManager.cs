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
    [SerializeField] Transform EnemyPrefabLocation;
    [SerializeField] Transform EnemySummonPrefabLocation;
    [SerializeField] Transform PlayerPrefabLocation;
    [SerializeField] Transform PlayerSummonPrefabLocation;

    //TEST
    [SerializeField] GameObject testMockup;

    [Space(10)]
    [HideInInspector] public List<Enemy> Enemies;
    [HideInInspector] public List<PlayerCharacter> Players;
    [HideInInspector] public List<PlayerCharacterSummon> PlayerSummons;
    [HideInInspector] public List<EnemySummon> EnemySummons;

    private void Awake() => Instance = this;

    void Start()
    {
        //Instantiate(testMockup)

        SetSpawns();

        Enemy[] foundEnemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in foundEnemies)
            Enemies.Add(enemy);

        PlayerCharacter[] foundPlayers = FindObjectsOfType<PlayerCharacter>();
        foreach (PlayerCharacter player in foundPlayers)
            Players.Add(player);

        PlayerVesselManager.Instance.Initialize();
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
                enemyCount += prefab.GetComponent<Character>().GetSpaceTaken;

                //TODO Figure out enemy spacing
                for (int i = enemyCount; i < prefab.GetComponent<Character>().GetSpaceTaken + enemyCount; i++)
                    prefab.GetComponent<Character>().CombatSpot.Add(i);
            }
        }

        foreach (GameObject enemySummon in EnemySummonsToSpawn)
        {
            if (enemySummonCount < EnemySummonLocations.Length)
            {
                GameObject prefab = Instantiate(enemySummon, EnemySummonPrefabLocation);
                prefab.transform.position = EnemySummonLocations[enemySummonCount].position;
                prefab.GetComponent<SpriteRenderer>().sortingOrder = enemySummonCount;
                enemySummonCount += prefab.GetComponent<Character>().GetSpaceTaken;
                prefab.GetComponent<Character>().CombatSpot.Add(enemySummonCount);
            }
        }

        foreach (GameObject player in PlayersToSpawn)
        {
            if (playerCount < PlayerLocations.Length)
            {
                GameObject prefab = Instantiate(player, PlayerPrefabLocation);
                prefab.transform.position = PlayerLocations[playerCount].position;
                prefab.GetComponent<SpriteRenderer>().sortingOrder = playerCount;
                playerCount += prefab.GetComponent<Character>().GetSpaceTaken;
                prefab.GetComponent<Character>().CombatSpot.Add(playerCount);
            }
        }

        foreach (GameObject playerSummon in PlayerSummonsToSpawn)
        {
            if (playerSummonCount < PlayerSummonLocations.Length)
            {
                GameObject prefab = Instantiate(playerSummon, PlayerSummonPrefabLocation);
                prefab.transform.position = PlayerSummonLocations[playerSummonCount].position;
                prefab.GetComponent<SpriteRenderer>().sortingOrder = playerSummonCount;
                playerSummonCount += prefab.GetComponent<Character>().GetSpaceTaken;
                prefab.GetComponent<Character>().CombatSpot.Add(playerSummonCount);
            }
        }
    }
}
