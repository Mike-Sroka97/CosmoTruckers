using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<Enemy> Enemies;
    void Start()
    {
        Enemy[] foundEnemies = FindObjectsOfType<Enemy>();
        foreach(Enemy enemy in foundEnemies)
        {
            Enemies.Add(enemy);
        }
    }
}
