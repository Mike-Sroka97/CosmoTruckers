using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_StarStruckSpawner : Augment
{
    [SerializeField] GameObject star;

    public List<(Enemy, int)> AliveEnemies = new List<(Enemy, int)>();
    const float xClamp = 5.9f;
    const float minSpawn = 5.0f;
    float spawnSpeed;

    public override void Activate(DebuffStackSO stack = null)
    {
        base.Activate(stack);
        spawnSpeed = minSpawn - StatusEffect;

        //find aliveEnemies
         foreach(Enemy enemy in EnemyManager.Instance.Enemies)
            if (!enemy.Dead)
                AliveEnemies.Add((enemy, 0));

        StartCoroutine(SpawnStar());
    }

    public override void StopEffect()
    {
        //COLE TODO Heals enemies (put on a delay????)
        foreach((Enemy, int) enemy in AliveEnemies)
        {
            if(!enemy.Item1.Dead)
            {
                Enemy tempEnemy = enemy.Item1;
                tempEnemy.TakeHealing(enemy.Item2);
                //Debug.Log("Healing " + tempEnemy.name + " for " + enemy.Item2 + " health.");
            }
        }
        AliveEnemies.Clear();
        StopAllCoroutines();
    }

    IEnumerator SpawnStar()
    {
        yield return new WaitForSeconds(spawnSpeed);
        float randomX = Random.Range(-xClamp, xClamp);
        GameObject tempStar;

        if (randomX >= 0)
        {
            tempStar = Instantiate(star, new Vector3(randomX, transform.position.y, transform.position.z), star.transform.rotation);
            tempStar.transform.eulerAngles = new Vector3(0, 0, 225);
        }
        else
        {
            tempStar = Instantiate(star, new Vector3(randomX, transform.position.y, transform.position.z), star.transform.rotation);
        }

        tempStar.transform.parent = CombatManager.Instance.GetMiniGame.transform; // this.gameObject.transform;

        StartCoroutine(SpawnStar());
    }
}
