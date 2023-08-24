using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("For testing placement")]
    [SerializeField] List<GameObject> EnemiesToSpawn;
    [SerializeField] List<Transform> EnemyLocations;
    [SerializeField] Transform EnemyPrefabLocation;

    [Space(10)]
    public List<Enemy> Enemies;
    public List<PlayerCharacter> Players;

    private void Awake()
    {
        int count = 0;

        foreach(var enemy in EnemiesToSpawn)
        {
            if(count < EnemyLocations.Count)
            {
                GameObject prefab = Instantiate(enemy, EnemyPrefabLocation);
                prefab.transform.position = EnemyLocations[count].position;
                prefab.GetComponent<SpriteRenderer>().sortingOrder = -count;
                count += prefab.GetComponent<Enemy>().GetSpaceTaken;
            }
        }
    }

    void Start()
    {
        Enemy[] foundEnemies = FindObjectsOfType<Enemy>();
        foreach(Enemy enemy in foundEnemies)
            Enemies.Add(enemy);

        PlayerCharacter[] foundPlayers = FindObjectsOfType<PlayerCharacter>();
        foreach (PlayerCharacter player in foundPlayers)
            Players.Add(player);
    }
}
