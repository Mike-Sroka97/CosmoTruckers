using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<Enemy> Enemies;
    public List<PlayerCharacter> Players;

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
